using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace MicroService.UserHttpHead.Auth
{
    public class UserMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var currentHttpContextAccessor = context.RequestServices
                                               .GetRequiredService<IHttpContextAccessor>();

            if (context.Request.Headers.TryGetValue("x-User-Info-Encode", out var base64))
            {
                var userJson = Encoding.Default.GetString(Convert.FromBase64String(base64));
                var userBaseDto = JsonConvert.DeserializeObject<UserInfoHeader>(userJson);

                if (userBaseDto != null)
                {
                    var identity = new ClaimsIdentity(
                                        new Claim[]
                                        {
                                            new Claim(UserClaimTypes.UserId, userBaseDto.userId?? ""),
                                            new Claim(UserClaimTypes.UserName,userBaseDto.userName?? ""),
                                            new Claim(UserClaimTypes.OrgId,userBaseDto.orgId?? ""),
                                            new Claim(UserClaimTypes.OrgName,userBaseDto.orgName ?? ""),
                                        });
                    currentHttpContextAccessor?.HttpContext?.User?.AddIdentity(identity);
                }
            }
            await next(context);
        }
    }
}
