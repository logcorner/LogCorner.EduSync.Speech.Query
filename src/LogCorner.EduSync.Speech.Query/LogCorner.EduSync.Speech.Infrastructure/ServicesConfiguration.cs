using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace LogCorner.EduSync.Speech.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static void AddElasticSearch<T>(this IServiceCollection services, string url, string index) where T : Entity<Guid>
        {
            services.AddScoped<IElasticSearchClient<T>, ElasticSearchClient<T>>(ctx =>
            {
                var elasticSearchClient = new ElasticSearchClient<T>(index);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    var setup = elasticSearchClient.Init(url);

                    if (setup.ServerError != null)
                    {
                        throw new Exception($"Cannot initialyze elasticsearch {url} - {index} - {setup.ServerError} - {setup.OriginalException}");
                    }
                }

                return elasticSearchClient;
            });
        }
    }
}