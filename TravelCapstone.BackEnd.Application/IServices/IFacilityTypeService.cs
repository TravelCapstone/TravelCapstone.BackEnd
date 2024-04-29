using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IFacilityTypeService
    {
        Task<AppActionResult> GetAllFacilityType();
        Task<AppActionResult> GetAllFacilityRatingByFacilityTypeId(FacilityType id);
    }
}
