using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IMaterialService
    {
        public Task<AppActionResult> AddMaterialtoTour(AddMaterialRequest request);
        public Task<AppActionResult> GetMaterialByMaterialType(MaterialType type);
        public Task<AppActionResult> GetLatestMaterialListPrice(); 
        public Task<AppActionResult> GetMaterialCost(AddMaterialRequest request);

    }
}
