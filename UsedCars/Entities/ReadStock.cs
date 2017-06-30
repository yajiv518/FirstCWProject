using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Entities
{
    [Serializable, DataContract]
    public class ReadStock
    {
        
        [JsonProperty("price")]
        public int Price { get; set; }
       [JsonProperty("year")]
        public int Year { get; set; }
       [JsonProperty("kilometers")]
        public int Kilometers { get; set; }
        [JsonProperty("fueltype")]
        public string FuelType { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("fueleconomy")]
        public float FuelEconomy { get; set; }

         [JsonProperty("carcompany")]
        public string carcompany { get; set; }
         [JsonProperty("modelname")]
        public string modelname { get; set; }
         [JsonProperty("carversionname")]
        public string carversionname { get; set; }
    }
}