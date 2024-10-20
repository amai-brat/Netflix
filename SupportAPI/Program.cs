using SupportAPI;
using SupportAPI.Hubs;
using SupportAPI.Services;
using SupportAPI.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy(builder.Configuration);
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddScoped<IHistoryService, HistoryService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthorization();
app.UseAuthentication();

app.MapHub<SupportHub>("/hub/support");
app.MapControllers();

app.Run();
