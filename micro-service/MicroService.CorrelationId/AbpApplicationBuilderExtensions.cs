namespace MicroService.CorrelationId
{
    public static class AbpApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<AbpCorrelationIdMiddleware>();
        }
    }
}