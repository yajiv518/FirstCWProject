using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using DAL;
using Entities;
using ElasticSearch;

namespace Cache
{
    public class CacheLayer
    {
        DataAccessLayer UpdateDetails = new DataAccessLayer();
        public string createkey(int carId)
        {
            return string.Format("UsedCar_{0}", carId);
        }

       
        public ReadStock GetAll(int carId)
        {
            ReadStock carDetails = null;
            try
            {
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {
                    string key = createkey(carId);
                    carDetails = (ReadStock)client.Get(key);
                    if (carDetails == null)
                    {

                        carDetails = UpdateDetails.Read(carId);
                        client.Store(StoreMode.Add, key, carDetails, DateTime.Now.AddMinutes(15));
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return carDetails;
        }
        public void updateStock(int id, Stocks stock)
        {
            using (MemcachedClient client = new MemcachedClient("memcached"))
            {
                string key = createkey(id);
                ReadStock getUpdatedData = new ReadStock();
                getUpdatedData = UpdateDetails.Edit(id, stock);
                ElasticSearchClient update = new ElasticSearchClient();
                client.Store(StoreMode.Set, key, getUpdatedData, DateTime.Now.AddMinutes(15));
                ESGetDetail getUpdatedESData = new ESGetDetail();
                getUpdatedESData = convertCachetoESData(getUpdatedData);
                update.UpdateStock(id, getUpdatedESData);
            }
        }

        private ESGetDetail convertCachetoESData(ReadStock getUpdatedData)
        {
            ESGetDetail convertedData = new ESGetDetail();
            convertedData.carcompany = getUpdatedData.carcompany;
            convertedData.ID = getUpdatedData.ID;
            convertedData.Price = getUpdatedData.Price;
            convertedData.Year = getUpdatedData.Year;
            convertedData.Kilometers = getUpdatedData.Kilometers;
            convertedData.FuelType = getUpdatedData.FuelType;
            convertedData.City = getUpdatedData.City;
            convertedData.modelname = getUpdatedData.modelname;
            convertedData.carversionname = getUpdatedData.carversionname;
            return convertedData;
        }
        public int createStock(Stocks stock)
        {
            int _newStockid = 0;
            try
            {
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {


                    ReadStock getCreatedData = new ReadStock();
                    getCreatedData = UpdateDetails.Create(stock);
                    string key = createkey(getCreatedData.ID);
                    client.Store(StoreMode.Set, key, getCreatedData, DateTime.Now.AddMinutes(15));
                    ESGetDetail getCreatedESData = new ESGetDetail();
                    getCreatedESData = convertCachetoESData(getCreatedData);
                    ElasticSearchClient create = new ElasticSearchClient();
                    create.CreateESStock(getCreatedESData);
                    _newStockid = getCreatedESData.ID;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return _newStockid;
        }
            public int deleteStock(int id)
            {
                int _tag=0;
                try
                {
                    using (MemcachedClient client = new MemcachedClient("memcached"))
                    {

                        ElasticSearchClient delete = new ElasticSearchClient();
                        delete.DeleteStock(id);
                        _tag = UpdateDetails.Delete(id);
                        string key = createkey(id);
                        client.Remove(key);
                      

                    }

                }
                catch (Exception)
                {
                    
                    throw;
                }
                return _tag;
            }
    }
}