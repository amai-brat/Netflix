using API.Middlewares.ExceptionHandler;
using DataAccess.Extensions;
using Domain.Services;
using Infrastucture;
using Infrastucture.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastucture();
builder.Services.AddControllers();
builder.Services.AddContentAPIServices();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandlerMiddleware();

app.MapControllers();

app.Run();
