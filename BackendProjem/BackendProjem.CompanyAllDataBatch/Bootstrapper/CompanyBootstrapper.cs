using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Domain.Model;
using BackendProjem.Infrastructure;
using BackendProjem.Infrastructure.Services;
using Nest;

namespace BackendProjem.CompanyAllDataBatch
{
    public class CompanyBootstrapper
    {
        private static int _numberOfReplicas;
        private static int _numberOfShards;
        private static int _refreshInterval = -1;
        private static string _indexAliasName;
        private static string _indexName;
        private static ElasticClient _client;

        private static readonly ITestCompanyService _testCompanyService = IocContainer.Resolve<ITestCompanyService>();

        public static async Task Run()
        {
            _numberOfReplicas = 1;
            _numberOfShards = 1;
            _refreshInterval = 5000;
            _indexAliasName = "dev_company";
            _indexName = $"dev_company_{DateTime.Now:yyyyMMddHHmmss}";
            _client = new ElasticClient(PrepareConnectionSettings());

            await CreateIndexAndMappings();

            await ImportData();

            await UpdateIndexSettings();

            await SwapAliases();
        }

        public static async Task CreateIndexAndMappings()
        {
            if (_client.Indices.Exists(_indexName).Exists)
            {
                return;
            }


            var createIndexResult = _client.Indices.Create(_indexName,
                    index => index.Map<CompanyMappingType>(
                        x => x.AutoMap()
                    ));

            if (!createIndexResult.IsValid || !createIndexResult.Acknowledged)
            {
                throw new Exception("Error on mapping!");
            }

            Console.WriteLine("CreateIndexAndMappings");
        }

        private static ConnectionSettings PrepareConnectionSettings()
        {
            return new ConnectionSettings(new Uri("http://localhost:9200/"))
                .EnableHttpCompression()
                .DisableDirectStreaming()
                .DefaultDisableIdInference(false)
                .DefaultMappingFor<CompanyMappingType>(m => m.IndexName(_indexName));
        }


        private static async Task ImportData()
        {
            Console.WriteLine($"ImportData Company List Started! {DateTime.Now}");

            var companyList = await _testCompanyService.GetCompany().ConfigureAwait(false);

            if(companyList == null || !companyList.Any())
            {
                return;
            }
            // Repodan veri alınacak.

            var response = await _client.BulkAsync(b => b.IndexMany(companyList, (bulkDescriptor, item) =>
                        bulkDescriptor.Document(item).Routing(item.Id).Index(_indexName))
                    .Refresh(Elasticsearch.Net.Refresh.False));

            if (!response.IsValid)
            {
                Console.WriteLine($"Company elastic import error: {response?.ServerError?.Error}");
            }

            Console.WriteLine($"ImportData Company List Completed! {DateTime.Now}");
        }

        private static async Task UpdateIndexSettings()
        {
            await _client.Indices.UpdateSettingsAsync(_indexName,
                descriptor => descriptor.IndexSettings(k => k.RefreshInterval(new Time(_refreshInterval))));
        }

        private static async Task SwapAliases()
        {
            var result = await _client.GetIndicesPointingToAliasAsync(_indexAliasName);

            var indexes = result.Where(x => x != _indexName);

            await _client.Indices.PutAliasAsync(_indexName, _indexAliasName);

            var indexesForAlias = await _client.GetIndicesPointingToAliasAsync(_indexAliasName);

            foreach (var index in indexes)
            {
                if (index == _indexName)
                {
                    continue;
                }
                await _client.Indices.DeleteAliasAsync(index, _indexAliasName);
            }
        }
    }
}
