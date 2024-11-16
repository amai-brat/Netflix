using Microsoft.EntityFrameworkCore;
using SupportPersistentAPI;
using SupportPersistentAPI.Data;
using SupportPersistentAPI.Data.Extensions;
using SupportPersistentAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMassTransitRabbitMq();
builder.Services.AddTransient<IHistoryService, HistoryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using (var scope = app.Services.CreateAsyncScope())
{
    await Task.Delay(1000);

    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
    if ((await dbContext!.Database.GetPendingMigrationsAsync()).Any())
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
