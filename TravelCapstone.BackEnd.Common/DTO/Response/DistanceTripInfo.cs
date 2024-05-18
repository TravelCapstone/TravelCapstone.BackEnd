using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class DistanceTripInfo
    {
        public class Distance
        {
            [JsonProperty("text")]
            public string Distances { get; set; }
            [JsonProperty("value")]
            public int NumberOfDistance { get; set; }
        }

        public class Duration
        {
            [JsonProperty("text")]
            public string Durations { get; set; }
            [JsonProperty("value")]
            public int NumberOfTime { get; set; }
        }

        public class Element
        {
            [JsonProperty("distance")]
            public Distance Distance { get; set; }
            [JsonProperty("duration")]
            public Duration Duration { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
        }

        public class Row
        {
            [JsonProperty("elements")]
            public List<Element> Elements { get; set; } 
        }

        public class Root
        {
            [JsonProperty("rows")]
            public List<Row> Rows { get; set; }
        }
    }
}
