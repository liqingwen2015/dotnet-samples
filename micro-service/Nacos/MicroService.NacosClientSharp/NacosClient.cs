using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MicroService.NacosClientSharp
{
    public class NacosClient
	{
		private readonly IServiceInstanceSelect serviceInstanceSelect;

		private readonly ILogger<NacosClient> logger;

		public HttpClient HttpClient { get; }

		public NacosClient(HttpClient httpClient, IServiceInstanceSelect serviceInstanceSelect, ILogger<NacosClient> logger)
		{
			HttpClient = httpClient;
			this.serviceInstanceSelect = serviceInstanceSelect;
			this.logger = logger;
		}

		public async Task<HttpResponseMessage> GetAsync(string url, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
			request.Options.Set(new HttpRequestOptionsKey<NacosDiscoveryOptions>("NacosDiscoveryOptions"), options);
			return await SendAsync(request, options, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PostStringAsync(string url, string body, string mediaType = "application/json", string encoding = "utf-8", NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(body, Encoding.GetEncoding(encoding), mediaType);
			return await PostAsync(url, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
			{
				Content = content
			};
			return await SendAsync(request, options, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PutStringAsync(string url, string body, string mediaType = "application/json", string encoding = "utf-8", NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(body, Encoding.GetEncoding(encoding), mediaType);
			return await PutAsync(url, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url)
			{
				Content = content
			};
			return await SendAsync(request, options, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> DeleteStringAsync(string url, string body, string mediaType = "application/json", string encoding = "utf-8", NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(body, Encoding.GetEncoding(encoding), mediaType);
			return await PutAsync(url, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> DeleteAsync(string url, HttpContent content = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url)
			{
				Content = content
			};
			return await SendAsync(request, options, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, NacosDiscoveryOptions options, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default(CancellationToken))
		{
			Uri originRequestUri = request.RequestUri;
			if (!originRequestUri.IsAbsoluteUri)
			{
				ServiceInstance serviceInstance = null;
				if (options != null)
				{
					serviceInstance = await serviceInstanceSelect.SelectOneHealthInstance(options.GroupName, options.ServiceName, options.ClustersOrVersion).ConfigureAwait(continueOnCapturedContext: false);
				}
				logger.LogDebug("the host from the nacos srv is " + serviceInstance);
				if (!string.IsNullOrEmpty(serviceInstance?.Host))
				{
					request.RequestUri = new Uri(new Uri(serviceInstance.ToString()), originRequestUri);
				}
				else
				{
					logger.LogDebug("failed to get the service addr list from the nacos.");
				}
			}
			logger.LogDebug($"the requesturi is {request.RequestUri}");
			return await HttpClient.SendAsync(request, completionOption, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> GetAsync(UriBuilder builder, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await SendAsync(builder, null, null, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PostJsonAsync(UriBuilder builder, object data = null, NacosDiscoveryOptions options = null, Func<object, string> funcSerializeObject = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await SendJsonAsync(builder, HttpMethod.Post, data, options, funcSerializeObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PostStringAsync(UriBuilder builder, string data = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(data);
			return await SendStringAsync(builder, HttpMethod.Post, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PutJsonAsync(UriBuilder builder, object data = null, NacosDiscoveryOptions options = null, Func<object, string> funcSerializeObject = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await SendJsonAsync(builder, HttpMethod.Put, data, options, funcSerializeObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> PutStringAsync(UriBuilder builder, string data = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(data);
			return await SendStringAsync(builder, HttpMethod.Put, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> DeleteJsonAsync(UriBuilder builder, object data = null, NacosDiscoveryOptions options = null, Func<object, string> funcSerializeObject = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await SendJsonAsync(builder, HttpMethod.Delete, data, options, funcSerializeObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> DeleteJsonAsync(UriBuilder builder, string data = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			using StringContent content = new StringContent(data);
			return await SendStringAsync(builder, HttpMethod.Delete, content, options, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> SendJsonAsync(UriBuilder builder, HttpMethod method = null, object data = null, NacosDiscoveryOptions options = null, Func<object, string> funcSerializeObject = null, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, Action<HttpRequestMessage> configure = null)
		{
			string jsonData = JsonConvert.SerializeObject(data);
			using StringContent content = new StringContent(jsonData, Encoding.GetEncoding("utf-8"), "application/json");
			return await SendAsync(builder, method, content, options, cancellationToken, completionOption, configure).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> SendStringAsync(UriBuilder builder, HttpMethod method = null, StringContent content = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, Action<HttpRequestMessage> configure = null)
		{
			return await SendAsync(builder, method, content, options, cancellationToken, completionOption, configure).ConfigureAwait(continueOnCapturedContext: false);
		}

		public async Task<HttpResponseMessage> SendAsync(UriBuilder builder, HttpMethod method = null, HttpContent content = null, NacosDiscoveryOptions options = null, CancellationToken cancellationToken = default(CancellationToken), HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, Action<HttpRequestMessage> configure = null)
		{
			Uri requestUri = ((builder.Port < 1) ? new Uri(builder.Path+builder.Query, UriKind.RelativeOrAbsolute) : builder.Uri);
			using HttpRequestMessage request = new HttpRequestMessage(method ?? HttpMethod.Get, requestUri)
			{
				Content = content
			};
			configure?.Invoke(request);
			return await SendAsync(request, options, completionOption, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}
}
