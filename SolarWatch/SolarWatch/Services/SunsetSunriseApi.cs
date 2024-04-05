using System.Net;

namespace SolarWatch.Services;

public class SunsetSunriseApi : ISunsetSunriseApi
{
    public ILogger<SunsetSunriseApi> _Logger;

    public SunsetSunriseApi(ILogger<SunsetSunriseApi> logger)
    {
        _Logger = logger;
    }

    public string GetSun(double lat, double lon, string date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";

        using var client = new WebClient();
        
        _Logger.LogInformation("Calling SunsetSunrise API url: {url}", url);
        
        if (client.DownloadString(url) == "")
        {
            _Logger.LogError("Invalid date/date format");
            throw new Exception();
        }

        string downloadString = client.DownloadString(url);
            
        return downloadString;
    }
}