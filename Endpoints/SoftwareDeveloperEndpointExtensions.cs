using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Entity;
using BugandFixSoftwareCompany.Extensions;
using BugandFixSoftwareCompany.Response;
using BugandFixSoftwareCompany.Result;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace BugandFixSoftwareCompany.Endpoints;

public static class SoftwareDeveloperEndpointExtensions
{
    public static WebApplication MapSoftwareDeveloperEndpoints(this WebApplication app)
    {
        app.MapGet("/error", () => {
            try
            {

            }
            catch 
            {

            }
        });

        //Error endpoint
        app.MapGet("/error", () => {
            throw new InvalidOperationException("This is a test exception");
        });

        // File upload endpoint
        app.MapPost("/upload", async (HttpContext context) =>
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            if (!context.Request.HasFormContentType)
            {
                return Results.BadRequest("The request must be multipart/form-data.");
            }

            var form = await context.Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();

            if (file is null || file.Length == 0)
            {
                return Results.BadRequest("No file was uploaded.");
            }

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Results.Ok(new { Message = "File uploaded successfully.", FileName = file.FileName });
        });

        // File download endpoint
        app.MapGet("/download/{fileName}", async (string fileName, HttpContext context) =>
        {
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var filePath = Path.Combine(uploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return Results.NotFound(new { Message = "File not found." });
            }

            var contentType = "application/octet-stream";

            context.Response.ContentType = contentType;
            context.Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");

            await context.Response.SendFileAsync(filePath);
            return Results.Ok();
        });



        app.MapGet("/developersWithCache", async (IMemoryCache cache, 
            ISoftwareDeveloperService service, 
            CancellationToken cancellationToken) =>
        {
            const string cacheKey = "developers_list";

            // Attempt to retrieve from cache
            if (!cache.TryGetValue(cacheKey, out List<SoftwareDeveloperResponse> developers))
            {
                // Fetch data from service if not in cache
                developers = (List<SoftwareDeveloperResponse>) await service.GetAllAsync(cancellationToken);

                // Add data to cache with a specified expiration time
                cache.Set(cacheKey, developers, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
            }

            return TypedResults.Ok(developers);
        })
        .WithName("ListofSoftwareDevelopersWithCache")
        .Produces<List<SoftwareDeveloperResponse>>(StatusCodes.Status200OK);


        app.MapGet("/developersWithResponseCaching", async (ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
        {
            var developers = await service.GetAllAsync(cancellationToken);
            return TypedResults.Ok(developers);            
        })
        .WithName("ListofSoftwareDevelopersWithResponseCaching")
        .Produces<List<SoftwareDeveloperResponse>>(StatusCodes.Status200OK)
       .WithMetadata(new ResponseCacheAttribute
        {
                 Duration = 60, // Cache for 60 seconds
                 Location = ResponseCacheLocation.Client
        });


        app.MapGet("/developers", async (ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
        {
            var developers = await service.GetAllAsync(cancellationToken);
            return TypedResults.Ok(developers);
        })
        .WithName("ListofSoftwareDevelopers")
        .Produces<List<SoftwareDeveloperResponse>>(StatusCodes.Status200OK);


        app.MapGet("/developers/{id:int}", async (int id, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
            await Task.FromResult<Results<Ok<SoftwareDeveloper>, NotFound<string>>>(await service.GetByIdAsync(id, cancellationToken) is SoftwareDeveloper developer
                ? TypedResults.Ok(developer)
                : TypedResults.NotFound($"Developer with ID {id} not found."))
        )
        .WithName("GetaSoftwareDeveloperById")
        .Produces<SoftwareDeveloperResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);



        //CustomResult
        app.MapGet("/Getbycustomresult/{id:int}", async (int id, ApplicationDbContext context) =>
        {
            var developer = await context.SoftwareDevelopers.FindAsync(id);

            // Use the custom result extension method
            return developer.ToResult();
        })
        .WithName("Getbycustomresult")
        .Produces<SoftwareDeveloperResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);




        app.MapPost("/developers", async (SoftwareDeveloper developer,
            ISoftwareDeveloperService service,
            CancellationToken cancellationToken,
            IValidator<SoftwareDeveloper> validator) =>
        {
            if (developer == null)
            {
                return Results.BadRequest("Invalid request body.");
            }

            // Validate the developer
            var validationResult = await validator.ValidateAsync(developer, cancellationToken);

            // If validation fails, return BadRequest with the error details
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            // If validation passes, proceed with adding the developer
            var addedDeveloper = await service.AddAsync(developer, cancellationToken);
            return Results.Created($"/developers/{addedDeveloper.Id}", addedDeveloper);
        })
        .WithName("AddaSoftwareDeveloper")
        .Produces<SoftwareDeveloperResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);


        app.MapPut("/developers/{id:int}", async (
            int id,
            SoftwareDeveloper developer,
            ISoftwareDeveloperService service,
            IValidator<SoftwareDeveloper> validator,
            CancellationToken cancellationToken) =>
        {
            // Validate the incoming data
            var validationResult = await validator.ValidateAsync(developer, cancellationToken);

            // If validation fails, return BadRequest with the error details
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            // Check if the route ID matches the developer's ID
            if (id != developer.Id)
            {
                return Results.BadRequest("ID in route does not match ID in body.");
            }

            // Update the developer
            var updatedDeveloper = await service.UpdateAsync(developer, cancellationToken);

            // Return the appropriate response
            return updatedDeveloper == null
                ? TypedResults.NotFound($"Developer with ID {id} not found.")
                : TypedResults.Ok(updatedDeveloper);
        })
        .WithName("UpdateaSoftwareDeveloper")
        .Produces<SoftwareDeveloperResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);



        app.MapDelete("/developers/{id:int}", async (int id, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
        {
            var success = await service.DeleteAsync(id, cancellationToken);
            return success
                ? TypedResults.Ok(true)
                : TypedResults.Ok(false);
        })
        .WithName("DeleteaSoftwareDeveloper")
        .Produces<bool>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);


        return app;
    }
}
