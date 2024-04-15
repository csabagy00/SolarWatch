namespace SolarWatch.Services;

public interface IGeocodingApi
{
    public Task<string> GetLatLon(string data);
}