using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices;

public interface ILocationService
{
    Task<AppActionResult> GetAllProvince();
    Task<AppActionResult> GetAllDistrictByProvinceId(Guid provinceId);
    Task<AppActionResult> GetAllCommuneByDistrictId(Guid districtId);
    Task<AppActionResult> GetAllCommuneByDistrictNameAndCommuneName(string districtName, string communeName);
}