using System.Net;
using SolarWatch.Data;

namespace SolarWatch.Services;

public class SunsetSunriseApi : ISunsetSunriseApi
{
    private ILogger<SunsetSunriseApi> _logger;
    private readonly SolarWatchContext _dbContext;

    public SunsetSunriseApi(ILogger<SunsetSunriseApi> logger, SolarWatchContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<string> GetSun(double lat, double lon, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling SunsetSunrise API url: {url}", url);
        
        var response = await client.GetAsync(url);

        var returnV = await response.Content.ReadAsStringAsync();
        
        if (returnV == "")
        {
            _logger.LogError("Invalid date/date format");
            throw new Exception();
        }

        return returnV;
    }

    public void AddSun(Sun sun)
    {
        _dbContext.Add(sun);
        _dbContext.SaveChanges();
    }
    
    public void AddCity(City city)
    {
        _dbContext.Add(city);
        _dbContext.SaveChanges();
    }
    
    public void AddLatLon(LatLon latLon)
    {
        _dbContext.Add(latLon);
        _dbContext.SaveChanges();
    }
}