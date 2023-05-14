using Nacos.V2;
using Nacos.V2.Naming.Dtos;

namespace MicroService.NacosClientSharp
{
    public class NacosServiceInstanceSelect : IServiceInstanceSelect
	{
		private INacosNamingService _nacosNamingService;

		public NacosServiceInstanceSelect(INacosNamingService nacosNamingService)
		{
			_nacosNamingService = nacosNamingService;
		}

		public async Task<ServiceInstance> SelectOneHealthInstance(string groupName, string serviceName)
		{
			Instance instance = await _nacosNamingService.SelectOneHealthyInstance(serviceName, groupName).ConfigureAwait(continueOnCapturedContext: false);
			if (instance == null)
			{
				return null;
			}
			string value;
			string schema = (instance.Metadata.TryGetValue("secure", out value) ? "https" : "http");
			return new ServiceInstance(instance.Ip, instance.Port, schema);
		}

		public async Task<ServiceInstance> SelectOneHealthInstance(string groupName, string serviceName, List<string> clustersOrVersion)
		{
			Instance instance = await _nacosNamingService.SelectOneHealthyInstance(serviceName, groupName, clustersOrVersion).ConfigureAwait(continueOnCapturedContext: false);
			if (instance == null)
			{
				return null;
			}
			string value;
			string schema = (instance.Metadata.TryGetValue("secure", out value) ? "https" : "http");
			return new ServiceInstance(instance.Ip, instance.Port, schema);
		}
	}
}
