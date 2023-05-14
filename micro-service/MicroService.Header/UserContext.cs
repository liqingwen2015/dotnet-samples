using System.Text;
using Newtonsoft.Json;

namespace MicroService.Header;

public interface IUserContext
{
    UserInfo GetCurrentUser();
}

public class UserContext : IUserContext
{
    private const string UserHttpHeaderKeyName = "x-User-Info-Encode";

    private HttpContext _httpContext;
    private readonly ILogger<UserContext> _logger;
    public UserContext(IHttpContextAccessor contextAccessor,
        IServiceScopeFactory serviceScopeFactory)
    {
        _httpContext = contextAccessor.HttpContext;

        var scrop = serviceScopeFactory.CreateScope();
        _logger = scrop.ServiceProvider.GetRequiredService<ILogger<UserContext>>();
    }

    public UserInfo GetCurrentUser()
    {
        var currentUser = GetUserGuidFromHttpHeader();

        return currentUser;
    }
    private UserInfo GetUserGuidFromHttpHeader()
    {
        try
        {
            if (_httpContext.Request.Headers.TryGetValue(UserHttpHeaderKeyName, out var base64StringUserInfo))
            {
                var userInfo = Encoding.Default.GetString(Convert.FromBase64String(base64StringUserInfo));

                var userInfoEntity = JsonConvert.DeserializeObject<UserInfo>(userInfo);

                return userInfoEntity;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "用户请求头认证信息不存在...");
        }
        return null;
    }


}