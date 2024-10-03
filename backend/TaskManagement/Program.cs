using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TaskManagement.Data;
using TaskManagement.Repositories;
using TaskManagement.Repositories.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


Env.Load();

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
{
    { "ConnectionStrings:SQLConnection", $"Server={Environment.GetEnvironmentVariable("DB_HOST")};User ID={Environment.GetEnvironmentVariable("DB_USER")};Password={Environment.GetEnvironmentVariable("SA_PASSWORD")};TrustServerCertificate=true;MultipleActiveResultSets=true" },
    { "AppSettings:JWT_SECRET", Environment.GetEnvironmentVariable("JWT_SECRET") }
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:JWT_SECRET").Value)
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });


builder.Services.AddSwaggerGen(opts =>
{
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<String>()
        }
    });
});


builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<DbInitializer>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbInitializer = services.GetRequiredService<DbInitializer>();
    dbInitializer.Initialize();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
