using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Microsoft.IdentityModel.Tokens;
using AppointmentAPI.Data;
using Microsoft.EntityFrameworkCore;
using AppointmentAPI.Middleware;
using System.Text;

//MVC

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddOpenApi();
builder.Services.AddControllers(); // it use 
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Title = "Appointment API",
//         Version = "v1"
//     });

//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Enter: Bearer {your JWT token}"
//     });

//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new List<string>()
//         }
//     });
// });

builder.Services.AddSwaggerGen(c =>
{
    const string schemeId = "Bearer";

    c.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });

    // .NET 10 version using the "doc" delegate and explicit dictionary assignment
    c.AddSecurityRequirement(doc => 
    {
        var requirement = new OpenApiSecurityRequirement();
        var scheme = new OpenApiSecuritySchemeReference(schemeId, doc);
        
        // Use the bracket [ ] syntax instead of the { } initializer
        requirement[scheme] = new List<string>(); 
        
        return requirement;
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});



// Database registration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// Global exception handling (VERY IMPORTANT ORDER)
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}







// ------------------------------------



// using AppointmentAPI.Data;
// using AppointmentAPI.Middleware;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi;
// // using Microsoft.OpenApi.Models;   
// using System.Collections.Generic;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// // 1. Controllers
// builder.Services.AddControllers();

// // 2. Swagger (Using Swashbuckle)
// builder.Services.AddEndpointsApiExplorer();
// // builder.Services.AddSwaggerGen(c =>
// // {
// //     const string schemeId = "Bearer";

// //     c.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
// //     {
// //         Name = "Authorization",
// //         Type = SecuritySchemeType.Http,
// //         Scheme = "bearer",
// //         BearerFormat = "JWT",
// //         In = ParameterLocation.Header,
// //         Description = "Enter 'Bearer' [space] and then your valid token."
// //     });

// //     c.AddSecurityRequirement(new OpenApiSecurityRequirement
// //     {
// //         {
// //             new OpenApiSecurityScheme
// //             {
// //                 Reference = new OpenApiReference
// //                 {
// //                     Type = ReferenceType.SecurityScheme,
// //                     Id = "Bearer"
// //                 }
// //             },
// //             Array.Empty<string>()
// //         }
// //     });
// // });
// //-----------------------------------------------------
// // builder.Services.AddSwaggerGen(c =>
// // {
// //     const string schemeId = "Bearer";

// //     // 1. Define the Security Scheme
// //     c.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
// //     {
// //         Name = "Authorization",
// //         Type = SecuritySchemeType.Http,
// //         Scheme = "bearer",
// //         BearerFormat = "JWT",
// //         In = ParameterLocation.Header,
// //         // Description = "Enter 'Bearer' [space] and then your valid token."
// //     });

// //     // 2. Add the Requirement using the .NET 10 Delegate Pattern
// //     c.AddSecurityRequirement(new OpenApiSecurityRequirement
// //     {
// //         {
// //             new OpenApiSecurityScheme
// //             {
// //                 Reference = new OpenApiReference
// //                 {
// //                     Type = ReferenceType.SecurityScheme,
// //                     Id = schemeId
// //                 }
// //             },
// //             Array.Empty<string>()
// //         }
// //     });
// // });
// // /---------------------------------------

// builder.Services.AddSwaggerGen(c =>
// {
//     const string schemeId = "Bearer";

//     c.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
//     {
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer", 
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Description = "Enter 'Bearer' [space] and then your valid token."
//     });

//     // .NET 10 version using the "doc" delegate and explicit dictionary assignment
//     c.AddSecurityRequirement(doc => 
//     {
//         var requirement = new OpenApiSecurityRequirement();
//         var scheme = new OpenApiSecuritySchemeReference(schemeId, doc);
        
//         // Use the bracket [ ] syntax instead of the { } initializer
//         requirement[scheme] = new List<string>(); 
        
//         return requirement;
//     });
// });

// // 3. Database
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("DefaultConnection")
//     ));

// // 4. JWT Authentication Fix
// // We pull the key into a variable first to avoid the Null Reference warning
// var jwtKey = builder.Configuration["Jwt:Key"] ?? "Your_Default_Super_Secret_Key_32_Chars_Long";
// var issuer = builder.Configuration["Jwt:Issuer"] ?? "AppointmentAPI";
// var audience = builder.Configuration["Jwt:Audience"] ?? "AppointmentAPIUsers";

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = issuer,
//             ValidAudience = audience,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//         };
//     });

// var app = builder.Build();

// // 5. Middleware Pipeline
// if (app.Environment.IsDevelopment())
// {
//     // These two lines enable the Swagger UI page
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseMiddleware<ExceptionMiddleware>();

// app.UseHttpsRedirection();

// // UseAuthentication must come BEFORE UseAuthorization
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapControllers();

// app.Run();
