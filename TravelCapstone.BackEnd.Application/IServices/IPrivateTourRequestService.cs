using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IPrivateTourRequestService
    {
        Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privtaeTourequestDTO);
    }
}
