using Elasticsearch.Net;
using LogCorner.EduSync.Speech.Infrastructure.Model;
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
                    .EnableDebugMode().IncludeServerStackTraceOnError()
                    .PrettyJson()
                   .DefaultIndex(_indexName);

            _client = new ElasticClient(connectionSettings);

            var result = _client.Indices.Exists(Indices.Index(_indexName));
            if (result.Exists) return new CreateIndexResponse
            {
                ShardsAcknowledged = true
            }
                 ;
            var createIndexResponse = _client.Indices.Create(_indexName, c => c
                .Map<T>(m => m.AutoMap())
            );
            Console.WriteLine(createIndexResponse.DebugInformation);
            return createIndexResponse;
        }

        public async Task<IList<T>> Get()
        {
            var searchResponse = await _client.SearchAsync<T>(s => s
                .Query(q => q
                    .MatchAll()
                )
            );
            Console.WriteLine(searchResponse.DebugInformation);
            return searchResponse.Documents?.ToList();
        }

        public async Task<SearchResult<T>> Get(int page, int pageSize)
        {
            if (page <= 0)
                throw new ArgumentNullException($"page {page} is not valid, page number should be greater than 0");

            if (pageSize > 10)
                pageSize = 10;

            var from = (page - 1) * pageSize;

            var searchResponse = await _client.SearchAsync<T>(s => s
                .Query(q => q
                    .MatchAll()
                )
                .From(from)
                .Size(pageSize)
            );
            Console.WriteLine(searchResponse.DebugInformation);
            return new SearchResult<T>
            {
                Total = searchResponse.Total,
                Page = page,
                PageSize = pageSize,
                Results = searchResponse.Documents.ToList(),
                ElapsedMilliseconds = searchResponse.Took
            };
        }

        public async Task<T> Get(Guid id)
        {
            var searchResponse = await _client.GetAsync<T>(id);
            Console.WriteLine(searchResponse.DebugInformation);
            return searchResponse.Source;
        }
    }
}