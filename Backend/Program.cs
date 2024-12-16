using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PawpalBackend.Models;
using PawpalBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure DatabaseSettings from appsettings.json
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Register UserService as a singleton
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<PetsService>();
builder.Services.AddSingleton<ServiceServices>();
builder.Services.AddSingleton<BookingService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddEndpointsApiExplorer(); // For minimal APIs
builder.Services.AddSwaggerGen(); // Adds Swagger generation

// Define a CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:8081")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(); // Enables the generation of the Swagger JSON
    app.UseSwaggerUI(); // Enables Swagger UI for visualizing APIs
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

// Enable CORS with the defined policy
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

// for playit.gg
// app.Run("http://0.0.0.0:5272");

// for local development
app.Run();