using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Implementations;
using BugandFixSoftwareCompany.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BugandFixSoftwareCompany.Extensions;

public static class SoftwareDeveloperServiceExtensions
{
    public static IServiceCollection AddSoftwareDeveloperApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Dependency Injection
        services.AddScoped<ISoftwareDeveloperRepository, SoftwareDeveloperRepository>();
        services.AddScoped<ISoftwareDeveloperService, SoftwareDeveloperService>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<SoftwareDeveloperValidator>();

        return services;
    }
}
