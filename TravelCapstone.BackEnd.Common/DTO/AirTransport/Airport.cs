using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.AirTransport
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DataAirport
    {
        [JsonProperty("AirportCode")]
        public string AirportCode { get; set; }

        [JsonProperty("AirportName")]
        public string AirportName { get; set; }

        [JsonProperty("CityCode")]
        public string CityCode { get; set; }

        [JsonProperty("CityName")]
        public string CityName { get; set; }

        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("CountryName")]
        public string CountryName { get; set; }
    }

    public class Airport
    {
        [JsonProperty("Datas")]
        public List<DataAirport> Datas { get; set; }

        [JsonProperty("StatusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("StatusDes")]
        public string StatusDes { get; set; }
    }


}
