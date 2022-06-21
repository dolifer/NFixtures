using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using NFixtures.Primitives;

namespace NFixtures.WebApi.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection"/> extensions to configure test authentication.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures <see cref="JwtBearerOptions"/> to allow any JWT token,
        /// but success only for a given collection of <see cref="TestUser"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="users">The collection of <see cref="TestUser"/> to use.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureTestAuthentication(this IServiceCollection services, params TestUser[] users) 
            => services.ConfigureTestAuthentication(JwtBearerDefaults.AuthenticationScheme, users);

        /// <summary>
        /// Configures <see cref="JwtBearerOptions"/> to allow any JWT token,
        /// but success only for a given collection of <see cref="TestUser"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="schema">The name of the authentication schema.</param>
        /// <param name="users">The collection of <see cref="TestUser"/> to use.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureTestAuthentication(this IServiceCollection services, string schema, params TestUser[] users)
        {
            services
                .PostConfigure<JwtBearerOptions>(schema, o =>
                {
                    o.TokenValidationParameters.SignatureValidator = (token, _) => new JwtSecurityToken(token);
                    o.TokenValidationParameters.ValidateAudience = false;
                    o.TokenValidationParameters.ValidateIssuer = false;

                    o.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var token = context.SecurityToken as JwtSecurityToken;
                            var claim = token?.Claims.FirstOrDefault(c => c.Type == TestUser.IdClaim);

                            if (claim is null || !Guid.TryParse(claim.Value, out var id))
                            {
                                context.Fail(FormatStrings.Authorization_ClaimNotFound);
                                return Task.CompletedTask;
                            }

                            if (users.Any(x => x.Id == id))
                            {
                                context.Success();
                                return Task.CompletedTask;
                            }

                            context.Fail(string.Format(FormatStrings.Authorization_UserNotFound, id));

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
