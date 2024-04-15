using System.Net;

namespace SolarWatch.Services;

public class SunsetSunriseApi : ISunsetSunriseApi
{
    public ILogger<SunsetSunriseApi> _Logger;

    public SunsetSunriseApi(ILogger<SunsetSunriseApi> logger)
    {
        _Logger = logger;
    }

    public async Task<string> GetSun(double lat, double lon, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";

        using var client = new HttpClient();
        
        _Logger.LogInformation("Calling SunsetSunrise API url: {url}", url);
        
        var response = await client.GetAsync(url);

        var returnV = await response.Content.ReadAsStringAsync();
        
        if (returnV == "")
        {
            _Logger.LogError("Invalid date/date format");
            throw new Exception();
        }

        return returnV;
    }
}