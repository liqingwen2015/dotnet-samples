using MicroService.UserHttpHead;
using MicroService.UserHttpHead.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AddSwaggerCustomHeaderFilter>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUserClaimsAccessor, UserClaimsAccessor>();
builder.Services.AddSingleton<UserMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<UserMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();