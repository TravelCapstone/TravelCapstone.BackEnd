using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
namespace TravelCapstone.BackEnd.Application.IServices { 
    public interface IFacilityServiceService { 
        Task<AppActionResult> GetServiceByProvinceIdAndServiceType(Guid Id, ServiceType type, int pageNumber, int pageSize); 
        Task<AppActionResult> GetServicePriceRangeByDistrictIdAndRequestId(Guid Id, Guid requestId, int pageNumber, int pageSize);
        Task<AppActionResult> GetServiceByFacilityId(Guid Id, int pageNumber, int pageSize);
        Task<AppActionResult> GetListServiceForPlan(Guid privateTourRequestId, Guid quotationDetailId, int pageNumber, int pageSize);
        Task<AppActionResult> GetAllFacilityRating();
    }
}