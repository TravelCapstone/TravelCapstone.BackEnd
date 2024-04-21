using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("location")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationService _service;

    public LocationController(ILocationService service)
    {
        _service = service;
    }

    [HttpGet("get-all-province")]
    public async Task<AppActionResult> GetAllProvince()
    {
        return await _service.GetAllProvince();
    }
    [HttpGet("get-province-by-name/{provinceName}")]
    public async Task<AppActionResult> GetProvinceByName(string provinceName)
    {
        return await _service.GetProvinceByName(provinceName);
    }

    [HttpGet("get-all-district-by-provinceId/{provinceId}")]
    public async Task<AppActionResult> GetAllDistrictByProvinceId(Guid provinceId)
    {
        return await _service.GetAllDistrictByProvinceId(provinceId);
    }

    [HttpGet("get-all-commune-by-districtId/{districtId}")]
    public async Task<AppActionResult> GetAllCommuneByDistrictId(Guid districtId)
    {
        return await _service.GetAllCommuneByDistrictId(districtId);
    }

    [HttpGet("get-all-commune-by-districtName-communeName/{districtName}/{communeName}")]
    public async Task<AppActionResult> GetAllCommuneByDistrictNameAndCommuneName(string districtName,
        string communeName)
    {
        return await _service.GetAllCommuneByDistrictNameAndCommuneName(districtName, communeName);
    }
}