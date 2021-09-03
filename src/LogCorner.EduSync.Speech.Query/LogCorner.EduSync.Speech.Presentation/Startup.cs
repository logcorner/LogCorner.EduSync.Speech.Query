using LogCorner.EduSync.Speech.Application.UseCases;
using LogCorner.EduSync.Speech.Infrastructure;
using LogCorner.EduSync.Speech.Presentation.Exceptions;
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
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.WithOrigins(Configuration["allowedOrigins"].Split(","))
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                         );
            });

            services.AddCustomAuthentication(Configuration);
            services.AddCustomSwagger();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           // app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseRouting();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    var OAuthClientId = Configuration["SwaggerUI:OAuthClientId"];
                    var OAuthClientSecret = Configuration["SwaggerUI:OAuthClientSecret"];
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
                    c.OAuthClientId(OAuthClientId);
                    c.OAuthClientSecret(OAuthClientSecret);
                    c.OAuthAppName("The Speech Micro Service Query Swagger UI");
                    c.OAuthScopeSeparator(" ");
                    c.OAuthUsePkce();
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}