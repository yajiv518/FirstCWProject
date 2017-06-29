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
        public IEnumerable<ESGetDetail> GetAllStock()
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3")
                                                        .Type("esgetdetail")
                                                        .MatchAll());
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }
        public IEnumerable<ESGetDetail> GetStockByCity(string cityName)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3")
                                                        .Type("esgetdetail")
                                                        .Query(q=>q
                                                            .Term(p=>p.City,cityName)
                                                            )
                                                        );
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }
        public IEnumerable<ESGetDetail> GetStockByBudget(int minValue, int maxValue)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
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
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }
        public void UpdateStock(int id, ESGetDetail updatedStock)
        {
            client.Update<ESGetDetail, object>(u => u
                .Id(id)
                .Doc(new { Price = updatedStock.Price,
                           Year = updatedStock.Year,
                           Kilometers = updatedStock.Kilometers,
                           FuelType = updatedStock.FuelType,
                           City = updatedStock.City,
                           carcompany = updatedStock.carcompany,
                           modelname = updatedStock.modelname,
                           carversionname = updatedStock.carversionname
                })
                .RetryOnConflict(3)
                .Refresh()
            );
        }
        public void DeleteStock(int id)
        {
            client.DeleteByQuery<ESGetDetail>(q => q
             .AllIndices()
             .Query(rq => rq
                 .Term(f => f.ID, id)
                )
            );
        }	
    }
}
