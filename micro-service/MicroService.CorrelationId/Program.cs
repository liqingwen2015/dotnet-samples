namespace MicroService.CorrelationId
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<AbpCorrelationIdOptions>(options =>
            {
                options.HttpHeaderName = "x-request-id";
                options.SetResponseHeader = true;//�Ƿ񷵻���Ӧͷ
            });

            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<ICorrelationIdProvider, AspNetCoreCorrelationIdProvider>();
            builder.Services.AddTransient<ICorrelationIdProvider, CorrelationIdProvider>();
            builder.Services.AddTransient<AbpCorrelationIdMiddleware>();

            builder.Services.AddControllers(options =>
            {
                //options.Filters.Add<ResultWrapperFilter>();
                //options.Filters.Add<GlobalExceptionFilter>();
            });

            builder.Services.AddHttpClient();

            var app = builder.Build();
            app.UseMiddleware<AbpCorrelationIdMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}