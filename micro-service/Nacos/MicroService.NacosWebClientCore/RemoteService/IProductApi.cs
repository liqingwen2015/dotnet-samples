using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace MicroService.NacosWebClientCore.RemoteService
{
    // for webapiclient/core
    //[HttpHost("http://ab-product-service")]
    [LoggingFilter]
    public interface IProductApi : IHttpApi
    {
        [HttpGet("/WeatherForecast")]
        Task<List<WeatherForecast>> GetWeatherForecast();
    }
}
