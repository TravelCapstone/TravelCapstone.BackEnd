using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface IMapService
{
    public Task<AppActionResult> Geocode(string address);
    public Task<AppActionResult> AutoComplete(string address);
}