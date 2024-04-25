using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IMapService
{
    public Task<AppActionResult> Geocode(string address);

    public Task<AppActionResult> AutoComplete(string address);
    public Task<AppActionResult> GetVehicle(Guid startPoint, Guid endPoint);
    public Task<AppActionResult> ImportPositionToProvince();
    public Task<AppActionResult> FindOptimalPath(Guid StartDestinationId, List<Guid> DestinationIds, bool IsPilgrimageTrip);
}