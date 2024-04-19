namespace SolarWatch.Services;

public interface IJsonProcessor
{
    public Sun ProcessSun(string data, string city, string date);

    public LatLon ProcessLatLon(string data);

    public City ProcessCity(string data);
}