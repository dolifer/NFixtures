using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace NFixtures.WebApi.Helpers
{
    /// <summary>
    /// Helper class to build JWT tokens.
    /// </summary>
    public static class JwtTokenHelper
    {
        private static readonly JwtSecurityTokenHandler _securityTokenHandler = new();

        /// <summary>
        /// Creates a JWT token string.
        /// </summary>
        /// <param name="securityToken">The token.</param>
        /// <returns>JWT token string.</returns>
        /// <exception cref="ArgumentNullException">securityToken is null.</exception>
        public static string GetToken(SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            return _securityTokenHandler.WriteToken(securityToken);
        }

        /// <summary>
        /// Creates a JWT token string.
        /// </summary>
        /// <param name="securityTokenDescriptor">The token descriptor.</param>
        /// <returns>JWT token string.</returns>
        /// <exception cref="ArgumentNullException">securityTokenDescriptor is null.</exception>
        public static string GetToken(SecurityTokenDescriptor securityTokenDescriptor)
        {
            if (securityTokenDescriptor == null)
            {
                throw new ArgumentNullException(nameof(securityTokenDescriptor));
            }

            return GetToken(_securityTokenHandler.CreateToken(securityTokenDescriptor));
        }

        /// <summary>
        /// Creates a JWT token string.
        /// </summary>
        /// <param name="identity">The test identity.</param>
        /// <returns>JWT token string.</returns>
        /// <exception cref="ArgumentNullException">securityTokenDescriptor is null.</exception>
        public static string GetToken(ClaimsIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Audience = "NFixture.Audience",
                Issuer = "NFixture.Issuer",
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            };

            return GetToken(_securityTokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
