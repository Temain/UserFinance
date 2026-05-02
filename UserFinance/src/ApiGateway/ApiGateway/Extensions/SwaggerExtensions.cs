using ApiGateway.OpenApi;
using Microsoft.OpenApi.Models;

namespace ApiGateway.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddApiGatewaySwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("gateway-v1", new OpenApiInfo
            {
                Title = "UserFinance Gateway API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the JWT access token here."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.DocumentFilter<GatewayDocumentFilter>();
        });

        return services;
    }

    public static IApplicationBuilder UseApiGatewaySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "UserFinance Gateway Swagger";
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/gateway-v1/swagger.json", "Gateway API");
            options.SwaggerEndpoint("/swagger/user/swagger/v1/swagger.json", "User Service API");
            options.SwaggerEndpoint("/swagger/finance/swagger/v1/swagger.json", "Finance Service API");
            options.DisplayRequestDuration();
            options.EnablePersistAuthorization();
        });

        return app;
    }
}
