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
    public class DriverService : GenericBackendService, IDriverService
    {
        private readonly IRepository<Driver> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DriverService(IServiceProvider serviceProvider,
            IRepository<Driver> repository,
            IUnitOfWork unitOfWork
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppActionResult> GetAvailableDriver(DateTime startTime, DateTime endTime)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var vehicleRouteRepository = Resolve<IRepository<VehicleRoute>>();
                var collidedVehicleRouteDb = await vehicleRouteRepository!.GetAllDataByExpression(v => v.Route.StartTime.Date >= startTime.Date && v.Route.EndTime.Date <= endTime.Date
                                                                                                    || v.Route.EndTime.Date >= endTime.Date && v.Route.StartTime.Date <= startTime.Date
                                                                                                    || v.Route.StartTime.Date <= endTime.Date && startTime.Date <= v.Route.EndTime.Date, 0, 0, null, false, null);
                if (collidedVehicleRouteDb.Items != null && collidedVehicleRouteDb.Items.Count > 0)
                {
                    var driverIds = collidedVehicleRouteDb.Items.Select(v => v.DriverId).ToList();
                    result.Result = await _repository.GetAllDataByExpression(d => !driverIds.Contains(d.Id), 0, 0, null, false, null);
                } else
                {
                    result.Result = await _repository.GetAllDataByExpression(null, 0, 0, null, false, null);
                }
            } catch(Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
