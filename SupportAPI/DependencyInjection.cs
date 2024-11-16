using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SupportAPI.Configuration;
using System.Text;

namespace SupportAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hub/support"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return serviceCollection;
        }

        public static IServiceCollection AddCorsWithFrontendPolicy(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(name: "Frontend",
                    policy =>
                    {
                        policy.WithOrigins(configuration["FrontendConfig:Url"]!)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            return serviceCollection;
        }

        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection serviceCollection,
            RabbitMqConfig rabbitConfiguration)
        {
            serviceCollection.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(rabbitConfiguration.Hostname!), h =>
                    {
                        h.Username(rabbitConfiguration.Username);
                        h.Password(rabbitConfiguration.Password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return serviceCollection;
        }

    }
}
