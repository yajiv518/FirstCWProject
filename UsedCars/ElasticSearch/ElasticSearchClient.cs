﻿using Carwale.DAL.CoreDAL;
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
        private ElasticClient _client = ElasticClientInstance.GetInstance();
        private QueryContainer _queryStore;
        //private MatchAllQuery _allStocks = new MatchAllQuery();
        private TermQuery _stocksByCity = new TermQuery()
        {
            Field = "city"
        };
        private RangeQuery _stocksByBudget = new RangeQuery()
        {
            Field = "price"
        };

        private SearchRequest MakeSearchRequest(int page, int pageSize, QueryContainer query)
        {
            return new SearchRequest
            {
                From = page,
                Size = pageSize,
                Query = query
            };
        }

        public IEnumerable<ESGetDetail> GetAllStocks(int page, int pageSize)
        {
            _queryStore = new MatchAllQuery();
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByCity(string cityName, int page, int pageSize)
        {
            _stocksByCity.Value = cityName;
            _queryStore = _stocksByCity;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByBudget(string minValue, string maxValue, int page, int pageSize)
        {
            _stocksByBudget.GreaterThanOrEqualTo = (minValue);
            _stocksByBudget.LowerThanOrEqualTo = (maxValue);
            _queryStore = _stocksByBudget;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }

        public IEnumerable<ESGetDetail> GetStocksByCityAndPrice(string cityName, string minValue, string maxValue, int page, int pageSize)
        {
            _stocksByCity.Value = cityName;
            _stocksByBudget.GreaterThanOrEqualTo = minValue;
            _stocksByBudget.LowerThanOrEqualTo = maxValue;
            _queryStore = _stocksByCity && _stocksByBudget;
            var searchRequest = MakeSearchRequest(page, pageSize, _queryStore);
            var searchResults = _client.Search<ESGetDetail>(searchRequest);
            return searchResults.Documents.ToArray<ESGetDetail>();
        }


        public void CreateESStock(ESGetDetail createdStock)
        {
            _client.Index(createdStock, i => i
                .Index("stockdata_g3_1")
                .Type("esgetdetail")
                .Id(createdStock.ID.ToString())
                     );
        }

        public void UpdateESStock(int stockId, ESGetDetail updatedStock)
        {
            _client.Update<ESGetDetail, object>(u => u
                .Id(stockId)
                .Index("stockdata_g3_1")
                .Type("esgetdetail")
                .Doc(new
                {
                    Price = updatedStock.Price,
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
        public void DeleteESStock(int stockId)
        {
            _client.DeleteByQuery<ESGetDetail>(q => q
            .AllIndices()
            .Query(rq => rq
                .Term(f => f.ID, stockId)
               )
           );
        }
    }
}
