using Microsoft.AspNetCore.Mvc;

namespace MicroService.CorrelationId.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase//  ApiControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICorrelationIdProvider _correlationIdProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICorrelationIdProvider correlationIdProvider,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _correlationIdProvider = correlationIdProvider;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            _logger.LogInformation($"{_correlationIdProvider.Get()}");
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();
            return Ok(data);
        }

        [HttpGet("GetProudct")]
        public async Task<IActionResult> GetProudct()
        {
            //如果请求不是用户发起的，调用方也是可以自己传递一个request_id。
            var httpClient = _httpClientFactory.CreateClient();
            var existsRequestIdHeader = HttpContext.Request.Headers.TryGetValue("x-request-id", out var requestIdHeader);

            if (existsRequestIdHeader)
            {
                httpClient.DefaultRequestHeaders.Add("x-request-id", requestIdHeader.ToString());
            }

            _logger.LogInformation($"{_correlationIdProvider.Get()}");

            var httpresponse = await httpClient.GetAsync("http://localhost:5191/WeatherForecast");
            var result = await httpresponse.Content.ReadFromJsonAsync<List<WeatherForecast>>();

            return Ok(result);
        }
    }
}