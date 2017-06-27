using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsedCarProject.Entities
{
    public class Stocks
    {

        
        public int StockId { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int Kilometers { get; set; }
        [Required]
        public int FuelTypeId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public int ColorId { get; set; }

        public int FuelEconomy { get; set; }

        [Required]
        public int MakeId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public int VersionId { get; set; }

    }
}