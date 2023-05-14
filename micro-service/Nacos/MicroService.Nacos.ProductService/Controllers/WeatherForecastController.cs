using Microsoft.AspNetCore.Mvc;
using Nacos.V2;

namespace MicroService.Nacos.ProductService.Controllers
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
        private readonly INacosNamingService _nacosNamingService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, INacosNamingService nacosNamingService)
        {
            _logger = logger;
            _nacosNamingService = nacosNamingService;
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

        [HttpGet("GetOrderService")]
        public async Task<IActionResult> TetAsync()
        {
            // 这里需要知道被调用方的服务名
            var instance = await _nacosNamingService.SelectOneHealthyInstance(serviceName: "ab-order-service", groupName: "global");

            var host = $"{instance.Ip}:{instance.Port}";

            var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                        ? $"https://{host}"
                        : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return Ok("empty");
            }

            var url = $"{baseUrl}/WeatherForecast";

            using HttpClient client = new HttpClient();
            var result = await client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }
}