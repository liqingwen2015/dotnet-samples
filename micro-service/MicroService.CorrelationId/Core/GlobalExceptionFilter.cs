using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace MicroService.CorrelationId.Core
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var correlationIdProvider = context.HttpContext.RequestServices.GetRequiredService<ICorrelationIdProvider>();
            string correlationId = correlationIdProvider.Get();
            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpCorrelationIdOptions>>().Value;

            //异常返回结果包装
            var rspResult = ResponseResult<object>.ErrorResult(context.Exception.Message);

            if (options.SetResponseHeader)
            {
                rspResult.RequestId = correlationIdProvider.Get();
            }

            //日志记录
            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;
            context.Result = new InternalServerErrorObjectResult(rspResult);

            if (context.HttpContext.Response.Headers.ContainsKey(options.HttpHeaderName))
            {
                return;
            }
            context.HttpContext.Response.Headers[options.HttpHeaderName] = correlationId;
        }
        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}