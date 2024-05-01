using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> _roleManager;
    private UserManager<IdentityUser> _userManager;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public void AddRoles()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var tAdmin = CreateAdminRole(_roleManager, config);
        tAdmin.Wait();

        var tUser = CreateUserRole(_roleManager, config);
        tUser.Wait();
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExsits();
        tAdmin.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager, IConfigurationRoot config)
    {
        await roleManager.CreateAsync(new IdentityRole(config["Roles:Admin"]));
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager, IConfigurationRoot config)
    {
        await roleManager.CreateAsync(new IdentityRole(config["Roles:User"]));
    }

    private async Task CreateAdminIfNotExsits()
    {
        var adminInDb = await _userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb == null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
            var adminCreated = await _userManager.CreateAsync(admin, "admin123");

            if (adminCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}