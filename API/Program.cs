using API.Middlewares.ExceptionHandler;
using DataAccess.Extensions;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddContentAPIServices();


var app = builder.Build();

app.UseExceptionHandlerMiddleware();
app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();

app.Run();
