using System.Net.Http.Json;
using SolarWatch;
using Xunit;

namespace SolarWatchIntegrationTest;

[Collection("IntegrationTests")]
public class SolarControllerIntegrationTest
{
    private readonly SolarWatchwebApplicationFactory _app;
    private readonly HttpClient _client;

    public SolarControllerIntegrationTest(SolarWatchwebApplicationFactory app, HttpClient client)
    {
        _app = app;
        _client = client;
    }

    [Fact]
    public async Task TestEndPoint()
    {
        var response = await _client.GetAsync("http://localhost:5292/Sun/Get?city=budapest&date=2024-05-14");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<Sun>();

        Sun sun = new Sun
        {
            Id = 16,
            Sunrise = "3:21:09 AM",
            Sunset = "6:00:20 PM",
            Date = "2024-05-14",
            City = "Budapest"
        };
        
        Assert.Equal(sun, data);
    }

}