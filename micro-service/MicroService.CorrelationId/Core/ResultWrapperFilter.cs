using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace MicroService.CorrelationId.Core
{
    /// <summary>
    /// ASP.NET Core WebApi返回结果统一包装实践
    /// https://www.cnblogs.com/wl-blog/p/16140104.html
    /// </summary>
    public class ResultWrapperFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var correlationIdProvider = context.HttpContext.RequestServices.GetRequiredService<ICorrelationIdProvider>();
            string correlationId = correlationIdProvider.Get();

            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AbpCorrelationIdOptions>>().Value;

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionWrapper = controllerActionDescriptor?.MethodInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();
            var controllerWrapper = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttributes(typeof(NoWrapperAttribute), false).FirstOrDefault();

            //如果包含NoWrapperAttribute则说明不需要对返回结果进行包装，直接返回原始值
            if (actionWrapper != null || controllerWrapper != null)
            {
                return;
            }

            //根据实际需求进行具体实现
            var rspResult = new ResponseResult<object>();

            if (options.SetResponseHeader)
            {
                rspResult.RequestId = correlationId;
            }

            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult?.Value == null)
                {
                    rspResult.Status = ResultStatus.Fail;
                    rspResult.Message = "未找到资源";
                    context.Result = new ObjectResult(rspResult);
                }
                else
                {
                    //如果返回结果已经是ResponseResult<T>类型的则不需要进行再次包装了
                    if (objectResult.DeclaredType?.IsGenericType is true &&
                        objectResult.DeclaredType?.GetGenericTypeDefinition() == typeof(ResponseResult<>))
                    {
                        return;
                    }
                    rspResult.Data = objectResult.Value;
                    context.Result = new ObjectResult(rspResult);
                }
            }
        }
    }
}