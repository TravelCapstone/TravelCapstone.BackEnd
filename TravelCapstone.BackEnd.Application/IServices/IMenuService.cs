using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IMenuService
    {
        Task<AppActionResult> GetMenuByFacilityId(Guid id, int pageIndex, int pageSize);
        Task<AppActionResult> GetAllMenu(int pageIndex, int pageSize);
        Task<IActionResult> GetMenuDishUpdateTemplate();
    }
}
