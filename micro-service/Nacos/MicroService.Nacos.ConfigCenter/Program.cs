using MicroService.Nacos.ConfigCenter;
using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// �������ļ���ȡNacos�������
builder.WebHost.ConfigureAppConfiguration((context, builder) =>
{
    var c = builder.Build();
    // Ĭ�ϻ�ʹ��JSON����������������Nacos Server������
    builder.AddNacosV2Configuration(c.GetSection("NacosConfig"));
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//nacos����ע���뷢��
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