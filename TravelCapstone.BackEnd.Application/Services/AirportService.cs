using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.AirTransport;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Enum;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class AirportService : GenericBackendService, IAirportService
    {
        public AirportService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public string ConverFormateDateAirport(DateTime date)
        {
            DateTime departDateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day);

            // Format lại thành dạng chuỗi "dd/MM/yyyy"
            return  departDateTime.ToString("ddMMyyyy");
        }
        public async Task<AppActionResult> SearchAirFlight(FlightSearchRequestDto request)
        {
            AppActionResult result = new AppActionResult();
           
            try
            {

                FlightSearchRequest payload = new FlightSearchRequest()
                {
                    Key = "d009561aae4040b3f10a83036242f0b07a5",
                    ProductKey= "j371azf9plina2s",
                    Adt= request.Adt,
                    Chd= request.Chd,
                    Inf= request.Inf,
                    ListFlight = request.ListFlight,
                    ViewMode=false

                };
                var response = await CallAPIAsync("https://plugin.datacom.vn/flightsearch", payload, APIMethod.POST);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var data = response.Content;
                    var obj = JsonConvert.DeserializeObject<FlightSearch>(data!);
                    result.Result = obj;
                }
            }
            catch (Exception ex)
            {

                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> SearchAirport(string keyword)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var payload = new
                {
                    ProductKey = "j371azf9plina2s",
                    Lang = "vi",
                    Keyword = keyword,
                    Code = ""
                };
                var response = await CallAPIAsync("https://plugin.datacom.vn/searchairport", payload, APIMethod.POST);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var data = response.Content;
                    var obj = JsonConvert.DeserializeObject<Airport>(data!);
                    result.Result = obj;
                }
            }
            catch (Exception ex)
            {

                result = BuildAppActionResultError(result, ex.Message);
            }

            return result;
        }
    }
}
