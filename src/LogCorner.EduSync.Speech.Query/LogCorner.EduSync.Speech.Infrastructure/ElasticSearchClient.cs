using Elasticsearch.Net;
using LogCorner.EduSync.Speech.ReadModel.SpeechReadModel;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Infrastructure
{
    public class ElasticSearchClient<T> : IElasticSearchClient<T> where T : Entity<Guid>
    {
        private readonly string _indexName;

        private ElasticClient _client;

        public ElasticSearchClient(string indexName)
        {
            _indexName = indexName;
        }

        public AcknowledgedResponseBase Init(string url)
        {
            var pool = new SingleNodeConnectionPool(new Uri(url));
            var connectionSettings =
                new ConnectionSettings(pool, (builtin, settings) => new JsonNetSerializer(
                    builtin, settings,
                    () => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore },
                    resolver => resolver.NamingStrategy = new SnakeCaseNamingStrategy()
                ))
                    .EnableDebugMode()
                    .PrettyJson()
                   .DefaultIndex(_indexName);

            _client = new ElasticClient(connectionSettings);

            var result = _client.Indices.Exists(Indices.Index(_indexName));
            if (result.Exists) return new CreateIndexResponse();
            var createIndexResponse = _client.Indices.Create(_indexName, c => c
                .Map<T>(m => m.AutoMap())
            );
            return createIndexResponse;
        }

        public async Task<IList<T>> Get()
        {
            var searchResponse = await _client.SearchAsync<T>(s => s
                .Query(q => q
                    .MatchAll()
                )
            );
            return searchResponse.Documents?.ToList();
        }

        public async Task<T> Get(Guid id)
        {
            var searchResponse = await _client.GetAsync<T>(id);
            return searchResponse.Source;
        }
    }
}