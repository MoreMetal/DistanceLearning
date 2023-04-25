using DistanceLearning.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DistanceLearning;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("LiteDBConnection");

        builder.Services.AddDbContext<AppDbContext>(options => 
            options.UseSqlite(connectionString)
        );

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.Cookie.Name = "YourAppCookieName";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            options.LoginPath = "/Identity/Account/Login";
            // ReturnUrlParameter requires 
            //using Microsoft.AspNetCore.Authentication.Cookies;
            options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            options.SlidingExpiration = true;
        });

        // Add services to the container.
        builder.Services.AddMvc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.MapControllerRoute(
            name: "admin",
            pattern: "{area:exists}/{controller=TaskTypes}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        using (var scope = app.Services.CreateScope())
        {
            var scopedProvider = scope.ServiceProvider;
            try
            {
                var userManager = scopedProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var identityContext = scopedProvider.GetRequiredService<AppDbContext>();
                await IdentitySeed.SeedAsync(identityContext, userManager, roleManager);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Произошла ошибка при заполнении базы данных.");
            }
        }

        app.Run();
    }
}
