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
        public int GetAllStockCount()
        {
            var searchResult = client.Count<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .Query(p=>p.MatchAll()));
            int count = Convert.ToInt32(searchResult.Count);
            return count;
        }
        public IEnumerable<ESGetDetail> GetAllStock(int page,int pageSize)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .From(page)
                                                        .Size(pageSize)
                                                        .MatchAll());
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }


        public int GetCityBasedStockCount(string cityName)
        {
            var searchResult = client.Count<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .Query(q => q
                                                            .Term(p => p.City, cityName)
                                                            ));
            int count = Convert.ToInt32(searchResult.Count);
            return count;
        }
        public IEnumerable<ESGetDetail> GetStockByCity(string cityName,int page,int pageSize)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .From(page)
                                                        .Size(pageSize)
                                                        .Query(q=>q
                                                            .Term(p=>p.City,cityName)
                                                            )
                                                        );
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }


        public int GetBudgetBasedStockCount(int minValue, int maxValue)
        {
            var searchResult = client.Count<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .Query(q => q
                                                            .Range(p => p
                                                                .OnField("price")
                                                                .LowerOrEquals(maxValue)
                                                                .GreaterOrEquals(minValue)
                                                                )
                                                            ));
            int count = Convert.ToInt32(searchResult.Count);
            return count;
        }
        public IEnumerable<ESGetDetail> GetStockByBudget(int minValue, int maxValue, int page, int pageSize)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .From(page)
                                                        .Size(pageSize)
                                                        .Query(q => q
                                                            .Range(p=>p
                                                                .OnField("price")
                                                                .LowerOrEquals(maxValue)
                                                                .GreaterOrEquals(minValue)
                                                                )
                                                            ));
            IEnumerable<ESGetDetail> getAllStock = searchResult.Documents.ToArray<ESGetDetail>();
            return getAllStock;
        }

        public int GetStockCountByCityAndPrice(string cityName, int minValue, int maxValue)
        {
            var searchResult = client.Count<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .Query(q => q
                                                            .Range(p => p
                                                                .OnField("price")
                                                                .LowerOrEquals(maxValue)
                                                                .GreaterOrEquals(minValue)
                                                                )
                                                                && q.Term(p => p.City, cityName)
                                                            ));
            int count = Convert.ToInt32(searchResult.Count);
            return count;
        }
        public IEnumerable<ESGetDetail> GetStockByCityAndPrice(string cityName, int minValue, int maxValue, int page, int pageSize)
        {
            var searchResult = client.Search<ESGetDetail>(s => s
                                                        .Index("stockdata_g3_1")
                                                        .Type("esgetdetail")
                                                        .From(page)
                                                        .Size(pageSize)
                                                        .Query(q => q
                                                            .Range(p=>p
                                                                .OnField("price")
                                                                .LowerOrEquals(maxValue)
                                                                .GreaterOrEquals(minValue)
                                                                )
                                                                && q.Term(p => p.City, cityName)
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
