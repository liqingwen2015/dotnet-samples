using MicroService.NacosClientSharp;
using MicroService.NacosClientSharp.Test.NacosServiceConfig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//方法一：注册NacosClient
builder.Services.AddNacosClient(builder.Configuration);

builder.Services.Configure<ProductNacosServiceOptions>(builder.Configuration.GetSection("RemoteService:ProductNacosServiceOptions"));

//方法二：注册NacosClient
// IHttpClientFactory设置忽略ssl错误
//https://stackoverflow.com/questions/57283898/ignore-ssl-connection-errors-via-ihttpclientfactory
//builder.Services.AddNacosClient(builder.Configuration, "nacos", "nacos");
//builder.Services.AddHttpClient(name: "nacos")
//                .ConfigureHttpMessageHandlerBuilder(builder =>
//                  {
//                      builder.PrimaryHandler = new HttpClientHandler
//                      {
//                          UseProxy = false,
//                          AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
//                          UseDefaultCredentials = false,
//                          ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
//                      };
//                  });

//Ast.Net Core 和 .Net Core 控制台日志输出加上时间
//https://stackoverflow.com/questions/45434653/log-event-datetime-with-net-core-console-logger
builder.Services.AddLogging(options =>
{
    options.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fffffff] ";
    });
});


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