using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NFixtures.Primitives;
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
            var identity = new TestIdentityBuilder(_fixture.FirstUser).Build();
            var client = _fixture.CreateDefaultClient().WithJwtBearer(identity);

            // act
            var response = await client.GetAsync("/api/values");

            // assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
