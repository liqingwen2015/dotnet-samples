using MicroService.NacosClientSharp.Test.NacosServiceConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MicroService.NacosClientSharp.Test.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly NacosClient _nacosClient;
    private readonly IOptionsMonitor<ProductNacosServiceOptions> _optionsMonitor;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, NacosClient nacosClient,
        IOptionsMonitor<ProductNacosServiceOptions> optionsMonitor)
    {
        _logger = logger;
        _nacosClient = nacosClient;
        _optionsMonitor = optionsMonitor;
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
        var response = await _nacosClient.GetAsync("/WeatherForecast", _optionsMonitor.CurrentValue);

        var result = await response?.Content?.ReadFromJsonAsync<List<WeatherForecast>>();

        return result;
    }
}