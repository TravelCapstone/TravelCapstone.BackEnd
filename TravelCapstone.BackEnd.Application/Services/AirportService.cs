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

        public async Task<AppActionResult> SearchAirFlight(List<AirlineType> airlineTypes, string starpointCode, string endPointCode, DateTime date)
        {
            AppActionResult result = new AppActionResult();
            DateTime departDateTime = new DateTime(date.Date.Year, date.Date.Month, date.Date.Day);

            // Format lại thành dạng chuỗi "dd/MM/yyyy"
            string departDate = departDateTime.ToString("ddMMyyyy");
            try
            {
                List<object> listFlight = new List<object>();
                
                foreach (var airlineType in airlineTypes)
                {
                    switch (airlineType)
                    {
                        case AirlineType.VIETNAM_AIRLINE:
                            listFlight.Add(new
                            {
                                StartPoint = "SGN",
                                EndPoint = "DLI",
                                DepartDate = departDate,
                                Airline = "VN"
                            });
                            break;
                        case AirlineType.VIETJET:
                            listFlight.Add(new
                            {
                                StartPoint = "SGN",
                                EndPoint = "DLI",
                                DepartDate = departDate,
                                Airline = "VJ"
                            });
                            break;
                        case AirlineType.BAMBOO:
                            listFlight.Add(new
                            {
                                StartPoint = "SGN",
                                EndPoint = "DLI",
                                DepartDate = departDate,
                                Airline = "QH"
                            });
                            break;
                        case AirlineType.VIET_TRAVEL_AIRLINE:
                            listFlight.Add(new
                            {
                                StartPoint = "SGN",
                                EndPoint = "DLI",
                                DepartDate = departDate,
                                Airline = "VU"
                            });
                            break;
                    }

                }

                var payload = new
                {
                    Key = "d009561aae4040b3f10a83036242f0b07a5",
                    ProductKey = "j371azf9plina2s",
                    Adt = 1,
                    Chd = 0,
                    Inf = 0,
                    ViewMode = false,
                    ListFlight = listFlight
                };
                var response = await CallApi("https://plugin.datacom.vn/flightsearch", payload, APIMethod.POST);
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
                var response = await CallApi("https://plugin.datacom.vn/searchairport", payload, APIMethod.POST);
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
