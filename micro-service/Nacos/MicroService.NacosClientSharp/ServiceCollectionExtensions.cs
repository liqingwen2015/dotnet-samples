using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nacos.AspNetCore.V2;

namespace MicroService.NacosClientSharp
{
    public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddNacosClient(this IServiceCollection services, IConfiguration configuration, 
													    string section = "nacos", string nacosDefaultHttpClient = "")
		{
			services.AddServiceRegistry(configuration, section);
			services.AddTransient(delegate (IServiceProvider sp)
			{
				var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nacosDefaultHttpClient);
				var requiredService = sp.GetRequiredService<IServiceInstanceSelect>();
				return new NacosClient(httpClient, requiredService, sp.GetService<ILogger<NacosClient>>());
			});
			return services;
		}

		public static IServiceCollection AddServiceRegistry(this IServiceCollection services, IConfiguration configuration, 
									     string section = "nacos")
		{
			services.AddNacosAspNet(configuration, section);
			services.AddSingleton<IServiceInstanceSelect, NacosServiceInstanceSelect>();
			return services;
		}
	}
}
