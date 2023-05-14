namespace MicroService.Header;

/// <summary>
/// 网关转换后的用户信息
/// </summary>
public class UserInfo
{
    /// <summary>
    /// 网关转换用户名
    /// </summary>
    public string userName { get; set; }
    /// <summary>
    /// 网关转换用户ID
    /// </summary>
    public string userId { get; set; }
    /// <summary>
    /// 网关转换组织ID
    /// </summary>
    public string orgId { get; set; }
    /// <summary>
    /// 网关转换组织名称
    /// </summary>
    public string orgName { get; set; }
    /// <summary>
    /// 网关转换组织编码
    /// </summary>
    public string orgCode { get; set; }
}