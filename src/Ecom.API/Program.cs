using Ecom.API.Extensions;
using Ecom.API.MiddleWare;
using Ecom.Infrastructure;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiRegestraion();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
        {
            var securitySchema = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWt Auth Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            s.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
            s.AddSecurityRequirement(securityRequirement);
        });
builder.Services.InfrastructureConfiguration(builder.Configuration);

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

//app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();



app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
InfrastructureRegestration.InfrastructureConfigMiddleware(app);

app.Run();
