using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using Interfaces;
using Cache;
using DAL;
using ElasticSearch;


namespace Services.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            /*Stocks s = new Stocks();
            s.Price = 100000;
            s.Year = 2013;
            s.Kilometers = 1000;
            s.FuelTypeId = 2;
            s.CityId = 2;
            s.ColorId = 2;
            s.MakeId = 2;
            s.VersionId = 2;
            s.FuelEconomy = 12.5;
            s.ModelId = 2;
            ReadStock r = new ReadStock();
            CacheLayer c = new CacheLayer();
            IDataAccess<Stocks> d = new DataAccessLayer();
            //int d12 = d.Create(s);
            d.Edit(1, s);
            //d.Delete(5);
            //r=c.GetAll(2);
            Console.Write(r);*/
            SyncESwithDatabase s = new SyncESwithDatabase();
            s.SyncIndex();
            //ElasticSearchClient s = new ElasticSearchClient();
            //s.GetStockByBudget(5,50000000);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}