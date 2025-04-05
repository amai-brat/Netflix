using Microsoft.EntityFrameworkCore;
using PaymentService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddDbContext<AppDbContext>(b =>
{
    b.UseNpgsql(builder.Configuration.GetConnectionString("DATABASE_CONNECTION_STRING_PAYMENT"));
});

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<PaymentService.Services.PaymentService>();
app.Run();