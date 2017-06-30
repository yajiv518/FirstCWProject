using Cache;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
                int stockId = usedCars.updateStock(id, stocks);
                return Ok(stockId);
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
    }
}
