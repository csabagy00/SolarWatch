using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Data;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ILogger<CitiesController> _logger;
    private readonly SolarWatchContext _dbContext;


    public CitiesController(ILogger<CitiesController> logger, SolarWatchContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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
    public async Task<ActionResult<City>> CityPatch(int id, string name, string country, string state)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: PATCH Request");

            var selected = _dbContext.Cities.FirstOrDefault(c => c.Id == id);

            if (selected == null)
                return NotFound();

            selected.Name = name;
            selected.Country = country;
            selected.State = string.IsNullOrEmpty(state) ? null : state;
            
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
    public async Task<ActionResult<City>> CityPost(string name, string country, string state)
    {
        try
        {
            _logger.Log(LogLevel.Information, "Cities: POST Request");

            LatLon latLon = new LatLon();
            Sun sun = new Sun();

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