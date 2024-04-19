using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IServiceService
    {
        Task<AppActionResult> GetServiceByProvinceIdAndServiceType(Guid Id, ServiceType type);
        Task<AppActionResult> GetServiceByProvinceId(Guid Id,Guid requestId);
    }
}
