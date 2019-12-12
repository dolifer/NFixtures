using System.IdentityModel.Tokens.Jwt;

namespace NFixtures.WebApi.Helpers
{
    public static class JwtTokenHelper
    {
        public static readonly JwtSecurityTokenHandler SecurityTokenHandler = new JwtSecurityTokenHandler();
    }
}