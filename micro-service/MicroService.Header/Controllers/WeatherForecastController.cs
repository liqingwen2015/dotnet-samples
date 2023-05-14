using Microsoft.AspNetCore.Mvc;

namespace MicroService.Header.Controllers
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
        private readonly IUserContext _userContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetCurrentUser")]
        public UserInfo GetCurrentUser()
        {
            return _userContext.GetCurrentUser();
        }
    }
}