using Application.Helper;
using Application.Repository.LoginRepo;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// =====================================================
// CORS
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://shaheedbhagatsinghhealthandeducation.com",
                "https://soft.shaheedbhagatsinghhealthandeducation.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =====================================================
// Controllers
// =====================================================
builder.Services.AddControllers();

// =====================================================
// Database
// =====================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =====================================================
// Swagger
// =====================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =====================================================
// Dependency Injection
// =====================================================
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// =====================================================
// JWT Configuration
// =====================================================
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key is missing in appsettings.json"
    );
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

// =====================================================
// Build Application
// =====================================================
var app = builder.Build();

// =====================================================
// HTTPS
// =====================================================
app.UseHttpsRedirection();

// =====================================================
// Swagger
// =====================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// =====================================================
// Static Files
// =====================================================
app.UseStaticFiles();

// =====================================================
// Routing
// =====================================================
app.UseRouting();

// =====================================================
// CORS
// IMPORTANT:
// Must be after Routing
// Must be before Authentication/Authorization
// =====================================================
app.UseCors("AllowFrontend");

// =====================================================
// Authentication
// =====================================================
app.UseAuthentication();

// =====================================================
// Authorization
// =====================================================
app.UseAuthorization();

// =====================================================
// Controllers
// =====================================================
app.MapControllers();

// =====================================================
// Run
// =====================================================
app.Run();
