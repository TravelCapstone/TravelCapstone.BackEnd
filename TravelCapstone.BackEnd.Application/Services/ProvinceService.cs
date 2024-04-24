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
    public class ProvinceService: GenericBackendService, IProvinceService
    {
        private readonly IRepository<Province> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProvinceService(
            IRepository<Province> repository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetAllProvinceByPrivateTourRequestId(Guid Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                List<Province> data = new List<Province>();
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourRequestDb = await privateTourRequestRepository!.GetByExpression(p => p.Id == Id, p => p.Tour); 
                if(privateTourRequestDb == null)
                {
                    return result;
                }
                var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
                var requestedLocationListDb = await requestedLocationRepository!.GetAllDataByExpression(null, 0, 0, null, false, r => r.Province!);
                data.InsertRange(0, requestedLocationListDb.Items!.Select(r => r.Province!));
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
                    destinationIds.Add(route.EndPointId);
                }
                var destinationRepository = Resolve<IRepository<Destination>>();
                var destinationDb = await destinationRepository!.GetAllDataByExpression(r => destinationIds.Contains(r.Id), 1, Int32.MaxValue, null, false, r => r.Communce!.District!.Province!);
                data.InsertRange(Math.Max(0, data.Count - 1), destinationDb!.Items!.Select(r => r.Communce!.District!.Province).ToList()!);
                result.Result = data.DistinctBy(p => p.Id);
            } catch ( Exception ex )
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
