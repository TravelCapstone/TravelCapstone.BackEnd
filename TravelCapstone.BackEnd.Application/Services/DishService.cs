using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class DishService : GenericBackendService, IDishService
    {
        private readonly IRepository<Dish> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DishService(IServiceProvider serviceProvider,
            IRepository<Dish> repository
        ) : base(serviceProvider)
        {
            _repository = repository;
        }
        public async Task<AppActionResult> GetDishListByMenuId(Guid menuId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var menuDishRepository = Resolve<IRepository<MenuDish>>();
                var menuDishDb = await menuDishRepository!.GetAllDataByExpression(m => m.MenuId == menuId, 0, 0, null, false, m => m.Dish);
                if(menuDishDb.Items != null && menuDishDb.Items.Count > 0) {
                    var data = new PagedResult<Dish>();
                    data.Items = menuDishDb.Items.DistinctBy(m => m.DishId).Select(m => m.Dish).ToList()!;
                    result.Result = data;
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
