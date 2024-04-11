using API.Middlewares.ExceptionHandler;
using Application.Dto;
using Application.Mappers;
using Application.Services.RegisterExtensions;
using Application.Validators;
using DataAccess.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddContentApiServices();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ContentProfile));
builder.Services.AddScoped<IValidator<MovieContentAdminPageDto>, MovieContentDtoAdminPageValidator>();
builder.Services.AddScoped<IValidator<SerialContentAdminPageDto>, SerialContentDtoAdminPageValidator>();
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

app.MapControllers();

app.Run();
