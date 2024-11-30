namespace BugandFixSoftwareCompany.Endpoints;

public static class CORSEndPoint
{
    public static WebApplication MapCORSEndpoints(this WebApplication app)
    {
        app.MapGet("/public-data", () => "This endpoint allows any origin")
           .RequireCors("AllowAllOrigins");

        app.MapPost("/restricted-data", (HttpContext context) =>
        {
            return Results.Ok("This endpoint allows specific origins only.");
        })
            .RequireCors("AllowSpecificOrigins");
        return app;
    }

}