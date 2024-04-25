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
    public class SellPriceHistoryService : GenericBackendService, ISellPriceHistoryService
    {
        private readonly IRepository<SellPriceHistory> _sellPriceHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SellPriceHistoryService(IServiceProvider serviceProvider, IUnitOfWork unitOfWork, IRepository<SellPriceHistory> sellPriceHistoryRepository) : base(serviceProvider)
        {
            _sellPriceHistoryRepository = sellPriceHistoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetHotelPriceByRating(Guid ratingId, int pageIndex, int pageSize)
            {
            var result = new AppActionResult();
            var facilityRatingRepository = Resolve<IRepository<FacilityRating>>();
            try
            {
                var facilityRating = await facilityRatingRepository!.GetById(ratingId);
                if (facilityRating == null)
                {
                    result = BuildAppActionResultError(result, $"Loại dịch vụ với ${ratingId} này không tồn tại");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    result.Result = await _sellPriceHistoryRepository.GetAllDataByExpression(p => p.FacilityService!.Facility!.FacilityRating!.FacilityTypeId == FacilityType.HOTEL 
                    && p.FacilityService!.Facility!.FacilityRatingId == ratingId, pageIndex, pageSize, a=> a.Date, false, p => p.FacilityService!.Facility!.FacilityRating!);
                }
            } catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllEntertainmentPriceByFacilityId(Guid facilityId, int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            var facilityRepository = Resolve<IRepository<Facility>>();
            try
            {
                 result.Result = await _sellPriceHistoryRepository.GetAllDataByExpression(p => p.FacilityService!.FacilityId == facilityId && p.FacilityService.ServiceTypeId == ServiceType.ENTERTAIMENT,
                     pageIndex, pageSize, a=> a.Date, false, p => p.FacilityService!.Facility!.FacilityRating!);
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }

    }
}
