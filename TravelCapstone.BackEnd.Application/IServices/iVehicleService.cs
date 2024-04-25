using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface iVehicleService
    {
        public Task<AppActionResult> UploadQuotation(IFormFile file);
        public Task<AppActionResult> ValidateExcelFile(IFormFile file);
        public Task<IActionResult> GetPriceQuotationTemplate();
    }
}
