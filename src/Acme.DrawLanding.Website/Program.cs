using Microsoft.AspNetCore.Authentication;

namespace Acme.DrawLanding.Website;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = GetAndValidateConnectionString(builder);

        builder.Services.AddAuthentication().AddCookie(options =>
        {
            options.AccessDeniedPath = "/account/denied";
            options.LoginPath = "/account/login";
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

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
            pattern: "{area:exists}/{controller=Submissions}/{action=Index}/{id?}");

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
}