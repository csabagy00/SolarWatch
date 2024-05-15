using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public Sun ProcessSun(string data, string city, string date)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        try
        {
            string sunrise = results.GetProperty("sunrise").GetString();
            string sunset = results.GetProperty("sunset").GetString();

            Sun? riseSet = CreateSun(sunrise, sunset, date, city);

            if (riseSet == null)
                throw new Exception();
                    
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

    public Sun? CreateSun(string sunrise, string sunset, string date, string city)
    {
        if (sunrise.IsNullOrEmpty() || sunset.IsNullOrEmpty())
            return null;
        
        Sun sun = new Sun { Sunrise = sunrise, Sunset = sunset, Date = date, City = city};

        return sun;
    }

    public City? CreateCity(string name, string country, string? state)
    {
        if (name.IsNullOrEmpty() || country.IsNullOrEmpty())
            return null;
        
        City city = new City { Name = name, Country = country, State = state};
        
        return city;
    }
}