namespace SolarWatch.Services;

public interface IJsonProcessor
{
    public Sun Process(string data, string city, string date);
}