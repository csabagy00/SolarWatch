using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SolarWatch.Controllers;


[ApiController]
[Route("[controller]")]
public class SunController : ControllerBase
{
    public ILogger<SunController> _logger;

    public SunController(ILogger<SunController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("Get")]
    public ActionResult<Sun> SunGet([Required]string city, [Required]string date)
    {
        _logger.Log(LogLevel.Information, "Get Request");
        
        
        
        return Ok();
    }
}