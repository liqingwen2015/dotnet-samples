namespace MicroService.NacosClientSharp
{
	public class NacosDiscoveryOptions
	{
		public const string Name = "NacosDiscoveryOptions";

		public string GroupName { get; set; }

		public string ServiceName { get; set; }

		public List<string> ClustersOrVersion { get; set; } = new List<string>();

	}
}