using Cache;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace Services.Controllers
{
    public class ServicesController : ApiController
    {
        CacheLayer usedCars = new CacheLayer();

        
        [HttpPost,Route("api/Usedcar")]
        public IHttpActionResult POST([FromBody] Stocks stocks)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Bad Input");
                }
                int stockId = usedCars.createStock(stocks);
                string originalPath = string.Format("S:/FirstCWProject/UsedCars/Services/CarImages/{0}/", stockId);
                Directory.CreateDirectory(Path.GetDirectoryName(originalPath));
                return Ok(stockId);
            }
            catch
            {
                return InternalServerError();
            }

        }

        [Route("api/Usedcar/{id}")]
        [HttpPut]
        public IHttpActionResult PUT(int id, [FromBody] Stocks stocks)
        {
            try
            {
                if (!ModelState.IsValid && id < 0)
                {
                    return BadRequest("Bad Input");
                }
                usedCars.updateStock(id, stocks);
                return Ok(id);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/Usedcar/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Bad Input");
                }
                int stockId = usedCars.deleteStock(id);
                return Ok(stockId);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/Usedcar/{id}/CarDetail")]
        [HttpGet]
        public IHttpActionResult GET(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Bad Input");
                }
                CacheLayer carDetail = new CacheLayer();
                ReadStock stockDetail = carDetail.GetAll(id);
                return Ok(stockDetail);
            }
            catch
            {
                return InternalServerError();
            }
        }


        [Route("api/{id}")]
        [HttpPost]
        public IHttpActionResult generateImage([FromUri]int id, [FromBody]Display dObj)
        {
            try
            {
                if (id != -1 && dObj.imgUrl != null)
                {
                    Produce sendobj = new Produce();
                    sendobj.sender(id, dObj.imgUrl);
                }
                return Ok("Your image is uploaded successfully. :) ");

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
