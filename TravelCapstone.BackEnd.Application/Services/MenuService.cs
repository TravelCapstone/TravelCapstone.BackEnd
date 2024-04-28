using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class MenuService : GenericBackendService, IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IUnitOfWork _unitOfWork;   
        public MenuService(IServiceProvider serviceProvider, IRepository<Menu> menuRepository,  
            IUnitOfWork unitOfWork) : base(serviceProvider)
        {
            _menuRepository = menuRepository;   
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetMenuByFacilityId(Guid id, int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var facilityRepository = Resolve<IRepository<Facility>>();
                var facility = await facilityRepository!.GetById(id);
                if (facility == null)
                {
                    result = BuildAppActionResultError(result, $"Nơi cung cấp dịch vụ với ${id} này không tồn tại");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    var menuDb = await _menuRepository!.GetAllDataByExpression(m => m.FacilityService.FacilityId == facility.Id, pageIndex, pageSize, null, false, m => m.FacilityService!.Facility);
                    if (menuDb.Items != null && menuDb.Items.Count > 0)
                    {
                        List<MenuResponse> menuResponses = new List<MenuResponse>();
                        var menuDishRepository = Resolve<IRepository<MenuDish>>();
                        foreach (var item in menuDb.Items)
                        {
                            MenuResponse menu = new MenuResponse();
                            menu.Menu = item;
                            var menuDishDb = await menuDishRepository!.GetAllDataByExpression(m => m.MenuId == item.Id, 0, 0, null, false, m => m.Dish!);
                            menu.Dishes = menuDishDb.Items!.Select(m => m.Dish).ToList()!;
                            menuResponses.Add(menu);
                        }
                        result.Result = new PagedResult<MenuResponse>
                        {
                            Items = menuResponses
                        };
                    }
                }
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllMenu(int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var menuDb = await _menuRepository!.GetAllDataByExpression(null, pageIndex, pageSize, null, false, m => m.FacilityService!.Facility);
                if(menuDb.Items != null && menuDb.Items.Count > 0)
                {
                    List<MenuResponse> menuResponses = new List<MenuResponse>();
                    var menuDishRepository = Resolve<IRepository<MenuDish>>();
                    foreach(var item in menuDb.Items)
                    {
                        MenuResponse menu = new MenuResponse();
                        menu.Menu = item;
                        var menuDishDb = await menuDishRepository!.GetAllDataByExpression(m => m.MenuId == item.Id, 0, 0, null, false, m => m.Dish!);
                        menu.Dishes = menuDishDb.Items!.Select(m => m.Dish).ToList()!;
                        menuResponses.Add(menu);
                    }
                    result.Result = new PagedResult<MenuResponse>
                    {
                        Items = menuResponses
                    };
                }

            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }
    }
}
