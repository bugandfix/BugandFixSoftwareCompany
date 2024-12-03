using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Implementations;
using BugandFixSoftwareCompany.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BugandFixSoftwareCompany.Extensions;

public static class GeneralServiceExtensions
{
    public static IServiceCollection AddGeneralApplicationServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // MemoryCaching
        services.AddMemoryCache();

        //Response Caching
        services.AddResponseCaching();


        //CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("https://example.com", "https://anotherdomain.com")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });

            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });


        // Database Context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
