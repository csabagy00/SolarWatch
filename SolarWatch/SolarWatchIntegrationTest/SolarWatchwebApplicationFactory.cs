using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SolarWatch.Data;

namespace SolarWatchIntegrationTest;

public class SolarWatchwebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    /*public HttpClient Client { get; }
    
    public SolarWatchwebApplicationFactory(HttpClient client)
    {
        Client = client;
    }*/

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContectDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));
            var usersDbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<UsersContext>));

            services.Remove(solarWatchDbContectDescriptor);
            services.Remove(usersDbContextDescriptor);

            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            services.AddDbContext<UsersContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            using var scope = services.BuildServiceProvider().CreateScope();

            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();

            var userContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
            userContext.Database.EnsureDeleted();
            userContext.Database.EnsureCreated();
            
            
        });
    }
}