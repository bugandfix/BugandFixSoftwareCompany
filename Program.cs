
using BugandFixSoftwareCompany.Abstractions;
using BugandFixSoftwareCompany.Data;
using BugandFixSoftwareCompany.Entity;
using BugandFixSoftwareCompany.Implementations;
using BugandFixSoftwareCompany.Validations;
using FluentValidation;
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

//Validation
builder.Services.AddValidatorsFromAssemblyContaining<SoftwareDeveloperValidator>();

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



app.MapPost("/developers", async (SoftwareDeveloper developer, 
    ISoftwareDeveloperService service, 
    CancellationToken cancellationToken , 
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
});


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
});

#region WithOutValidation

//app.MapPost("/developers", async (SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
//{
//    var addedDeveloper = await service.AddAsync(developer, cancellationToken);
//    return TypedResults.Created($"/developers/{addedDeveloper.Id}", addedDeveloper);
//});

//app.MapPut("/developers/{id:int}", async (int id, SoftwareDeveloper developer, ISoftwareDeveloperService service, CancellationToken cancellationToken) =>
//{
//    if (id != developer.Id)
//    {
//        return Results.BadRequest("ID in route does not match ID in body.");
//    }

//    var updatedDeveloper = await service.UpdateAsync(developer, cancellationToken);
//    return updatedDeveloper == null
//        ? TypedResults.NotFound($"Developer with ID {id} not found.")
//        : TypedResults.Ok(updatedDeveloper);
//});

#endregion

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