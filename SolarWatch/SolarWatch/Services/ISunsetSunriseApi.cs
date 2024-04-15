namespace SolarWatch.Services;

public interface ISunsetSunriseApi
{
    public Task<string> GetSun(double lat, double lon, string date);
}