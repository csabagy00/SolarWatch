using System.Net;

namespace SolarWatch.Services;

public class Geocoding : IGeocodingApi
{
    public ILogger<Geocoding> _logger;

    public Geocoding(ILogger<Geocoding> logger)
    {
        _logger = logger;
    }

    public string GetLatLon(string city)
    {
        var apiKey = "3b56c738243bfacb244a70d9c4eb7998";
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid={apiKey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling Geocoding API url: {url}", url);

        if (client.DownloadString(url) == "[]")
        {
            _logger.LogError("Invalid city");
            throw new Exception();
        }

        return client.DownloadString(url);
    }
}