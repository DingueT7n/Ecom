using Ecom.API.Extensions;
using Ecom.API.MiddleWare;
using Ecom.Infrastructure;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiRegestraion();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InfrastructureConfigureation(builder.Configuration);

//Configuer Redis

builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
{
    var configuer = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"),true);
    return ConnectionMultiplexer.Connect(configuer);
}); 

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleWare>();

app.UseHttpsRedirection();


app.UseAuthorization();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");


app.MapControllers();

app.Run();
