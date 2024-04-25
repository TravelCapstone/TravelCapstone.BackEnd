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
    public class FacilityService : GenericBackendService, IFacilityService
    {
        private IRepository<Facility> _repository;
        private IUnitOfWork _unitOfWork;
        public FacilityService(IRepository<Facility> repository, IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetAllFacilityByPronvinceId(Guid provinceId, ServiceType serviceType)
        {
            AppActionResult result = new();
            var facilityServiceRepository = Resolve<IRepository<TravelCapstone.BackEnd.Domain.Models.FacilityService>>();
            try
            {
                var service = await facilityServiceRepository!.GetAllDataByExpression(a => a.ServiceTypeId == serviceType
                && a.Facility!.Communce!.District!.ProvinceId == provinceId, 0, 0, null, false, a=> a.Facility!);
                var ids = service.Items!.Select(a => a.Facility!.Id).ToList();
                result.Result = await _repository.GetAllDataByExpression(
                    a => ids.Contains(a.Id), 0, 0, null, false, a => a.FacilityRating!.FacilityType!, a => a.FacilityRating!.Rating!, a=> a.Communce!.District!.Province!);

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");

            }
            return result;

        }
    }
}
