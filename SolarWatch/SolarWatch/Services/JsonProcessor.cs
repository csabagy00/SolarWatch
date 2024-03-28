using System.Text.Json;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public Sun Process(string data, string city, string date)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        Sun riseSet = new Sun
        {
            Sunrise = DateTime.Parse(results.GetProperty("sunrise").GetString()),
            Sunset = DateTime.Parse(results.GetProperty("sunset").GetString()),
            Date = date,
            City = city
        };

        return riseSet;

    }
}