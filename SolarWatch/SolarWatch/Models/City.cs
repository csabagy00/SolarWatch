namespace SolarWatch;

public class City
{
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public string State { get; init; }
    
    public string Country { get; init; }
    
    public LatLon LatLon { get; init; }

    public SunriseSunset SunriseSunset { get; init; } = new SunriseSunset();

    public void SetSunriseSunset(string sunrise, string sunset)
    {
        SunriseSunset.Sunrise = sunrise;
        SunriseSunset.Sunset = sunset;
    }

    public void SetLatLon(double lat, double lon)
    {
        LatLon.Lat = lat;
        LatLon.Lon = lon;
    } 

    public string GetSunrise()
    {
        return SunriseSunset.Sunrise;
    }
    
    public string GetSunset()
    {
        return SunriseSunset.Sunset;
    }
}