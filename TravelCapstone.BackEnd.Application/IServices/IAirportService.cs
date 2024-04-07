using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IAirportService
    {
        Task<AppActionResult> SearchAirport(string keyword);
        Task<AppActionResult> SearchAirFlight(List<AirlineType> airlineTypes, string starpointCode, string endPointCode, DateTime date);
    }
}
