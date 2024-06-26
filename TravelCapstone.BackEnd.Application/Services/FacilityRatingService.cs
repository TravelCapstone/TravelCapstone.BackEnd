using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
	public class FacilityRatingService : GenericBackendService, IFacilityRatingService
	{
		private readonly IRepository<FacilityRating> _repository;
		private readonly IUnitOfWork _unitOfWork;

		public FacilityRatingService(IServiceProvider serviceProvider,
			IRepository<FacilityRating> repository,
			IUnitOfWork unitOfWork
		) : base(serviceProvider)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
		}

		public async Task<AppActionResult> GetFacilityRatingByLocation(LocationRequestDto dto)
		{
			AppActionResult result = new AppActionResult();
			try
			{
				var facilityRepository = Resolve<IRepository<Facility>>();
				PagedResult<Facility> facilityDb = null;
				if(dto.CommuneId != null)
				{
					facilityDb = await facilityRepository.GetAllDataByExpression(f => f.CommunceId == dto.CommuneId, 0, 0, null, false, f => f.FacilityRating);
				} else if(dto.DistrictId != null)
				{
					facilityDb = await facilityRepository.GetAllDataByExpression(f => f.Communce.DistrictId == dto.DistrictId, 0, 0, null, false, f => f.FacilityRating);
				}
				else if(dto.ProvinceId != null) {
					facilityDb = await facilityRepository.GetAllDataByExpression(f => f.Communce.District.ProvinceId == dto.ProvinceId, 0, 0, null, false, f => f.FacilityRating);
				}
				else
				{
					result = BuildAppActionResultError(result, "Vui lòng kiểm tra thông các id đỉa điểm đã nhập");
					return result;
				}
				if(facilityDb.Items.Count > 0)
				{
					var data = facilityDb.Items.DistinctBy(f => f.FacilityRatingId).Select(f => f.FacilityRating.RatingId).ToList();
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
