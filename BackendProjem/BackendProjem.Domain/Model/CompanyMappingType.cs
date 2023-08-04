using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace BackendProjem.Domain.Model
{
    [ElasticsearchType(RelationName = "company")]
    public class CompanyMappingType
    {
        [Keyword] public string Id { get; set; }
        [Keyword] public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
