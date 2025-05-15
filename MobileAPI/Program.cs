using Application;
using DataAccess.Extensions;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.HttpOverrides;
using MobileAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddContentApiServices();
builder.Services.AddHttpClient();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddMobileGraphQL();

var app = builder.Build();

await Migrator.MigrateAsync(app.Services);

app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
app.Run();