using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using DAL;

namespace Cache
{
    public class cacheLayer
    {

        MemcachedClient client = new MemcachedClient();
        public object getall(int carId)
        {
            string key = "UsedCar_" + Convert.ToString(carId);
            object carDetails = client.Get(key);
            if (carDetails != null)
                return carDetails;
            else
            {
                DataAccessLayer GetDetails = new DataAccessLayer();
                object stockDetail=GetDetails.Read(carId);
                 client.Store(StoreMode.Set, key, stockDetail,DateTime.Now.AddMinutes(15));
                return stockDetail;

            }
        }
    }
}