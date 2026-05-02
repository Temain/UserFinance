using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiGateway.OpenApi;

public sealed class GatewayDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Components ??= new OpenApiComponents();
        swaggerDoc.Components.Schemas = CreateSchemas();
        swaggerDoc.Paths = new OpenApiPaths
        {
            ["/health"] = CreateHealthPathItem(),
            ["/api/auth/login"] = CreateOperationPathItem(OperationType.Post, "Login via gateway and receive JWT access token.",
                false, "LoginUserRequest"),
            ["/api/auth/logout"] = CreateOperationPathItem(OperationType.Post,
                "Logout via gateway using the current JWT access token.", true),
            ["/api/users"] = CreateOperationPathItem(OperationType.Post, "Register a new user via gateway.", false,
                "RegisterUserRequest"),
            ["/api/users/{userId}"] = CreateOperationPathItem(OperationType.Get, "Get user profile via gateway.", true, null,
                "userId"),
            ["/api/users/{userId}/currencies"] = CreateCurrenciesPathItem(),
            ["/api/users/{userId}/currencies/{currencyId}"] = CreateOperationPathItem(OperationType.Delete,
                "Remove a currency from the current user via gateway.", true, null, "userId", "currencyId"),
            ["/api/users/{userId}/currency-rates"] = CreateOperationPathItem(OperationType.Get,
                "Get all currency rates for the current user via gateway.", true, null, "userId"),
            ["/api/users/{userId}/currency-rates/{currencyId}"] = CreateOperationPathItem(OperationType.Get,
                "Get one currency rate for the current user via gateway.", true, null, "userId", "currencyId")
        };
    }

    private static OpenApiPathItem CreateHealthPathItem()
    {
        return new OpenApiPathItem
        {
            Operations =
            {
                [OperationType.Get] = new OpenApiOperation
                {
                    Summary = "Gateway health check",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse
                        {
                            Description = "Gateway is healthy."
                        }
                    }
                }
            }
        };
    }

    private static OpenApiPathItem CreateCurrenciesPathItem()
    {
        return new OpenApiPathItem
        {
            Operations =
            {
                [OperationType.Get] = CreateOperation(OperationType.Get, "Get user currencies via gateway.", true, null,
                    "userId"),
                [OperationType.Post] = CreateOperation(OperationType.Post,
                    "Add one or more currencies to the current user via gateway.", true, "AddUserCurrenciesRequest",
                    "userId")
            }
        };
    }

    private static OpenApiPathItem CreateOperationPathItem(OperationType operationType, string summary, bool requiresAuth,
        string? requestBodySchemaId = null, params string[] parameterNames)
    {
        return new OpenApiPathItem
        {
            Operations =
            {
                [operationType] = CreateOperation(operationType, summary, requiresAuth, requestBodySchemaId, parameterNames)
            }
        };
    }

    private static OpenApiOperation CreateOperation(OperationType operationType, string summary, bool requiresAuth,
        string? requestBodySchemaId = null, params string[] parameterNames)
    {
        var operation = new OpenApiOperation
        {
            Summary = summary,
            Parameters = parameterNames.Select(CreatePathParameter).ToList(),
            Responses = new OpenApiResponses
            {
                ["200"] = new OpenApiResponse
                {
                    Description = "Success."
                }
            }
        };

        if (!string.IsNullOrWhiteSpace(requestBodySchemaId))
        {
            operation.RequestBody = CreateRequestBody(requestBodySchemaId);
        }

        if (operationType == OperationType.Post)
        {
            operation.Responses["201"] = new OpenApiResponse
            {
                Description = "Created."
            };
        }

        if (operationType == OperationType.Delete)
        {
            operation.Responses["204"] = new OpenApiResponse
            {
                Description = "No content."
            };
        }

        if (requiresAuth)
        {
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }
                    ] = Array.Empty<string>()
                }
            ];
        }

        if (operationType == OperationType.Get && parameterNames.Length > 0)
        {
            operation.Responses["403"] = new OpenApiResponse
            {
                Description = "Forbidden."
            };
        }

        return operation;
    }

    private static OpenApiParameter CreatePathParameter(string parameterName)
    {
        return new OpenApiParameter
        {
            Name = parameterName,
            In = ParameterLocation.Path,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "integer",
                Format = parameterName == "currencyId" ? "int32" : "int64"
            },
            Example = parameterName == "currencyId" ? new OpenApiInteger(840) : new OpenApiLong(1)
        };
    }

    private static OpenApiRequestBody CreateRequestBody(string schemaId)
    {
        return new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new()
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = schemaId
                        }
                    }
                }
            }
        };
    }

    private static Dictionary<string, OpenApiSchema> CreateSchemas()
    {
        return new Dictionary<string, OpenApiSchema>
        {
            ["LoginUserRequest"] = CreateAuthRequestSchema(),
            ["RegisterUserRequest"] = CreateAuthRequestSchema(),
            ["AddUserCurrenciesRequest"] = new()
            {
                Type = "object",
                Required = new HashSet<string>
                {
                    "currencyIds"
                },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    ["currencyIds"] = new()
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "integer",
                            Format = "int32"
                        },
                        Example = new OpenApiArray
                        {
                            new OpenApiInteger(840),
                            new OpenApiInteger(978),
                            new OpenApiInteger(156)
                        }
                    }
                }
            }
        };
    }

    private static OpenApiSchema CreateAuthRequestSchema()
    {
        return new OpenApiSchema
        {
            Type = "object",
            Required = new HashSet<string>
            {
                "name",
                "password"
            },
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["name"] = new()
                {
                    Type = "string",
                    Example = new OpenApiString("demo-user")
                },
                ["password"] = new()
                {
                    Type = "string",
                    Example = new OpenApiString("secret123")
                }
            }
        };
    }
}
