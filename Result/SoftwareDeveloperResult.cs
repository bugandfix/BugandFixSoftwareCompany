using BugandFixSoftwareCompany.Entity;

namespace BugandFixSoftwareCompany.Result;

public class SoftwareDeveloperResult : IResult
{
    private readonly SoftwareDeveloper _developer;

    public SoftwareDeveloperResult(SoftwareDeveloper developer)
    {
        _developer = developer;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        if (_developer == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(new { Message = "Developer not found" });
            return;
        }

        // Customizing the response
        var response = new
        {
            _developer.Id,
            _developer.Name,
            _developer.Specialization,
            _developer.Experience,
            LinkedInProfile = _developer.LinkedInProfile,
            Title = _developer.Experience > 10 ? "Senior" : "Junior"
        };

        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
