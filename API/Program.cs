using API.Hubs;
using API.Middlewares.ExceptionHandler;
using Application.Dto;
using Application.Mappers;
using Application.Services.RegisterExtensions;
using Application.Validators;
using DataAccess.Extensions;
using FluentValidation;
using Infrastructure;
using Infrastructure.Options;
using Infrastucture;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddSignalR();
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastucture();
builder.Services.AddControllers();
builder.Services.AddContentApiServices();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ContentProfile));
builder.Services.AddScoped<IValidator<MovieContentAdminPageDto>, MovieContentDtoAdminPageValidator>();
builder.Services.AddScoped<IValidator<SerialContentAdminPageDto>, SerialContentDtoAdminPageValidator>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

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

app.UseCors("Frontend");
app.MapHub<NotificationHub>("/hub/notification");
app.MapControllers();

app.Run();
