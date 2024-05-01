using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> _roleManager;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
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

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager, IConfigurationRoot config)
    {
        await roleManager.CreateAsync(new IdentityRole(config["Roles:Admin"]));
    }

    private async Task CreateUserRole(RoleManager<IdentityRole> roleManager, IConfigurationRoot config)
    {
        await roleManager.CreateAsync(new IdentityRole(config["Roles:User"]));
    }
}