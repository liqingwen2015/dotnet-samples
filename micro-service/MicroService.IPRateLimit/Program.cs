using AspNetCoreRateLimit;
using MicroService.IPRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Ìí¼ÓÅäÖÃ
builder.Host.ConfigureAppConfiguration((context, builder) =>
{
    builder.AddJsonFile("ratelimitconfig.json", optional: true, reloadOnChange: true);
});

builder.Services.AddIpRateLimit(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var rateLimitingOption = builder.Configuration.GetSection(nameof(RateLimitingOptions)).Get<RateLimitingOptions>();

if (rateLimitingOption.IpRateLimit)
{
    app.UseIpRateLimiting();
}

app.UseAuthorization();


app.MapControllers();

app.Run();