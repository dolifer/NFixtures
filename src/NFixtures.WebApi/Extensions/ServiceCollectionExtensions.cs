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
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureTestAuthentication(this IServiceCollection services) 
            => services.ConfigureTestAuthentication(JwtBearerDefaults.AuthenticationScheme);

        /// <summary>
        /// Configures <see cref="JwtBearerOptions"/> to allow any JWT token,
        /// but success only for a given collection of <see cref="TestUser"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="schema">The name of the authentication schema.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureTestAuthentication(this IServiceCollection services, string schema)
        {
            services
                .PostConfigure<JwtBearerOptions>(schema, o =>
                {
                    o.TokenValidationParameters = new()
                    {
                        IssuerSigningKeyResolver = null,
                        SignatureValidator = (token, _) => new JwtSecurityToken(token),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireSignedTokens = false,
                        ValidateIssuerSigningKey = false
                    };
                });

            return services;
        }
    }
}
