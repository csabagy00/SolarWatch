namespace SolarWatch;

public class City
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string? State { get; set; }
    
    public string Country { get; set; }

    public LatLon LatLon { get; set; }

    public Sun Sun { get; set; } 

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