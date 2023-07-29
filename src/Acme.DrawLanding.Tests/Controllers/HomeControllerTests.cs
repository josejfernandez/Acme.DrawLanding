using Acme.DrawLanding.Tests.Mocking;

namespace Acme.DrawLanding.Tests.Controllers;

public sealed class HomeControllerTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _integrationTestFixture;

    public HomeControllerTests(IntegrationTestFixture integrationTestFixture)
    {
        _integrationTestFixture = integrationTestFixture ?? throw new ArgumentNullException(nameof(integrationTestFixture));
    }

    [Fact]
    public async Task Get_Index__returns_http_success()
    {
        // Arrange
        var client = _integrationTestFixture.AppClient;

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
