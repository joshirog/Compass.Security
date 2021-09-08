using System;
using Compass.Security.Domain.Entities;
using Compass.Security.Infrastructure.Persistences.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Commons.Extensions
{
    public static class OpenIddictExtension
    {
        public static void AddOpenIddictExtension(this IServiceCollection services)
        {
            services
                .AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>()
                        .ReplaceDefaultEntities<Domain.Entities.Application, ApplicationAuthorization, ApplicationScope, ApplicationToken, Guid>();
                })
                .AddServer(options =>
                {
                    options
                        .AllowAuthorizationCodeFlow()
                        .RequireProofKeyForCodeExchange()
                        .AllowClientCredentialsFlow()
                        .AllowRefreshTokenFlow();

                    options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo");

                    options
                        .AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey()
                        .DisableAccessTokenEncryption();

                    options.RegisterScopes("api");

                    options
                        .UseAspNetCore()
                        .EnableTokenEndpointPassthrough()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough();
                })

                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}