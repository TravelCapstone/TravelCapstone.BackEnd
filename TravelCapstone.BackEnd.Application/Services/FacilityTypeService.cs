using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class FacilityTypeService : GenericBackendService, IFacilityTypeService
    {
        private IUnitOfWork _unitOfWork;
        private IRepository<FacilityType> _repository;

        public FacilityTypeService(IUnitOfWork unitOfWork, IRepository<FacilityType> repository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<AppActionResult> GetAllFacilityRatingByFacilityTypeId(TravelCapstone.BackEnd.Domain.Enum.FacilityType id)

        {
            AppActionResult result = new AppActionResult();
            var facilityRatingRepository = Resolve<IRepository<FacilityRating>>();
            try
            {
                result.Result = await facilityRatingRepository!.GetAllDataByExpression(a => a.FacilityTypeId == id, 0, 0, null, false, a => a.FacilityType!, a => a.Rating!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllFacilityType()
        {
            AppActionResult result = new AppActionResult();
            try
            {
                result.Result = await _repository!.GetAllDataByExpression(null, 0, 0, null, false, null);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
            }
            return result;
        }
    }

}
