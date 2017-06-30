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
        public int updateStock(int id, Stocks stock)
        {
            using (MemcachedClient client = new MemcachedClient("memcached"))
            {
                string key = createkey(id); 
               
                int tag = UpdateDetails.Edit(id, stock);
                //ElasticSearchClient update = new ElasticSearchClient();
                //update.UpdateStock(id, stock);
                client.Store(StoreMode.Set, key, stock, DateTime.Now.AddMinutes(15));
                return tag;

            }
        }
            public int createStock(Stocks stock)
        {
            int _tag = 0;
            try
            {
                using (MemcachedClient client = new MemcachedClient("memcached"))
                {

                    
                    _tag = UpdateDetails.Create(stock);
                    string key = createkey(_tag);
                    client.Store(StoreMode.Set, key, stock, DateTime.Now.AddMinutes(15));
                    

                }
            }
            catch (Exception)
            {
                
                throw;
            }
            return _tag;
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