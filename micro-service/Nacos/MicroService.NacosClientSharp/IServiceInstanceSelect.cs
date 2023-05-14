namespace MicroService.NacosClientSharp
{
	public interface IServiceInstanceSelect
	{
		Task<ServiceInstance> SelectOneHealthInstance(string groupName, string serviceName);

		Task<ServiceInstance> SelectOneHealthInstance(string groupName, string serviceName, List<string> clustersOrVersion);
	}

}