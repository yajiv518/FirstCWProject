using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entities;
using DAL;
using Cache;
namespace UsedCarUI.Controllers
{
    public class UsedcarController : Controller
    {
        //
        // GET: /Usedcar/

        CacheLayer usedCars = new CacheLayer();

        [Route("api/Usedcar/")]
        [System.Web.Mvc.HttpPost]
        public ActionResult POST( [FromBody] Stocks stocks)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                int stockId = usedCars.createStock(stocks);
                return View(stockId);
            }
            catch
            {
                return View();
            }

        }

        [Route("api/Usedcar/{id}{stocks}")]
        [System.Web.Mvc.HttpPut]
        public ActionResult PUT (int id, [FromBody] Stocks stocks)
        {
            try
            {
                if (!ModelState.IsValid && id<0)
                {
                    return View();
                }
                int stockId = usedCars.updateStock(id, stocks);
                return View(stockId);
            }
            catch
            {
                return View();
            }
        }

        [Route("api/Usedcar/{id}")]
        [System.Web.Mvc.HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                int stockId = usedCars.deleteStock(id);
                return View(stockId);
            }
            catch
            {
                return View();
            }
        }

        [Route("api/Usedcar/{id}/CarDetail")]
        [System.Web.Mvc.HttpGet]
        public ActionResult GET(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                CacheLayer carDetail= new CacheLayer();
                ReadStock stockDetail = carDetail.GetAll(id);
                return View(stockDetail);
            }
            catch
            {
                return View();
            }
        }
          
        public ActionResult CarsForSale()
        {
            return View();
        }

        

    }
}
