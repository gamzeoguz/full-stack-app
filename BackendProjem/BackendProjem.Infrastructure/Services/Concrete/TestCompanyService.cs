using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendProjem.Caching.Core;
using BackendProjem.Domain;
using BackendProjem.Domain.Entities;
using BackendProjem.Domain.Model;
using Elasticsearch.Net;
using Nest;


namespace BackendProjem.Infrastructure.Services
{
    public class TestCompanyService: ITestCompanyService
    {
        private readonly ITestCompanyRepository _repository;
        private static ElasticClient _client;

        public TestCompanyService(ITestCompanyRepository repository)
        {
            this._repository = repository;
            _client = new ElasticClient(PrepareConnectionSettings());

        }

        private ConnectionSettings PrepareConnectionSettings()
        {
            return new ConnectionSettings(new Uri("http://localhost:9200/"))
                .EnableHttpCompression()
                .DisableDirectStreaming()
                .DefaultDisableIdInference(false)
                .DefaultMappingFor<CompanyMappingType>(m => m.IndexName("dev_company_20230724113530"));
        }
        public async Task<List<CompanyMappingType>> GetCompany()
        {
            try
            {
                //x => x.Name == name
                var response = await this._repository.FindList().ConfigureAwait(false);
                return response.Select(x => new CompanyMappingType
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    IsActive = x.IsActive
                }).ToList(); 
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<TestCompany>> GetCompanyFromElasticSearch(String companyName)
        {
            try
            {
                var response = await _client.SearchAsync<TestCompany>(s => s
                         .Index(Indices.Index("dev_company_20230724113530"))
                         .Query(q =>
                           (
                            (
                              q.Term("_index", "dev_company_20230724113530") &&
                              q.Match(m => m.Field("name").Query(companyName)) &&
                              q.Bool(b => b
                                 .Must(
                                    m => m.Term(x => x.IsActive, "true")))
                                                                                    
                                 )
                                 ))
                                .Sort(sort => sort.Descending(SortSpecialField.Score))
                                .Size(10));
                return response.Documents.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> RemoveTestCompany(int id)
        {
            try
            {
                var response = await _client.DeleteByQueryAsync<TestCompany>(s => s
                         .Index(Indices.Index("dev_company_20230724152856"))
                         .Query(q =>
                           (
                            (
                              q.Term("_index", "dev_company_20230724152856") &&
                              q.Term(m => m.Id, id)
                             )
                                 )));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateTestCompany(TestCompanyUpdateRequestModel testCompanyUpdateRequestModel)
        {
            try
            {/*
                var document = new CompanyMappingType
                {
                    Id = testCompanyUpdateRequestModel.Id.ToString(),
                    IsActive = testCompanyUpdateRequestModel.IsActive,
                    Name = testCompanyUpdateRequestModel.Name
                };



                var response = _client.Update<CompanyMappingType>(document.Id, x =>
                    x.Index("dev_company_20230724152856")
                        .Doc(document)
                        .Refresh(Refresh.True)
                        .Routing(document.Id));*/
               // var response = await _client.IndexAsync(new TestCompany { Id = testCompanyUpdateRequestModel.Id, Name = testCompanyUpdateRequestModel.Name, IsActive = testCompanyUpdateRequestModel.IsActive }, i => i.Index("dev_company_20230724113530").Routing(testCompanyUpdateRequestModel.Id).Refresh(Elasticsearch.Net.Refresh.False));
                var response = await _client.UpdateByQueryAsync<TestCompany>(s => s
                         .Index(Indices.Index("dev_company_20230724152856"))
                         .Query(q =>
                           (
                            (
                              q.Term("_index", "dev_company_20230724152856") &&
                              q.Term(m => m.Id, testCompanyUpdateRequestModel.Id) &&
                              q.Term(m => m.Name, testCompanyUpdateRequestModel.Name) &&
                              q.Term(m => m.IsActive, testCompanyUpdateRequestModel.IsActive)
                              
                             )
                            )
                           )
                         .Script("ctx._source.flag = 'foo'")
                         .Conflicts(Conflicts.Proceed)
                         .Refresh(true)
                         );
                if (response == null) { return false; }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
