using System.Text;
using Application.Exceptions.Base;
using DataAccess;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MobileAPI.Helpers;
using MobileAPI.Options;
using MobileAPI.Types;
using MobileAPI.Types.Auth;
using MobileAPI.Types.Content;
using MobileAPI.Types.ContentType;
using MobileAPI.Types.FavouriteContent;
using MobileAPI.Types.Genre;
using MobileAPI.Types.Section;
using MobileAPI.Types.Subscriptions;

namespace MobileAPI;

public static class DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static IServiceCollection AddMobileGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
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
            .AddType<SubscriptionType>()
            .AddInterfaceType<ContentBase>()
                .AddType<MovieContent>()
                .AddType<SerialContent>()
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
                .AddTypeExtension<ContentQuery>()
                .AddTypeExtension<GenreQuery>()
                .AddTypeExtension<ContentTypeQuery>()
                .AddTypeExtension<FavouriteContentQuery>()
                .AddTypeExtension<SectionQuery>()
            .AddMutationType()
                .AddTypeExtension<AuthMutation>()
                .AddTypeExtension<SubscriptionMutation>()
                .AddTypeExtension<FavouriteContentMutation>();

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
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
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)),
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

    public static IServiceCollection AddServiceApis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<JwtTokenHandler>();
        
        var options = configuration.GetSection("Services").Get<ServicesOptions>() 
                      ?? throw new InvalidOperationException("Services options are missing.");
        
        services.AddHttpClient("SubscriptionService", client =>
        {
            client.BaseAddress = new Uri(options.SubscriptionServiceUrl);
        }).AddHttpMessageHandler<JwtTokenHandler>();

        return services;
    }

}