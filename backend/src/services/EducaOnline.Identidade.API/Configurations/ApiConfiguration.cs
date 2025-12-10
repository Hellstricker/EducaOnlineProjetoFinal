using EducaOnline.WebAPI.Core.Identidade;
using MeuProjeto.WebAPI.Core.Configuration;

namespace EducaOnline.Identidade.API.Configurations
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                         .WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                         .WithExposedHeaders("X-Pagination"));
            });


            services.AddControllers();

            services.AddHealthCheckConfig(services.BuildServiceProvider().GetRequiredService<IConfiguration>());

            return services;
        }

        public static WebApplication UseApiConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("Total");

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();

            app.UseHealthCheckConfig();

            app.MapControllers();            

            return app;
        }
    }
}
