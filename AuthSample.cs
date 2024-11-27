//namespace BugandFixSoftwareCompany;

//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

//builder.Services.AddAuthorization(); // Add authorization support

//var app = builder.Build();

//// Add authentication and authorization middleware
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapPost("/generate-token", (HttpContext context) =>
//{
//    var tokenHandler = new JwtSecurityTokenHandler();
//var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

//var tokenDescriptor = new SecurityTokenDescriptor
//{
//    Subject = new ClaimsIdentity(new[]
//    {
//            new Claim(ClaimTypes.Name, "TestUser"),
//            new Claim(ClaimTypes.Role, "Admin") // Add custom roles/claims if needed
//        }),
//    Expires = DateTime.UtcNow.AddHours(1),
//    Issuer = builder.Configuration["Jwt:Issuer"],
//    Audience = builder.Configuration["Jwt:Audience"],
//    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//};

//var token = tokenHandler.CreateToken(tokenDescriptor);
//    return Results.Ok(new { token = tokenHandler.WriteToken(token) });
//});


//app.MapGet("/plain-string", () => "Hello, World!")
//    .RequireAuthorization(); // Secure this endpoint with authentication



//// Define a secure group of endpoints
//var secureGroup = app.MapGroup("/secure")
//    .RequireAuthorization(); // Apply to all endpoints in this group

//// Add endpoints to the secure group
//secureGroup.MapGet("/data", () => "This is secure data!");
//secureGroup.MapPost("/create", () => "Securely created something!");

//// Public endpoints remain unaffected
//app.MapGet("/public", () => "This is public data.");



//app.UseSwagger();
//app.UseSwaggerUI();

//app.Run();

