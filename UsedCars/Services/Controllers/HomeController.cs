using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entities;
using Cache;
using ElasticSearch;
using DTOs;
using System.IO;

namespace Services.Controllers
{
    public class HomeController : Controller
    {
        ElasticSearchClient getAllCars = new ElasticSearchClient();
        SearchResultDTO resultsUsedCar = new SearchResultDTO();

        public ActionResult Index()
        {
            //SyncESwithDatabase s = new SyncESwithDatabase();
            //s.SyncIndex();
            resultsUsedCar.ResultList = getAllCars.GetAllStocks(0, resultsUsedCar.PageSize + 1);
            if (resultsUsedCar.ResultList.Count() > resultsUsedCar.PageSize)
                resultsUsedCar.NextPageId = 1;
            return View("~/Views/Usedcar/CarsForSale.cshtml", resultsUsedCar);
        }


        [Route("Home/Filter")]
        [System.Web.Mvc.HttpGet]
        public ActionResult Filter(string city = null, string page = null, string minPrice = null, string maxPrice = null)
        {
            try
            {
                int pageNo = GetPageNo(page);
                if (city != "select")
                {
                    if ( minPrice != "")
                    {
                        resultsUsedCar.ResultList = getAllCars.GetStocksByCityAndPrice(city, minPrice, maxPrice, pageNo * resultsUsedCar.PageSize, resultsUsedCar.PageSize + 1);
                    }
                    else
                    {
                        resultsUsedCar.ResultList = getAllCars.GetStocksByCity(city, pageNo * resultsUsedCar.PageSize, resultsUsedCar.PageSize + 1);
                    }
                }
                else if (minPrice != "")
                {
                    resultsUsedCar.ResultList = getAllCars.GetStocksByBudget(minPrice, maxPrice, pageNo * resultsUsedCar.PageSize, resultsUsedCar.PageSize + 1);
                }
                else
                {
                    resultsUsedCar.ResultList = getAllCars.GetAllStocks(pageNo * resultsUsedCar.PageSize, resultsUsedCar.PageSize + 1);
                }
                return View("~/Views/Shared/UsedCar.cshtml", resultsUsedCar);
            }
            catch
            {
                return View();
            }
        }

        [Route("Home/stock/{stockId}/")]
        [System.Web.Mvc.HttpGet]
        public ActionResult GetCarDetail(int stockId)
        {
            try
            {
                ProfilePage displayStock = new ProfilePage();
                CacheLayer _cacheLayer = new CacheLayer();
                displayStock.CarProfile = _cacheLayer.GetStock(stockId);
                string directoryName = string.Format("S:/FirstCWProject/UsedCars/Services/CarImages/{0}/", stockId);
                displayStock.ImageCount = (Directory.GetFiles(directoryName).Length) / 3;
                return View("~/Views/Usedcar/GET.cshtml", displayStock);
            }
            catch
            {
                return View();
            }
        }

        private int GetPageNo(string page)
        {
            int pageNo = 0;
            if (page != null)
            {
                pageNo = Convert.ToInt32(page);
                resultsUsedCar.PreviousPageId = pageNo - 1;
                resultsUsedCar.NextPageId = pageNo + 1;
            }
            else
            {
                resultsUsedCar.NextPageId = 1;
                resultsUsedCar.PreviousPageId = -1;
            }
            return pageNo;
        }
    }
}