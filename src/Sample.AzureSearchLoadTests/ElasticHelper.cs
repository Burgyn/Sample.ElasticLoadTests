using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Eql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.ElasticLoadTests
{
    class ElasticHelper
    {
        const string ElasticServerUri = "https://localhost:9200"; // "https://localhost:9200";
        const string ElasticServerUsername = "elastic";
        const string ElasticServerPassword = "strongpasswordmino";
        const string ElasticIndexName = "orders";

        private readonly ElasticsearchClient _elasticClient;

        public ElasticHelper()
        {
            _elasticClient = ElasticClient.Create(
                ElasticServerUri, ElasticServerUsername, ElasticServerPassword, ElasticIndexName);
        }

        public async Task<long> UploadDocumentsAsync(IEnumerable<Order> documents)
        {
            BulkResponse response = await _elasticClient.IndexManyAsync(documents);
            if (!response.IsValidResponse)
            {
                Console.WriteLine($"Batch failed to index invoices.");
                Console.WriteLine(response.DebugInformation);
            }
            //await _elasticClient.Indices.RefreshAsync(ElasticIndexName);
            return response.Took;
        }

        public async Task<long> GetDocumentsCountAsync()
        {
            var c = (await _elasticClient.CountAsync(c => c.Index(ElasticIndexName)));

            return c.Count;
        }
    }
}
