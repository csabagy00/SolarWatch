using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
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
    private readonly SolarWatchContext _dbContext;

    public SunController(ILogger<SunController> logger, IJsonProcessor jsonProcessor, IGeocodingApi geocodingApi, ISunsetSunriseApi sunsetSunriseApi, SolarWatchContext dbContext)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _geocodingApi = geocodingApi;
        _sunsetSunriseApi = sunsetSunriseApi;
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("Get"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<Sun>> SunGet([Required]string city, [Required]string date)
    {
        _logger.Log(LogLevel.Information, "Get Request");

        var selectedSun = _dbContext.Suns.FirstOrDefault(s => s.City == city && s.Date == date);

        if (selectedSun != null)
        {
            Console.WriteLine("From DB");
            return Ok(selectedSun);
        }
        
        try
        {
            Console.WriteLine("From External API");
            string latLonData = await _geocodingApi.GetLatLon(city);

            LatLon latLon = _jsonProcessor.ProcessLatLon(latLonData);

            string sunData = await _sunsetSunriseApi.GetSun(latLon.Lat, latLon.Lon, date);
            
            City newCityEntity = _jsonProcessor.ProcessCity(latLonData);

            Sun sun = _jsonProcessor.ProcessSun(sunData, city, date);

            newCityEntity.LatLon = latLon;
            newCityEntity.Sun = sun;

            Console.WriteLine(sun.City);

            _sunsetSunriseApi.AddCity(newCityEntity);
            
            return Ok(sun);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error getting data \n{e.Message}");
            return StatusCode(400);
        }
    }

    [HttpPatch("Patch"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Sun>> SunPatch([Required]Sun sun)
    { 
        try
        {
            _logger.Log(LogLevel.Information, "Patch Request");

            _dbContext.Suns.Update(sun);
            await _dbContext.SaveChangesAsync();
            
            return Ok(sun);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while patching.");
            throw;
        }
    }

    [HttpPost("Post"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Sun>> SunPost([Required] Sun sun)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Post Request");

            _dbContext.Suns.Add(sun);
            await _dbContext.SaveChangesAsync();

            return Ok(sun);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}