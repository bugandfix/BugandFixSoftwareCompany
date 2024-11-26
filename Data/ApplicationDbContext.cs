using BugandFixSoftwareCompany.Entity;
using Microsoft.EntityFrameworkCore;

namespace BugandFixSoftwareCompany.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<SoftwareDeveloper> SoftwareDevelopers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SoftwareDeveloper>().HasData(
            new SoftwareDeveloper { Id = 1, Name = "Ali", Specialization = "Backend", Experience = 10 },
            new SoftwareDeveloper { Id = 2, Name = "Reza", Specialization = "Frontend", Experience = 3 },
            new SoftwareDeveloper { Id = 3, Name = "Hamid", Specialization = "DevOps", Experience = 12 }
        );
    }
}
