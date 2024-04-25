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
                    result.Result = await _menuRepository.GetAllDataByExpression(p => p.FacilityService!.Facility == facility && p.FacilityService.ServiceTypeId == ServiceType.FOODANDBEVARAGE, pageIndex, pageSize, null, false, p => p.FacilityService!.Facility!);
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
