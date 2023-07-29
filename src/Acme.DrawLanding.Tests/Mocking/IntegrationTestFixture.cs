using Acme.DrawLanding.Library.Data;
using Acme.DrawLanding.Website;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.DrawLanding.Tests.Mocking;

public class IntegrationTestFixture : IDisposable
{
    public CustomApplicationFactory<Program> AppFactory { get; }

    public HttpClient AppClient => AppFactory.CreateClient();

    public IntegrationTestFixture()
    {
        AppFactory = new CustomApplicationFactory<Program>();

        using var scope = AppFactory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _ = context.Database.EnsureDeleted();
        _ = context.Database.EnsureCreated();
    }

    public async Task SeedDatabase(Func<AppDbContext, Task> seedFunction)
    {
        ArgumentNullException.ThrowIfNull(seedFunction);

        using var scope = AppFactory.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await seedFunction(context);
    }

    public void Dispose()
    {
        AppFactory.Dispose();

        GC.SuppressFinalize(this);
    }
}
