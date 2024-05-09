using System.Text;
using API;
using API.Hubs;
using API.MetadataProviders;
using API.Middlewares.ExceptionHandler;
using DataAccess.Extensions;
using Infrastructure;
using Infrastructure.Options;
using Application;
using Application.Options;
using Infrastructure.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection("Minio"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddControllers().AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new CustomMetadataProvider ()));
builder.Services.AddContentApiServices();
builder.Services.AddAutoMapper(typeof(ContentProfile));
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidators();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddCorsWithFrontendPolicy();

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