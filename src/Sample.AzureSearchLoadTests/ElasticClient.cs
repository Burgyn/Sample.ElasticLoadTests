using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System;

namespace Sample.ElasticLoadTests
{
    internal static class ElasticClient
    {
        public static ElasticsearchClient Create(
            string uri,
            string username,
            string password,
            string indexName)
        {
            ElasticsearchClientSettings settings = new(new Uri(uri));

            settings.Proxy(new Uri("http://192.168.1.3:3128/"), null, null);

            settings.Authentication(new BasicAuthentication(username, password))
                .DefaultMappingFor<Order>(orderMapping =>
                {
                    orderMapping.IndexName(indexName);
                    orderMapping.IdProperty(invoice => invoice.Id);
                });

            settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

            return new ElasticsearchClient(settings);
        }
    }
}
