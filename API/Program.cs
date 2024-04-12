using API.Hubs;
using API.Middlewares.ExceptionHandler;
using DataAccess.Extensions;
using Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Frontend",
        policy  =>
        {
            policy.WithOrigins("http://localhost:5173/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddSignalR();
builder.Services.AddExceptionHandlerMiddleware();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services
    .AddCommentService()
    .AddReviewService()
    .AddCommentService()
    .AddFavouriteService()
    .AddNotificationService();


var app = builder.Build();

app.UseExceptionHandlerMiddleware();

app.UseCors("Frontend");
app.MapHub<NotificationHub>("/hub/notification");
app.MapControllers();

app.Run();
