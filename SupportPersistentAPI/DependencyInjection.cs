﻿using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SupportPersistentAPI.Consumers;
using System.Text;

namespace SupportPersistentAPI
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

        public static IServiceCollection AddSwaggerGenWithBearer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = """
                              Authorization using JWT by adding header
                              Authorization: Bearer [token]
                              """,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return serviceCollection;
        }

        public static IServiceCollection AddMassTransitRabbitMq(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<ChatMessageConsumer>();
                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.UsingInMemory((context, configure) =>
                {
                    configure.ConfigureEndpoints(context);
                });
            });

            return serviceCollection;
        }

    }
}
