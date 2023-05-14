using MicroService.NacosWebClientCore.RemoteService;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.NacosWebClientCore.Controllers
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
        private readonly IProductApi _productApi;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IProductApi productApi)
        {
            _logger = logger;
            _productApi = productApi;
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

        /// <summary>
        /// 获取nacos远程产品服务
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRemoteProductService")]
        public async Task<IEnumerable<WeatherForecast>> GetRemoteProductService()
        {
            var result = await _productApi.GetWeatherForecast();

            return result;
        }
    }
}