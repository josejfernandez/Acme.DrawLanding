using System.Data.Common;
using Acme.DrawLanding.Library.Common.Encryption;
using Acme.DrawLanding.Library.Data;
using Acme.DrawLanding.Library.Domain.Submissions;
using Acme.DrawLanding.Library.Domain.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.DrawLanding;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static IServiceCollection AddPersistenceWithInMemoryDb(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<DbConnection>(container =>
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return connection;
        });

        services.AddDbContext<AppDbContext>((container, options) =>
        {
            var connection = container.GetRequiredService<DbConnection>();
            options.UseSqlite(connection);
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IEncryptionService, AesEncryptionService>();
        services.AddScoped<IUserCredentialsService, UserCredentialsService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
