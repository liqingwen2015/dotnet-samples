using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroService.Apollo.ConfigCenter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : Controller
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<OrderPayOptions> _optionsMonitor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IConfiguration configuration,
            IOptionsMonitor<OrderPayOptions> optionsMonitor)
        {
            _logger = logger;
            _configuration = configuration;
            _optionsMonitor = optionsMonitor;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("config1")]
        public IActionResult GetConfig1()
        {
            var test = _configuration.GetValue<string>("test", defaultValue: "如果配置不存在，就使用默认值");
            var test1 = _configuration.GetValue<string>("test1", "0000");

            //读取json配置文件
            var redisOptions = _configuration.GetSection("OrderPayOptions").Get<OrderPayOptions>();

            var redisIp = _configuration.GetValue<string>("redis:ip", "");

            var rabbitmqUrl = _configuration.GetValue<string>("rabbitmq:url", "");

            return Json(redisOptions);
        }
    }
}