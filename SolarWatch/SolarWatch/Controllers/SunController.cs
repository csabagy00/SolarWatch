using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;


[ApiController]
[Route("[controller]")]
public class SunController : ControllerBase
{
    private readonly ILogger<SunController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IGeocodingApi _geocodingApi;
    private readonly ISunsetSunriseApi _sunsetSunriseApi;

    public SunController(ILogger<SunController> logger, IJsonProcessor jsonProcessor, IGeocodingApi geocodingApi, ISunsetSunriseApi sunsetSunriseApi)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _geocodingApi = geocodingApi;
        _sunsetSunriseApi = sunsetSunriseApi;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<ActionResult<Sun>> SunGet([Required]string city, [Required]string date)
    {
        _logger.Log(LogLevel.Information, "Get Request");
        try
        {
            string latLonData = await _geocodingApi.GetLatLon(city);

            LatLon latLon = _jsonProcessor.ProcessLatLon(latLonData);

            string sunData = await _sunsetSunriseApi.GetSun(latLon.Lat, latLon.Lon, date);

            Sun sun = _jsonProcessor.ProcessSun(sunData, city, date);
            
            return Ok(sun);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error getting data \n{e.Message}");
            return StatusCode(400);
        }
        
    }
}