using System.Text;
using API.Hubs;
using API.Middlewares.ExceptionHandler;
using Application.Options;
using DataAccess.Extensions;
using Infrastructure;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Application;
using Infrastructure.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
builder.Services.AddContentApiServices();
builder.Services.AddAutoMapper(typeof(ContentProfile));
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidators();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Key"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Description = """
                      Authorization using JWT by adding header
                      Authorization: Bearer [token]
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseExceptionHandlerMiddleware();

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/hub/notification");
app.MapControllers();

app.Run();

public partial class Program;