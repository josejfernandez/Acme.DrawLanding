using Acme.DrawLanding.Library.Common.Encryption;

namespace Acme.DrawLanding.Website;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = GetAndValidateConnectionString(builder);
        var encryptionKey = GetEncryptionKey(builder);

        builder.Services.AddSingleton(encryptionKey);

        builder.Services.AddAuthentication().AddCookie(options =>
        {
            options.AccessDeniedPath = "/account/denied";
            options.LoginPath = "/account/login";
            options.LogoutPath = "/account/logout";
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddAntiforgery(options =>
        {
            options.FormFieldName = Constants.CsrfFieldName;
            options.HeaderName = Constants.CsrfHeaderName;
        });

        builder.Services.AddDomainServices();

        builder.Services.AddPersistence(connectionString);
        builder.Services.AddRepositories();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "Admin",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    private static string GetAndValidateConnectionString(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Application needs a connection string.");
        }

        return connectionString;
    }

    private static EncryptionKey GetEncryptionKey(WebApplicationBuilder builder)
    {
        var key = builder.Configuration.GetRequiredSection("EncryptionKey").Value;

        if (key == null)
        {
            throw new InvalidOperationException("Application needs an encryption key.");
        }

        return new EncryptionKey(Convert.FromBase64String(key));
    }
}