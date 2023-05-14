using MicroService.CorrelationId.Core;
using Microsoft.Extensions.Options;

namespace MicroService.CorrelationId
{
    public interface ICorrelationIdProvider
    {
        string Get();
    }
    public class AspNetCoreCorrelationIdProvider : ICorrelationIdProvider
    {
        protected IHttpContextAccessor HttpContextAccessor { get; }
        protected AbpCorrelationIdOptions Options { get; }
        public AspNetCoreCorrelationIdProvider(
            IHttpContextAccessor httpContextAccessor,
            IOptions<AbpCorrelationIdOptions> options)
        {
            HttpContextAccessor = httpContextAccessor;
            Options = options.Value;
        }

        public virtual string Get()
        {
            if (HttpContextAccessor.HttpContext?.Request?.Headers == null)
            {
                return CreateNewCorrelationId();
            }

            string correlationId = HttpContextAccessor.HttpContext.Request.Headers[Options.HttpHeaderName];

            if (correlationId.IsNullOrEmpty())
            {
                lock (HttpContextAccessor.HttpContext.Request.Headers)
                {
                    if (correlationId.IsNullOrEmpty())
                    {
                        correlationId = CreateNewCorrelationId();
                        HttpContextAccessor.HttpContext.Request.Headers[Options.HttpHeaderName] = correlationId;
                    }
                }
            }

            return correlationId;
        }
        protected virtual string CreateNewCorrelationId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
    public class CorrelationIdProvider : AspNetCoreCorrelationIdProvider, ICorrelationIdProvider
    {
        public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor, IOptions<AbpCorrelationIdOptions> options) 
            : base(httpContextAccessor, options)
        {

        }
        protected override string CreateNewCorrelationId()
        {
            var httpContext = this.HttpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                return $"{GetLocalIp()}:{Environment.MachineName}:{httpContext.TraceIdentifier}:{httpContext.Request.Path}"; //自定义链路ID
            }
            else
            {
                return base.CreateNewCorrelationId();
            }
        }

        /// <summary>
        /// 获取服务器本地Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            var addressList = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;
            var ips = addressList.Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(address => address.ToString()).ToArray();
            if (ips.Length == 1)
            {
                return ips.First();
            }
            return ips.Where(address => !address.EndsWith(".1")).FirstOrDefault() ?? ips.FirstOrDefault();
        }

        
    }

}