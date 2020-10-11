using Microsoft.Extensions.DependencyInjection;
using System;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;

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

                    if (setup.IsValid)
                    {
                        throw new Exception($"Cannot initialyze elastci search {url} {index} {setup.OriginalException}");
                    }
                }

                return elasticSearchClient;
            });
        }
    }
}