namespace BugandFixSoftwareCompany.Response;

public class SoftwareDeveloperResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public string? Title { get; set; } = "Unknown";
    public int Experience { get; set; }
    public string? Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? LinkedInProfile { get; set; }
}