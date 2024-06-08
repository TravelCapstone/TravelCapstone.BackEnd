using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IFacilityService
    {
        public Task<AppActionResult> GetFacilityByProvinceId(FilterLocation filter, int pageNumber, int pageSize);
        public Task<AppActionResult> GetAllFacility( int pageNumber, int pageSize);
        public Task<AppActionResult> GetAllFacilityByRatingId( FilterLocation filter,TravelCapstone.BackEnd.Domain.Enum.Rating ratingId,int pageNumber, int pageSize);

        public Task<AppActionResult> GetFacilityAndPortInformation(FacilityAndPortRequest dto);
    }
}
