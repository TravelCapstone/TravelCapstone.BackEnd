using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IReferenceTransportService
    {
        Task<AppActionResult> GetAllReferenceTransport(int pageIndex, int pageSize);
        Task<AppActionResult> GetAllReferenceTransportByProvinceId( FilterReferenceTransportPrice filter,int pageIndex, int pageSize);
    }
}
