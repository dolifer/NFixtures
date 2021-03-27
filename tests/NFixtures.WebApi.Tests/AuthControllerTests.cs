using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NFixtures.WebApi.Extensions;
using NFixtures.WebApi.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace NFixtures.WebApi.Tests
{
    public class AuthControllerTests : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public AuthControllerTests(ApiFixture fixture, ITestOutputHelper testOutputHelper)
        {
            fixture.SetLogger(testOutputHelper);
            _fixture = fixture;
        }

        [Fact]
        public async Task Get_Returns_Unauthorized()
        {
            // arrange
            var client = _fixture.CreateDefaultClient();

            // act
            var response = await client.GetAsync("/api/values");

            // assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Returns_Ok()
        {
            // arrange
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, "123"),
                    new(ClaimTypes.Email, "email@server.com")
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            };

            var client = _fixture.CreateDefaultClient().WithJwtBearer(tokenDescriptor);

            // act
            var response = await client.GetAsync("/api/values");

            // assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
