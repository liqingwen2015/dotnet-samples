namespace MicroService.Nacos.ConfigCenter
{
    public class SystemApiConfig
    {
        /// <summary>
        /// API唯一代码
        /// </summary>
        public int ApiCode { get; set; }

        /// <summary>
        /// API名称
        /// </summary>
        public string ApiName { get; set; }

        /// <summary>
        /// API接口基地址
        /// </summary>
        public string ApiBaseUrl { get; set; }

        /// <summary>
        /// 接口地址代码
        /// </summary>
        public string ApiAddressCode { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string AuthCode { get; set; }
    }
}
