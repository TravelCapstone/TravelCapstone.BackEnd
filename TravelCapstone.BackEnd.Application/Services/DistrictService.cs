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
    public class DistrictService: GenericBackendService, IDistrictService
    {
        private readonly IRepository<District> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DistrictService(
            IRepository<District> repository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetAllDistrictByPrivateTourRequestId(Guid Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                List<District> data = new List<District>();
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourRequestDb = await privateTourRequestRepository!.GetByExpression(p => p.Id == Id, p => p.Tour); 
                if(privateTourRequestDb == null)
                {
                    return result;
                }
                var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
                var requestedLocationListDb = await requestedLocationRepository!.GetAllDataByExpression(null, 0, 0, null, false, r => r.Province!);
                var provinceIds = requestedLocationListDb.Items!.Select(r => r.ProvinceId).ToList();
                var districtRepository = Resolve<IRepository<District>>();
                var districtDb = await districtRepository!.GetAllDataByExpression(d => provinceIds.Contains(d.ProvinceId), 0,0, null, false, d => d.Province);
                data.InsertRange(0, districtDb.Items!);
                var dayPlanRepository = Resolve<IRepository<DayPlan>>();
                var dayPlanDb = await dayPlanRepository!.GetAllDataByExpression(r => r.TourId == privateTourRequestDb.TourId, 0, 0, null, false,null);
                if(dayPlanDb.Items!.Count == 0)
                {
                    return result;
                }
                var dayPlanIds = dayPlanDb.Items.Select(d => d.Id);
                var routeRepository = Resolve<IRepository<Route>>();
                var routeDb = await routeRepository!.GetAllDataByExpression(r => dayPlanIds.Contains(r.DayPlanId),0,0, null, false, null);
                if (routeDb.Items!.Count == 0)
                {
                    return result;
                }
                HashSet<Guid> destinationIds = new HashSet<Guid>();
                foreach(var route in routeDb.Items)
                {
                    destinationIds.Add((Guid)route.StartPointId!);
                    destinationIds.Add((Guid)route.EndPointId);
                }
                //var destinationRepository = Resolve<IRepository<Destination>>();
                //var destinationDb = await destinationRepository!.GetAllDataByExpression(r => destinationIds.Contains(r.Id), 0, 0, null, false, r => r.Communce!.District!.Province!);
                //data.InsertRange(Math.Max(0, data.Count - 1), destinationDb!.Items!.Select(r => r.Communce!.District!).ToList()!);
                result.Result = new PagedResult<District>
                {
                    Items = data.DistinctBy(p => p.Id).ToList()
                };
            } catch ( Exception ex )
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
