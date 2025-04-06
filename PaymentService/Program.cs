using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Data.Abstractions;
using PaymentService.Data.Impls;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddScoped<IAccountProvider, FakeAccountProvider>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(b =>
{
    b.UseNpgsql(builder.Configuration.GetConnectionString("DATABASE_CONNECTION_STRING_PAYMENT"));
});

builder.Services.AddGrpc();

var app = builder.Build();

await Migrator.MigrateAsync(app.Services);
app.MapGrpcService<PaymentService.Services.PaymentService>();
app.Run();