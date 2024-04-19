namespace SolarWatch;

public class City
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public string? State { get; init; }
    
    public string Country { get; init; }

    public LatLon LatLon { get; init; } = new LatLon();

    public Sun Sun { get; init; } = new Sun();

    public void SetSunriseSunset(string sunrise, string sunset)
    {
        Sun.Sunrise = sunrise;
        Sun.Sunset = sunset;
    }

    public void SetLatLon(double lat, double lon)
    {
        LatLon.Lat = lat;
        LatLon.Lon = lon;
    } 

    public string GetSunrise()
    {
        return Sun.Sunrise;
    }
    
    public string GetSunset()
    {
        return Sun.Sunset;
    }
}