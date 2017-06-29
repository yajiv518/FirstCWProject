using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using DAL;
using Entities;

namespace Cache
{
    public class CacheLayer
    {

        
        public ReadStock GetAll(int carId)
        {
            using (MemcachedClient client = new MemcachedClient("memcached"))
            {
                string key = string.Format("UsedCar_{0}",carId);
                ReadStock carDetails = (ReadStock)client.Get(key);
                if (carDetails != null)
                    return carDetails;
                else
                {
                    DataAccessLayer GetDetails = new DataAccessLayer();
                    ReadStock stockDetail = GetDetails.Read(carId);
                     client.Store(StoreMode.Set, key, stockDetail,DateTime.Now.AddMinutes(15));
                    return stockDetail;

                }
            }
        }
    }
}