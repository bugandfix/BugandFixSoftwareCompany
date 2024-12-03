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

        // Other middleware configurations can go here (e.g., Authentication, CORS)
        return app;
    }
}
