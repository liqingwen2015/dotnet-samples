using MicroService.CorrelationId;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Product.Controllers
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
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICorrelationIdProvider correlationIdProvider)
        {
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation($"{_correlationIdProvider.Get()}");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                CorrelationId = _correlationIdProvider.Get(),
            })
            .ToArray();
        }
    }
}