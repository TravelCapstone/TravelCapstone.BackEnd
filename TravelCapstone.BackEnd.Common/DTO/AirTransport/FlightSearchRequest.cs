using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.AirTransport
{
    public class ListFlight
    {

        [JsonProperty("StartPoint")]
        public string StartPoint { get; set; }

        [JsonProperty("EndPoint")]
        public string EndPoint { get; set; }

        [JsonProperty("DepartDate")]
        public string DepartDate { get; set; }

        [JsonProperty("Airline")]
        public string Airline { get; set; }
    }

    public class FlightSearchRequest{
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("ProductKey")]
        public string ProductKey { get; set; }

        [JsonProperty("Adt")]
        public int Adt { get; set; }

        [JsonProperty("Chd")]
        public int Chd { get; set; }

        [JsonProperty("Inf")]
        public int Inf { get; set; }

        [JsonProperty("ViewMode")]
        public bool ViewMode { get; set; }

        [JsonProperty("ListFlight")]
        public List<ListFlight> ListFlight { get; set; }
    }
    public class FlightSearchRequestDto
    {

        [JsonProperty("Adt")]
        public int Adt { get; set; }

        [JsonProperty("Chd")]
        public int Chd { get; set; }

        [JsonProperty("Inf")]
        public int Inf { get; set; }

        [JsonProperty("ViewMode")]
        public bool ViewMode { get; set; }

        [JsonProperty("ListFlight")]
        public List<ListFlight> ListFlight { get; set; }
    }
}
