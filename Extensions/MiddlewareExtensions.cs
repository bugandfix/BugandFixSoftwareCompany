using AspNetCoreRateLimit;

namespace BugandFixSoftwareCompany.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        //Response Caching
        app.UseResponseCaching();

        // Use the global CORS middleware
        app.UseCors("AllowSpecificOrigins");

        //Error Handling
        app.UseMiddleware<ErrorHandlingMiddleware>();

        //Rate Limiting
        app.UseIpRateLimiting();

        // Other middleware configurations can go here (e.g., Authentication, CORS)
        return app;
    }
}
