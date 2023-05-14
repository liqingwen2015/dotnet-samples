using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.ConfigAdapter;
using Com.Ctrip.Framework.Apollo.Logging;
using MicroService.Apollo.ConfigCenter.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//输出debug日志在控制台，方便查找问题
Com.Ctrip.Framework.Apollo.Logging.LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Debug);
var apolloInfo = builder.Configuration.GetSection("apollo");

builder.Configuration.AddApollo(apolloInfo)
    .AddDefault()//添加默认namespace: application
    .AddNamespace("OrderApiJsonConfig", Com.Ctrip.Framework.Apollo.Enums.ConfigFileFormat.Json)
    .AddNamespace("RedisConfig", Com.Ctrip.Framework.Apollo.Enums.ConfigFileFormat.Properties)
    .AddNamespace("RabbitMqConfig", Com.Ctrip.Framework.Apollo.Enums.ConfigFileFormat.Properties)
    ;


//builder.Services.Configure<UserJsonConfig>(builder.Configuration.GetSection("UserJsonConfig"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OrderPayOptions>(builder.Configuration.GetSection("OrderPayOptions"));

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