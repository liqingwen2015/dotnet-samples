using MicroService.CorrelationId;

namespace MicroService.Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<AbpCorrelationIdOptions>(options =>
            {
                options.HttpHeaderName = "x-request-id";
            });

            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<ICorrelationIdProvider, AspNetCoreCorrelationIdProvider>();
            builder.Services.AddTransient<ICorrelationIdProvider, CorrelationIdProvider>();

            var app = builder.Build();

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