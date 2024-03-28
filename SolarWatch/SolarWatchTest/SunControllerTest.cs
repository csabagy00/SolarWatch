using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services;

namespace SolarWatchTest;

[TestFixture]
public class SunControllerTest
{
    private Mock<ILogger<SunController>> _loggerMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private Mock<ISunsetSunriseApi> _sunsetSunriseApiMock;
    private Mock<IGeocodingApi> _geocodingMock;
    private SunController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SunController>>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _sunsetSunriseApiMock = new Mock<ISunsetSunriseApi>();
        _geocodingMock = new Mock<IGeocodingApi>();
        _controller = new SunController(_loggerMock.Object, _jsonProcessorMock.Object, _geocodingMock.Object, _sunsetSunriseApiMock.Object);
    }

    [Test]
    public void SunGetReturnsNotFoundResultIfSunsetSunriseFails()
    {
        //Arrange
        _sunsetSunriseApiMock.Setup(x => x.GetSun(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()))
            .Throws(new Exception());
        
        //Act
        var result = _controller.SunGet("szeged", "2024-03-31");
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }


    [Test]
    public void SunGetReturnsNotFoundResultIfGeoCodingFails()
    {
        //Arrange
        var cityData = "{}";
        _geocodingMock.Setup(x => x.GetLatLon(cityData)).Throws(new Exception());

        //Act
        var result = _controller.SunGet("szeged", "2024-03-29");

        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }


    [Test]
    public void SunGetReturnsOkResult()
    {
        //Arrange
        var expectedSun = new Sun();
        var expectedLatLon = new LatLon();
        var sunsetData = "{}";
        var latLonData = "{}";

        _geocodingMock.Setup(x => x.GetLatLon(It.IsAny<string>())).Returns(latLonData);
        _sunsetSunriseApiMock.Setup(x => x.GetSun(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<string>()))
            .Returns(sunsetData);
        _jsonProcessorMock.Setup(x => x.ProcessLatLon(latLonData)).Returns(expectedLatLon);
        _jsonProcessorMock.Setup(x => x.ProcessSun(sunsetData, It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedSun);
        
        //Act
        var result = _controller.SunGet("szeged", "2023-03-29");
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(((OkObjectResult)result.Result).Value, Is.EqualTo(expectedSun));

    }
}