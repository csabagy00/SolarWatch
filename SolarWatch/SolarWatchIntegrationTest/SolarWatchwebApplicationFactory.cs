using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch;
using SolarWatch.Contracts;
using SolarWatch.Data;
using Xunit;

namespace SolarWatchIntegrationTest;

public class SolarWatchWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var solarWatchDbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<SolarWatchContext>));

            services.Remove(solarWatchDbContextDescriptor);

            services.AddDbContext<SolarWatchContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            using var scope = services.BuildServiceProvider().CreateScope();

            var solarContext = scope.ServiceProvider.GetRequiredService<SolarWatchContext>();
            solarContext.Database.EnsureDeleted();
            solarContext.Database.EnsureCreated();
        });
    }
}