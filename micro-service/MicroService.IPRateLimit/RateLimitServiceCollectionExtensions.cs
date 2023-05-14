using AspNetCoreRateLimit;

namespace MicroService.IPRateLimit
{
    public static class RateLimitServiceCollectionExtensions
    {
        /// <summary>
        /// IPLimit限流和ClientId限流 启动服务
        /// 这个服务同时配置了IP限流和clientid限流两部分
        /// </summary>
        public static IServiceCollection AddIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMemoryCache();

            ////load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));


            // register stores
            services.AddInMemoryRateLimiting();

            // configure the resolvers
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            return services;
        }
    }
}