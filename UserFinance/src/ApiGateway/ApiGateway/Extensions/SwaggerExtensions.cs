namespace ApiGateway.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddApiGatewaySwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseApiGatewaySwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "UserFinance Gateway Swagger";
            options.RoutePrefix = "swagger";
            options.SwaggerEndpoint("/swagger/user/swagger/v1/swagger.json", "User Service API");
            options.SwaggerEndpoint("/swagger/finance/swagger/v1/swagger.json", "Finance Service API");
            options.DisplayRequestDuration();
            options.EnablePersistAuthorization();
        });

        return app;
    }
}
