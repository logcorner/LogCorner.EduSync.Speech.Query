using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Infrastructure;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogCorner.EduSync.Speech.Presentation
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var elasticSearchUrl = Configuration["elasticSearchUrl"];
            services.AddScoped<ISpeechUseCase, SpeechUseCase>();
            services.AddElasticSearch<SpeechView>(elasticSearchUrl, "speech");

            services.AddCors(options =>
            {
                var allowedOrigins = Configuration["allowedOrigins"];
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(allowedOrigins.Split(","))
                            .AllowAnyOrigin()
                            .AllowAnyMethod();
                    });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            //  app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}