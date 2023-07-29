using System.Data.Common;
using Acme.DrawLanding.Library.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace Acme.DrawLanding.Tests.Mocking;

public class CustomApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.Single(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(dbContextOptionsDescriptor);

            var dbContextDescriptor = services.Single(x => x.ServiceType == typeof(AppDbContext));
            services.Remove(dbContextDescriptor);

            services.AddPersistenceWithInMemoryDb();
            services.AddRepositories();
        });

        builder.UseEnvironment("Development");
    }
}
