using System.Text;
using Application.Exceptions.Base;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobileAPI.Types;
using MobileAPI.Types.Auth;
using MobileAPI.Types.Reviews;
using MobileAPI.Types.Subscriptions;
using MobileAPI.Types.User;

namespace MobileAPI;

public static class DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static IServiceCollection AddMobileGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .ModifyCostOptions(options =>
            {
                options.MaxFieldCost = 10_000;
            })
            .AddAuthorization()
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            .RegisterDbContextFactory<AppDbContext>()
            .AddMutationConventions(new MutationConventionOptions
            {
                PayloadErrorsFieldName = "errors",
                ApplyToAllMutations = true
            })
            .AddErrorFilter(error =>
            {
                return error.Exception switch
                {
                    ArgumentValidationException ex => error.WithCode("400").WithMessage(ex.Message),
                    NotPermittedException ex => error.WithCode("403").WithMessage(ex.Message),
                    ValueChangedException ex => error.WithCode("409").WithMessage(ex.Message),
                    BusinessException => error.WithCode("500").WithMessage("Internal Server Error"),
                    _ => error
                };
            })
            .AddQueryType<Query>()
                .AddTypeExtension<SubscriptionQuery>()
                .AddTypeExtension<PersonalInfoQuery>()
                .AddTypeExtension<ReviewsQuery>()
            .AddMutationType()
                .AddTypeExtension<AuthMutation>()
                .AddTypeExtension<PersonalInfoMutation>();

        return services;
    }
    
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
                            path.StartsWithSegments("/hub/notification") ||
                            path.StartsWithSegments("/content/movie") ||
                            path.StartsWithSegments("/content/serial"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return serviceCollection;
    }
}