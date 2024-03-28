namespace SolarWatch.Services;

public interface IGeocodingApi
{
    public string GetLatLon(string data);
}