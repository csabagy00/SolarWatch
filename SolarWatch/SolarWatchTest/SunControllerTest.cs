using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Data;
using SolarWatch.Services;

namespace SolarWatchTest;

[TestFixture]
public class SunControllerTest
{
    private Mock<ILogger<SunController>> _loggerMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private Mock<ISunsetSunriseApi> _sunsetSunriseApiMock;
    private Mock<IGeocodingApi> _geocodingMock;
    private Mock<SolarWatchContext> _dbContext;
    private SunController _controller;
    private IJsonProcessor _jsonProcessor;

    [SetUp]
    public void Setup()
    {
        DbContextOptions<SolarWatchContext> options = new DbContextOptions<SolarWatchContext>();
        
        _loggerMock = new Mock<ILogger<SunController>>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _sunsetSunriseApiMock = new Mock<ISunsetSunriseApi>();
        _geocodingMock = new Mock<IGeocodingApi>();
        _dbContext = new Mock<SolarWatchContext>(options);
        _controller = new SunController(_loggerMock.Object, _jsonProcessorMock.Object, _geocodingMock.Object, _sunsetSunriseApiMock.Object, _dbContext.Object);
        _jsonProcessor = new JsonProcessor();
    }

    [Test]
    public async Task SuccessfulSunCreate()
    {
        Sun actual = _jsonProcessor.CreateSun("06:00:00 AM", "18:00:00 PM", "2024-05-06", "szeged");

        Sun expected = new Sun { Sunrise = "06:00:00 AM", Sunset = "18:00:00 PM", Date = "2024-05-06", City = "szeged" };

        Assert.That(actual.Sunrise, Is.EqualTo(expected.Sunrise));
        Assert.That(actual.Sunset, Is.EqualTo(expected.Sunset));
        Assert.That(actual.Date, Is.EqualTo(expected.Date));
        Assert.That(actual.City, Is.EqualTo(expected.City));
        Assert.That(actual, Is.InstanceOf<Sun>());
    }

    [Test]
    public async Task SunCreateReturnsNullIfSunriseIsEmpty()
    {
        Sun actual = _jsonProcessor.CreateSun("", "18:00:00 PM", "2024-05-06", "szeged");
        
        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task SunCreateReturnsNullIfSunriseIsNull()
    {
        Sun actual = _jsonProcessor.CreateSun(null, "18:00:00 PM", "2024-05-06", "szeged");
        
        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task SunCreateReturnsNullIfSunsetIsEmpty()
    {
        Sun actual = _jsonProcessor.CreateSun("06:00:00 AM", "", "2024-05-06", "szeged");
        
        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task SunCreateReturnsNullIfSunsetIsNull()
    {
        Sun actual = _jsonProcessor.CreateSun("06:00:00 AM", null, "2024-05-06", "szeged");
        
        Assert.That(actual, Is.EqualTo(null));
    }

    [Test]
    public async Task CityCreateSuccessfulWithStateNull()
    {
        City actual = _jsonProcessor.CreateCity("szeged", "HU", null);

        City expected = new City { Name = "szeged", Country = "HU"};

        Assert.That(actual, Is.InstanceOf<City>());
        Assert.That(actual.Name, Is.EqualTo(expected.Name));
        Assert.That(actual.Country, Is.EqualTo(expected.Country));
        Assert.That(actual.State, Is.EqualTo(null));
    }
    
    [Test]
    public async Task CityCreateSuccessfulWithStateNotNull()
    {
        City actual = _jsonProcessor.CreateCity("los angeles", "USA", "CA");

        City expected = new City { Name = "los angeles", Country = "USA", State = "CA"};

        Assert.That(actual, Is.InstanceOf<City>());
        Assert.That(actual.Name, Is.EqualTo(expected.Name));
        Assert.That(actual.Country, Is.EqualTo(expected.Country));
        Assert.That(actual.State, Is.EqualTo(expected.State));
    }

    [Test]
    public async Task CityCreateReturnsNullIfNameIsEmpty()
    {
        City actual = _jsonProcessor.CreateCity("", "USA", "CA");

        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task CityCreateReturnsNullIfNameIsNull()
    {
        City actual = _jsonProcessor.CreateCity(null, "USA", "CA");

        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task CityCreateReturnsNullIfCountryIsEmpty()
    {
        City actual = _jsonProcessor.CreateCity("los angeles", "", "CA");

        Assert.That(actual, Is.EqualTo(null));
    }
    
    [Test]
    public async Task CityCreateReturnsNullIfCountryIsNull()
    {
        City actual = _jsonProcessor.CreateCity("los angeles", null, "CA");

        Assert.That(actual, Is.EqualTo(null));
    }
}