using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IEventService
    {
        public Task<AppActionResult> GetEventListWithQuantity(int quantity);
        public Task<AppActionResult> CreateCustomEventString(CreateCustomEventStringRequest dto);
    }
}
