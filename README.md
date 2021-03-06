<h1 align="center">

<img src="https://raw.githubusercontent.com/dolifer/NFixtures/master/icon.png" alt="NFixtures" width="200"/>
<br/>
NFixtures
</h1>

<div align="center">

A set of fixtures to use in integration tests.

[![GitHub license](https://img.shields.io/badge/license-mit-blue.svg)](https://raw.githubusercontent.com/dolifer/NFixtures/master/LICENSE)

</div>

*NFixtures* give a set of pre-built fixtures you can inject into your tests.

## License

- NFixtures is licensed under the [MIT License](https://opensource.org/licenses/MIT)

## Getting started

Add this into your `NuGet.config` to configure it to use [GitHub Packages](https://docs.github.com/en/packages/guides/configuring-dotnet-cli-for-use-with-github-packages)

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="github" value="https://nuget.pkg.github.com/dolifer/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="dolifer" />
      <!-- This is my Personal Access Token, with `read:packages` permission only. -->
      <add key="ClearTextPassword" value="cb1bef6d9d28ee33acf740ee4b89730ef79a2e91" />
    </github>
  </packageSourceCredentials>
</configuration>
```

|PackageName| Description|
| - | - |
|**NFixtures.WebApi** | Contains [StartupFixture<T>](https://github.com/dolifer/NFixtures/blob/master/src/NFixtures.WebApi/StartupFixture.cs) that allows you to easiliy test your WebApi by passing your `Startup` |
|**NFixtures.xUnit** | Gives you a [NamedTestCase](https://github.com/dolifer/NFixtures/blob/master/src/NFixtures.xUnit/NamedTestCase.cs) that adds support of named test cases |
  
## StartupFixture

```csharp
public class ApiFixture : StartupFixture<Startup>
{
    protected override void ConfigureAppConfiguration(IConfigurationBuilder configurationBuilder)
    {
        // configure web host configuration - add env parameters, etc.
    }

    protected override void ConfigureTestServices(IServiceCollection services)
    {
        // add additional services (usually Mocks) to use in your test
    }
}
```

Now you can inject this into your tests, by implementing `IClassFixture<ApiFixture>`

```csharp
public class ControllerTests : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;

    public ControllerTests([NotNull] ApiFixture fixture, [NotNull] ITestOutputHelper output)
    {
        _fixture = fixture;
        _fixture.SetLogger(output); // redirects logger messages into standard xunit test output
    }

    [Fact]
    public async Task Get_Returns_Unauthorized()
    {
        // arrange
        var client = _fixture.CreateDefaultClient();

        // act
        var response = await client.GetAsync("/api/v1/controller").ConfigureAwait(false);

        // assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
```
