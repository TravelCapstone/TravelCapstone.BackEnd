using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.AirTransport
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DomesticData
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("FareDataId")]
        public int FareDataId { get; set; }

        [JsonProperty("FlightId")]
        public int FlightId { get; set; }

        [JsonProperty("Airline")]
        public string Airline { get; set; }

        [JsonProperty("AirlineType")]
        public object AirlineType { get; set; }

        [JsonProperty("AirlineOperating")]
        public string AirlineOperating { get; set; }

        [JsonProperty("FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("FlightValue")]
        public string FlightValue { get; set; }

        [JsonProperty("StartPoint")]
        public string StartPoint { get; set; }

        [JsonProperty("EndPoint")]
        public string EndPoint { get; set; }

        [JsonProperty("StartDate")]
        public string StartDate { get; set; }

        [JsonProperty("EndDate")]
        public string EndDate { get; set; }

        [JsonProperty("StartTime")]
        public string StartTime { get; set; }

        [JsonProperty("EndTime")]
        public string EndTime { get; set; }

        [JsonProperty("FareBasis")]
        public string FareBasis { get; set; }

        [JsonProperty("Adt")]
        public int Adt { get; set; }

        [JsonProperty("Chd")]
        public int Chd { get; set; }

        [JsonProperty("Inf")]
        public int Inf { get; set; }

        [JsonProperty("FareAdt")]
        public double FareAdt { get; set; }

        [JsonProperty("FareChd")]
        public double FareChd { get; set; }

        [JsonProperty("FareInf")]
        public double FareInf { get; set; }

        [JsonProperty("FareAdtFormat")]
        public string FareAdtFormat { get; set; }

        [JsonProperty("FareChdFormat")]
        public string FareChdFormat { get; set; }

        [JsonProperty("FareInfFormat")]
        public string FareInfFormat { get; set; }

        [JsonProperty("TotalFeeTaxAdt")]
        public double TotalFeeTaxAdt { get; set; }

        [JsonProperty("TotalFeeTaxChd")]
        public double TotalFeeTaxChd { get; set; }

        [JsonProperty("TotalFeeTaxInf")]
        public double TotalFeeTaxInf { get; set; }

        [JsonProperty("TotalFeeTaxAdtFormat")]
        public string TotalFeeTaxAdtFormat { get; set; }

        [JsonProperty("TotalFeeTaxChdFormat")]
        public string TotalFeeTaxChdFormat { get; set; }

        [JsonProperty("TotalFeeTaxInfFormat")]
        public string TotalFeeTaxInfFormat { get; set; }

        [JsonProperty("FareAdtFull")]
        public double FareAdtFull { get; set; }

        [JsonProperty("FareChdFull")]
        public double FareChdFull { get; set; }

        [JsonProperty("FareInfFull")]
        public double FareInfFull { get; set; }

        [JsonProperty("FareAdtFullFormat")]
        public string FareAdtFullFormat { get; set; }

        [JsonProperty("FareChdFullFormat")]
        public string FareChdFullFormat { get; set; }

        [JsonProperty("FareInfFullFormat")]
        public string FareInfFullFormat { get; set; }

        [JsonProperty("TotalFareAdt")]
        public double TotalFareAdt { get; set; }

        [JsonProperty("TotalFareChd")]
        public double TotalFareChd { get; set; }

        [JsonProperty("TotalFareInf")]
        public double TotalFareInf { get; set; }

        [JsonProperty("TotalFareChdFormat")]
        public string TotalFareChdFormat { get; set; }

        [JsonProperty("TotalFareAdtFormat")]
        public string TotalFareAdtFormat { get; set; }

        [JsonProperty("TotalFareInfFormat")]
        public string TotalFareInfFormat { get; set; }

        [JsonProperty("TotalPrice")]
        public double TotalPrice { get; set; }

        [JsonProperty("TotalPriceFormat")]
        public string TotalPriceFormat { get; set; }

        [JsonProperty("Duration")]
        public int Duration { get; set; }

        [JsonProperty("DurationFormat")]
        public string DurationFormat { get; set; }

        [JsonProperty("Leg")]
        public int Leg { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("IsSelect")]
        public bool IsSelect { get; set; }

        [JsonProperty("Session")]
        public string Session { get; set; }

        [JsonProperty("Promo")]
        public bool Promo { get; set; }

        [JsonProperty("ListSegment")]
        public List<ListSegment> ListSegment { get; set; }
    }

    public class ListAircraft
    {
        [JsonProperty("IATA")]
        public string IATA { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("Model")]
        public string Model { get; set; }
    }

    public class ListAirline
    {
        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Logo")]
        public string Logo { get; set; }

        [JsonProperty("Description")]
        public object Description { get; set; }

        [JsonProperty("AirlineType")]
        public string AirlineType { get; set; }
    }

    public class ListGeoCode
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

    public class ListSegment
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Airline")]
        public string Airline { get; set; }

        [JsonProperty("StartPoint")]
        public string StartPoint { get; set; }

        [JsonProperty("EndPoint")]
        public string EndPoint { get; set; }

        [JsonProperty("StartTime")]
        public string StartTime { get; set; }

        [JsonProperty("StartDate")]
        public string StartDate { get; set; }

        [JsonProperty("EndTime")]
        public string EndTime { get; set; }

        [JsonProperty("EndDate")]
        public string EndDate { get; set; }

        [JsonProperty("FlightNumber")]
        public string FlightNumber { get; set; }

        [JsonProperty("Duration")]
        public string Duration { get; set; }

        [JsonProperty("Cabin")]
        public string Cabin { get; set; }

        [JsonProperty("Class")]
        public string Class { get; set; }

        [JsonProperty("Plane")]
        public string Plane { get; set; }

        [JsonProperty("HandBaggage")]
        public string HandBaggage { get; set; }

        [JsonProperty("AllowanceBaggage")]
        public string AllowanceBaggage { get; set; }

        [JsonProperty("ListHiddenStop")]
        public object ListHiddenStop { get; set; }
    }

    public class FlightSearch
    {
        [JsonProperty("FlightType")]
        public string FlightType { get; set; }

        [JsonProperty("Currency")]
        public object Currency { get; set; }

        [JsonProperty("Adt")]
        public int Adt { get; set; }

        [JsonProperty("Chd")]
        public int Chd { get; set; }

        [JsonProperty("Inf")]
        public int Inf { get; set; }

        [JsonProperty("Session")]
        public object Session { get; set; }

        [JsonProperty("MinDuration")]
        public int MinDuration { get; set; }

        [JsonProperty("MinDurationFormat")]
        public string MinDurationFormat { get; set; }

        [JsonProperty("MaxDuration")]
        public int MaxDuration { get; set; }

        [JsonProperty("MaxDurationFormat")]
        public string MaxDurationFormat { get; set; }

        [JsonProperty("MinPrice")]
        public double MinPrice { get; set; }

        [JsonProperty("MinPriceFormat")]
        public string MinPriceFormat { get; set; }

        [JsonProperty("MaxPrice")]
        public double MaxPrice { get; set; }

        [JsonProperty("MaxPriceFormat")]
        public string MaxPriceFormat { get; set; }

        [JsonProperty("StopNums")]
        public List<int> StopNums { get; set; }

        [JsonProperty("ListAirline")]
        public List<ListAirline> ListAirline { get; set; }

        [JsonProperty("ListAircraft")]
        public List<ListAircraft> ListAircraft { get; set; }

        [JsonProperty("ListGeoCode")]
        public List<ListGeoCode> ListGeoCode { get; set; }

        [JsonProperty("DomesticDatas")]
        public List<DomesticData> DomesticDatas { get; set; }

        [JsonProperty("InternationalDatas")]
        public List<object> InternationalDatas { get; set; }

        [JsonProperty("StatusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("StatusDes")]
        public string StatusDes { get; set; }
    }


}
