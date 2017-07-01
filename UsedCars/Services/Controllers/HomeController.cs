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
using PagedList;
using PagedList.Mvc;
using DTOs;
using System.IO;

namespace Services.Controllers
{
    public class HomeController : Controller
    {
        //IEnumerable<ESGetDetail> getAllStock;
        //int totalStockNumber;
        ElasticSearchClient getAllCars = new ElasticSearchClient();
        SearchResultDTO results = new SearchResultDTO();

        public ActionResult Index(int? page)
        {
            //SyncESwithDatabase s = new SyncESwithDatabase();
            //s.SyncIndex();
               
               //int totalStockNumber = getAllCars.GetAllStockCount();
            IEnumerable<ESGetDetail> getAllStocks = getAllCars.GetAllStock(0, results.PageSize+1);
               
               results.ResultList = getAllStocks;
               //results.totalStock = totalStockNumber;
               if (getAllStocks.Count() > results.PageSize)
               results.NextPageId = 1;
               results.PreviousPageId = -1;
               return View("~/Views/Usedcar/CarsForSale.cshtml",results);
           
            //return View(getAllStocks);
        }

        

        [Route("Home/FilterByCity")]
        [System.Web.Mvc.HttpGet]
        public ActionResult FilterByCity(string city=null,string page=null,string min=null,string max=null)
        {
            try
            {
                IEnumerable<ESGetDetail> getAllStocks ;
                int pageNo = 0;
                if(page!=null)
                {
                    pageNo = Convert.ToInt32(page);
                    results.PreviousPageId = pageNo - 1;
                    results.NextPageId = pageNo + 1;
                }
                else 
                {
                    pageNo = 0;
                    results.NextPageId = 1;
                    results.PreviousPageId = -1;
                }
                if (city != "select" && city!=null)
                {
                    if (min != null && min!="")
                    {
                        getAllStocks = getAllCars.GetStockByCityAndPrice(city, min, max, pageNo * results.PageSize, results.PageSize+1);
                        results.ResultList = getAllStocks;
                    }
                    else
                    {
                        getAllStocks = getAllCars.GetStockByCity(city, pageNo * results.PageSize, results.PageSize+1);
                        results.ResultList = getAllStocks;
                    }
                    return View("~/Views/Shared/UsedCar.cshtml", results);
                }
                else if(min!=null && min!="")
                {
                    getAllStocks = getAllCars.GetStockByBudget(min, max, pageNo * results.PageSize, results.PageSize+1);
                    results.ResultList = getAllStocks;
                    return View("~/Views/Shared/UsedCar.cshtml", results);
                }
                else 
                {
                    getAllStocks = getAllCars.GetAllStock(pageNo * results.PageSize, results.PageSize+1);
                    results.ResultList = getAllStocks;
                    return View("~/Views/Shared/UsedCar.cshtml", results);
                }
            }
            catch
            {
                return View();
            }

        }

        [Route("Home/{id}/CarDetail")]
        [System.Web.Mvc.HttpGet]
        public ActionResult GET(int id)
        {
            try
            {
                ProfilePage detailCar = new ProfilePage();
                CacheLayer carDetail = new CacheLayer();
                ReadStock stockDetail = carDetail.GetAll(id);
                detailCar.CarProfile = stockDetail;
                string dir_name = string.Format("S:/FirstCWProject/UsedCars/Services/CarImages/{0}/", id ) ;
                detailCar.ImageCount = (Directory.GetFiles(dir_name).Length)/3;
                return View("~/Views/Usedcar/GET.cshtml", detailCar);
            }
            catch
            {
                return View();
            }
        }

        //[Route("Home/FilterByPrice/{minPrice},{maxPrice}/")]
        //[System.Web.Mvc.HttpGet]
        //public ActionResult FilterByPrice(int minPrice, int maxPrice)
        //{
        //    try
        //    {
        //        ElasticSearchClient getAllCars = new ElasticSearchClient();
        //        IEnumerable<ESGetDetail> getAllStocks = getAllCars.GetStockByBudget(minPrice, maxPrice);
        //        return View("~/Views/Shared/UsedCar.cshtml", getAllStocks);

        //    }
        //    catch
        //    {
        //        return View();
        //    }

        //}

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