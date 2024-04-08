using API.Middlewares.ExceptionHandler;
using Application.Services.RegisterExtensions;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddContentApiServices();


var app = builder.Build();

app.UseExceptionHandlerMiddleware();

app.MapControllers();

app.Run();
