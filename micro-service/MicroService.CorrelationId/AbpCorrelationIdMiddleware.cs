using Microsoft.Extensions.Options;

namespace MicroService.CorrelationId
{
    public class AbpCorrelationIdMiddleware : IMiddleware
    {
        private readonly AbpCorrelationIdOptions _options;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public AbpCorrelationIdMiddleware(IOptions<AbpCorrelationIdOptions> options,
            ICorrelationIdProvider correlationIdProvider)
        {
            _options = options.Value;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 1、生成唯一请求链路id
            // 首先判断请求头是否包含用户自定义传递的request_id，如果有，则不生成。否则生成唯一的request_id(服务器id+机器名+请求唯一路径)
            var correlationId = _correlationIdProvider.Get();

            try
            {
                // 2、调用下一个中间件之前，将当前的请求添加上响应头HttpHeader。
                CheckAndSetCorrelationIdOnResponse(context, _options, correlationId);
            }
            finally
            {
                await next(context);
            }
        }

        protected virtual void CheckAndSetCorrelationIdOnResponse(
            HttpContext httpContext,
            AbpCorrelationIdOptions options,
            string correlationId)
        {
            if (httpContext.Response.HasStarted)
            {
                return;
            }

            if (!options.SetResponseHeader)
            {
                return;
            }

            if (httpContext.Response.Headers.ContainsKey(options.HttpHeaderName))
            {
                return;
            }
            httpContext.Response.Headers[options.HttpHeaderName] = correlationId;
        }
    }
}