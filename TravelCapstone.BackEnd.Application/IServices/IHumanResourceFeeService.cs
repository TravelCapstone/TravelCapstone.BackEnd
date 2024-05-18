using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IHumanResourceFeeService
    {
        public Task<AppActionResult> GetSalary(List<HumanResourceCost> dtos, bool IsForTourguide);
    }
}
