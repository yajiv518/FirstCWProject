using Carwale.DAL.CoreDAL;
using Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch
{
    public class ElasticSearchClient
    {
        ElasticClient client = ElasticClientInstance.GetInstance();
        public IEnumerable<ReadStock> GetAllStock()
        {
            var searchResult = client.Search<ReadStock>(s => s
                                                        .Index("stockdata_g3")
                                                        .Type("esgetdetail")
                                                        .MatchAll());
            IEnumerable<ReadStock> getAllStock=searchResult.Documents.ToArray<ReadStock>();
            return getAllStock;
        }
        public IEnumerable<ReadStock> GetStockByCity(string cityName)
        {
            var searchResult = client.Search<ReadStock>(s => s
                                                        .Index("stockdata_g3")
                                                        .Type("esgetdetail")
                                                        .Query(q=>q
                                                            .Term(p=>p.City,cityName)
                                                            )
                                                        );
            IEnumerable<ReadStock> getAllStock = searchResult.Documents.ToArray<ReadStock>();
            return getAllStock;
        }
        public IEnumerable<ReadStock> GetStockByBudget(int minValue,int maxValue)
        {
            var searchResult = client.Search<ReadStock>(s => s
                                                        .Index("stockdata_g3")
                                                        .Type("esgetdetail")
                                                        .From(1)
                                                        .Size(1)
                                                        .Query(q => q
                                                            .Range(p=>p
                                                                .LowerOrEquals(maxValue)
                                                                .GreaterOrEquals(minValue)
                                                                )
                                                            ));
            IEnumerable<ReadStock> getAllStock = searchResult.Documents.ToArray<ReadStock>();
            return getAllStock;
        }
    }
}
