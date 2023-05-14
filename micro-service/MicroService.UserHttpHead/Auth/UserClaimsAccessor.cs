using System.Runtime.CompilerServices;

namespace MicroService.UserHttpHead.Auth
{
    public interface IUserClaimsAccessor
    {
        string UserId { get; }
        string UserName { get; }
        string OrgId { get; }
        string FindClaim(string userClaimTypeName);
    }

    public class UserClaimsAccessor : IUserClaimsAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserClaimsAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId => FindClaim(UserClaimTypes.UserId);

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName => FindClaim();//等于:public string UserName => FindClaim(UserClaimTypes.UserName);

        public string FindClaim([CallerMemberName] string userClaimTypeName = null)
        {
            var value = _httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == userClaimTypeName)?.Value;

            return value ?? string.Empty;
        }

        public string OrgId => this.GetOrgId();
    }
    public static class CurrentUserExtensions
    {
        public static string GetOrgCode(this IUserClaimsAccessor currentUser)
        {
            return currentUser.FindClaim(UserClaimTypes.OrgCode);
        }
        public static string GetOrgName(this IUserClaimsAccessor currentUser)
        {
            return currentUser.FindClaim(UserClaimTypes.OrgName);
        }
        public static string GetOrgId(this IUserClaimsAccessor currentUser)
        {
            return currentUser.FindClaim(UserClaimTypes.OrgId);
        }
        public static string GetOrgIdentitfier(this IUserClaimsAccessor currentUser)
        {
            return currentUser.FindClaim(UserClaimTypes.OrgIdentitfier);
        }
    }
}
