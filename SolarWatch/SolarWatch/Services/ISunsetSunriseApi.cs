namespace SolarWatch.Services;

public interface ISunsetSunriseApi
{
    public string GetSun(double lat, double lon, string date);
}