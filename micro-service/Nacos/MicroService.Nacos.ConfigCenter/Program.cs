using MicroService.Nacos.ConfigCenter;
using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// 从配置文件读取Nacos相关配置
builder.WebHost.ConfigureAppConfiguration((context, builder) =>
{
    var c = builder.Build();
    // 默认会使用JSON解析器来解析存在Nacos Server的配置
    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//nacos服务注入与发现
builder.Services.AddNacosAspNet(builder.Configuration, "nacos");

builder.Services.Configure<List<SystemApiConfig>>(builder.Configuration.GetSection("SystemApiConfig"));

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