using SupportAPI;
using SupportAPI.Hubs;
using SupportAPI.Services;
using SupportAPI.Data.Extensions;
using SupportAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddMassTransitInMemory();
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

app.UseHsts();

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthorization();
app.UseAuthentication();

app.MapHub<SupportHub>("/hub/support");
app.MapControllers();

app.Run();
