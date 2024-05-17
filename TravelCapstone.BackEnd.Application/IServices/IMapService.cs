using System.Collections.Generic;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IMapService
{
    public Task<AppActionResult> Geocode(string address);

    public Task<AppActionResult> AutoComplete(string address);
    public Task<AppActionResult> GetVehicle(Guid startPoint, Guid endPoint);
    public Task<AppActionResult> ImportPositionToProvince();
    public Task<AppActionResult> FindOptimalPath(Guid StartDestinationId, List<Guid> DestinationIds, bool IsPilgrimageTrip);
    public Task<AppActionResult> GetEstimateTripDate(Guid StartDestinationId, List<Guid> DestinationIds, VehicleType vehicleType, DateTime startDate, DateTime endDate);

}