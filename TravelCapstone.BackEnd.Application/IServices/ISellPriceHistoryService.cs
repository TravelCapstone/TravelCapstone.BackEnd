using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface ISellPriceHistoryService
    {
        Task<AppActionResult> GetHotelPriceByRating(Guid ratingId, int pageIndex, int pageSize);
        Task<AppActionResult> GetAllEntertainmentPriceByFacilityId(Guid facilityId, int pageIndex, int pageSize);
    }
}
