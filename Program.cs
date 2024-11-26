
using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Entity;
using BugandFixSoftwareCompany.Implementations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//Service Registration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Add SQL Server

builder.Services.AddScoped<ISoftwareDeveloperRepository, SoftwareDeveloperRepository>();
builder.Services.AddScoped<ISoftwareDeveloperService, SoftwareDeveloperService>();


var app = builder.Build();
//Middleware Registration Starts


#region EndPoints
app.MapGet("/developers", async (ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
{
    var developers = await service.GetAllAsync(cancellationToken);
    return TypedResults.Ok(developers);
});

app.MapGet("/developers/{id:int}", async (int id, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
    await Task.FromResult<Results<Ok<SoftwareDeveloper>, NotFound<string>>>(await service.GetByIdAsync(id, cancellationToken) is SoftwareDeveloper developer
        ? TypedResults.Ok(developer)
        : TypedResults.NotFound($"Developer with ID {id} not found."))
);

app.MapPost("/developers", async (SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
{
    var addedDeveloper = await service.AddAsync(developer, cancellationToken);
    return TypedResults.Created($"/developers/{addedDeveloper.Id}", addedDeveloper);
});

app.MapPut("/developers/{id:int}", async (int id, SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
{
    if (id != developer.Id)
    {
        return Results.BadRequest("ID in route does not match ID in body.");
    }

    var updatedDeveloper = await service.UpdateAsync(developer, cancellationToken);
    return updatedDeveloper == null
        ? TypedResults.NotFound($"Developer with ID {id} not found.")
        : TypedResults.Ok(updatedDeveloper);
});

app.MapDelete("/developers/{id:int}", async (int id, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
{
    var success = await service.DeleteAsync(id, cancellationToken);
    return success
        ? TypedResults.Ok(true)
        : TypedResults.Ok(false);
});
#endregion


app.UseSwagger();
app.UseSwaggerUI();

//Middleware Registration Stops
app.Run();