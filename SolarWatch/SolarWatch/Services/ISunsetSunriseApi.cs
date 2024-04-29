namespace SolarWatch.Services;

public interface ISunsetSunriseApi
{
    public Task<string> GetSun(double lat, double lon, string date);

    public void AddSun(Sun sun);
    
    public void AddLatLon(LatLon latLon);
    
    public void AddCity(City city);
}