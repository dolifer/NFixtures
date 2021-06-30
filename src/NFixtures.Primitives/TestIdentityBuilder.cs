using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace NFixtures.Primitives
{
    /// <summary>
    /// Builder for <see cref="ClaimsIdentity"/> from a given <see cref="TestUser"/>.
    /// </summary>
    public class TestIdentityBuilder
    {
        private readonly TestUser _user;
        private readonly List<Claim> _additionalClaims;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestIdentityBuilder"/> class.
        /// </summary>
        /// <param name="user">The user to build identity for.</param>
        public TestIdentityBuilder(TestUser user)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _additionalClaims = new List<Claim>();
        }

        /// <summary>
        /// Extends the current <see cref="TestUser"/> with a given claims.
        /// </summary>
        /// <param name="claims">The claims to add for current user.</param>
        /// <returns>The instance of <see cref="TestIdentityBuilder"/>.</returns>
        public TestIdentityBuilder WithClaims(params Claim[] claims)
        {
            _additionalClaims.AddRange(claims);
            return this;
        }

        /// <summary>
        /// Creates a new <see cref="ClaimsIdentity"/> with a set of default user claims
        /// and additional ones added via <see cref="WithClaims"/>.
        /// </summary>
        /// <returns>The <see cref="ClaimsIdentity"/> instance.</returns>
        public ClaimsIdentity Build()
        {
            var claims = new List<Claim>
            {
                new(TestUser.IdClaim, _user.Id.ToString()),
                new(ClaimTypes.NameIdentifier, _user.Name),
                new(ClaimTypes.Email, _user.Email)
            };

            claims.AddRange(_additionalClaims);

            return new ClaimsIdentity(claims);
        }
    }
}
