using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroService.Nacos.ConfigCenter.Controllers
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
        private readonly IOptionsMonitor<List<SystemApiConfig>> _options;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsMonitor<List<SystemApiConfig>> options)
        {
            _logger = logger;
            _options = options;
        }

        [HttpGet("GetWeatherForecast")]
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

        [HttpGet("GetConfig")]
        public IEnumerable<SystemApiConfig> GetConfig()
        {
            return _options.CurrentValue;
        }
    }
}