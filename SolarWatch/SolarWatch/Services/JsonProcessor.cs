using System.Text.Json;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public Sun ProcessSun(string data, string city, string date)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        try
        {
            Sun riseSet = new Sun
            {
                Sunrise = results.GetProperty("sunrise").GetString(),
                Sunset = results.GetProperty("sunset").GetString(),
                Date = date,
                City = city
            };

            return riseSet;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting data: {e.Message}");
            throw;
        }
        
    }

    public LatLon ProcessLatLon(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement element = json.RootElement.EnumerateArray().First();

        LatLon latLon = new LatLon()
        {
            Lat = (double)element.GetProperty("lat").GetDecimal(),
            Lon = (double)element.GetProperty("lon").GetDecimal()
        };

        return latLon;
    }

    public City ProcessCity(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement element = json.RootElement.EnumerateArray().First();
        
        City city = new City
        {
            Name = element.GetProperty("name").ToString(),
            Country = element.GetProperty("country").ToString(),
            State = element.TryGetProperty("state", out element) ? element.GetProperty("state").ToString() : null
        };

        return city;
    }
}