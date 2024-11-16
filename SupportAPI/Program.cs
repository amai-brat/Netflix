using SupportAPI;
using SupportAPI.Configuration;
using SupportAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddMassTransitRabbitMq(
    builder.Configuration.GetSection("RabbitMqConfig").Get<RabbitMqConfig>()!
);

var app = builder.Build();


app.UseRouting();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<SupportHub>("/hub/support");
app.MapControllers();

app.Run();
