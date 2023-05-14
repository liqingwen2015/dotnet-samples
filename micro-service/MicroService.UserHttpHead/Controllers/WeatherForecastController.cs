using MicroService.UserHttpHead.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.UserHttpHead.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUserClaimsAccessor _userClaimsAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserClaimsAccessor userClaimsAccessor)
        {
            _logger = logger;
            _userClaimsAccessor = userClaimsAccessor;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
                .ToArray();
        }

        [HttpGet("GetUserClaimsInfo")]
        public IActionResult GetUserClaimsInfo()
        {
            return Ok($@"{nameof(IUserClaimsAccessor.UserId)}={_userClaimsAccessor.UserId},
                         {nameof(IUserClaimsAccessor.UserName)}={_userClaimsAccessor.UserName}
                         {nameof(IUserClaimsAccessor.OrgId)}={_userClaimsAccessor.OrgId}
                    ");
        }
    }
}