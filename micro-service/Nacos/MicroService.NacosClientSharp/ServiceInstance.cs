namespace MicroService.NacosClientSharp
{
	public class ServiceInstance
	{
		public string Host { get; }

		public int Port { get; }

		public string Schema { get; set; }

		public ServiceInstance(string host, int port, string schema)
		{
			Host = host;
			Port = port;
			Schema = schema;
		}

		public override string ToString()
		{
			return $"{Schema}://{Host}:{Port}";
		}
	}
}