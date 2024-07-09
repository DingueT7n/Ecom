using Ecom.API.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Ecom.API.Extensions
{

    public static class ApiRegestration
    {

        public static IServiceCollection AddApiRegestraion(this IServiceCollection services)
        {

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                );
            services.Configure<ApiBehaviorOptions>(
                 opt =>
                  {
                      opt.InvalidModelStateResponseFactory = context =>
                      {
                          var errorResponse = new ApiValidationErrorResponse
                          {
                              Errors = context.ModelState
                              .Where(e => e.Value.Errors.Count > 0)
                              .SelectMany(x => x.Value.Errors)
                              .Select(x => x.ErrorMessage).ToArray()
                          };
                          return new BadRequestObjectResult(errorResponse);
                      };

                  });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .WithOrigins("https://localhost:4200");
                });
            });
            return services; 
        }
    }
}
