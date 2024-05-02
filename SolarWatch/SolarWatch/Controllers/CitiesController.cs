using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ILogger<CitiesController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IGeocodingApi _geocodingApi;
    private readonly ISunsetSunriseApi _sunsetSunriseApi;
    private readonly SolarWatchContext _dbContext;


    public CitiesController(ILogger<CitiesController> logger, SolarWatchContext dbContext, ISunsetSunriseApi sunsetSunriseApi, IGeocodingApi geocodingApi, IJsonProcessor jsonProcessor)
    {
        _logger = logger;
        _dbContext = dbContext;
        _sunsetSunriseApi = sunsetSunriseApi;
        _geocodingApi = geocodingApi;
        _jsonProcessor = jsonProcessor;
    }


    [HttpGet("Get"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<City>> CitiesGet()
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: GET Request");

            List<City> cities = _dbContext.Cities.ToList();

            return Ok(cities);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while getting data.");
            throw;
        }
    }

    [HttpPatch("Patch"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> CityPatch(int id, string name, string country, string? state)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: PATCH Request");

            var selected = _dbContext.Cities.FirstOrDefault(c => c.Id == id);

            if (selected == null)
                return NotFound();

            selected.Name = name;
            selected.Country = country;
            selected.State = string.IsNullOrWhiteSpace(state) ? null : state;
            
            await _dbContext.SaveChangesAsync();

            return Ok(selected);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while patching.");
            throw;
        }
    }

    [HttpPost("Post"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> CityPost(string name, string country, string? state)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: POST Request");

            string latLonData = await _geocodingApi.GetLatLon(name);
            LatLon latLon = _jsonProcessor.ProcessLatLon(latLonData);

            DateTime now = DateTime.Now;

            string dateFormat = $"{now.Year}-{now.Month}-{now.Day}";
            
            string sunData = await _sunsetSunriseApi.GetSun(latLon.Lat, latLon.Lon, dateFormat);
            Sun sun = _jsonProcessor.ProcessSun(sunData, name, dateFormat);

            City city = new City { Name = name, Country = country, State = state, LatLon = latLon, Sun = sun};
            
            _dbContext.Cities.Add(city);
            await _dbContext.SaveChangesAsync();

            return Ok(city);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while posting.");
            throw;
        }
    }

    [HttpDelete("Delete"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<City>> CityDelete([Required] int id)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: DELETE Request");
            var selected = _dbContext.Cities.FirstOrDefault(c => c.Id == id);

            if (selected == null)
                return NotFound();

            _dbContext.Cities.Remove(selected);
            await _dbContext.SaveChangesAsync();

            return Ok(selected);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occured while deleting.");
            throw;
        }
    }
    
}