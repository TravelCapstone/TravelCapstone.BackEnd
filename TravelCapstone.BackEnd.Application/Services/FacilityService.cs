using AutoMapper;
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
    public class FacilityService : GenericBackendService, IFacilityService
    {
        private readonly IMapper _mapper;
        private IRepository<Facility> _repository;
        private IUnitOfWork _unitOfWork;

        public FacilityService(
            IRepository<Facility> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> GetFacilityByProvinceId(Guid provinceId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var communeRepository = Resolve<IRepository<Commune>>();
                var communeDb = await communeRepository!.GetAllDataByExpression(c => c.District!.ProvinceId == provinceId, 0, 0, null, false, null);
                if(communeDb.Items != null & communeDb.Items!.Count > 0)
                {
                    var communeIds = communeDb.Items!.Select(c => c.Id);
                    var facilityDb = await _repository.GetAllDataByExpression(f => communeIds.Contains(f.CommunceId), 0, 0, null, false, a=> a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                    result.Result = facilityDb;
                }

            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
