using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MicroService.Header;

public class AddSwaggerCustomHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-User-Info-Encode",
            In = ParameterLocation.Header,
            Description = "参数备注",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString("用户身份信息base64字符串格式")
                //eyJ1c2VyTmFtZSI6IumYv+esqE5FVCIsInVzZXJJZCI6ImE3MjViYmZiLWVhYTYtNDZmYi1iMTUwLWQzMmZmMjE1MTc0YyIsIm9yZ0lkIjoiMDlzNDI0MzEtMmVkOC00Mi1hNjM3LWYxMnMyMXNjOTkxZCIsIm9yZ05hbWUiOiLlub/kuJzmt7HlnLMiLCJvcmdDb2RlIjoiQTAwMDEifQ==
            }
        });
    }
}