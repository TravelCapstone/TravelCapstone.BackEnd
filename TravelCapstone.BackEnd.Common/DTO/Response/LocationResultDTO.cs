using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class LocationResultDTO
    {
        public class GeocodeResponse
        {
            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("result")]
            public LocationResult Result { get; set; }
        }

        public class LocationResult
        {
            [JsonProperty("place_id")]
            public string PlaceId { get; set; }

            [JsonProperty("formatted_address")]
            public string FormattedAddress { get; set; }

            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Geometry
        {
            [JsonProperty("location")]
            public Location Coordinates { get; set; }
        }

        public class Location
        {
            [JsonProperty("lat")]
            public double Latitude { get; set; }

            [JsonProperty("lng")]
            public double Longitude { get; set; }
        }
    }
}
