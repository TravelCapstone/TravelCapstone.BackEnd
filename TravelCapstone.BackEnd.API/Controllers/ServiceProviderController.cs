using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.API.Controllers;

[Route("service-provider")]
[ApiController]
public class ServiceProviderController : ControllerBase
{
    private readonly IServiceProviderService _service;

    public ServiceProviderController(IServiceProviderService service)
    {
        _service = service;
    }

    [HttpGet("get-all/{pageNumber:int}/{pageSize:int}")]
    public async Task<AppActionResult> GetAllServiceProvider(string? keyword, int pageNumber = 1, int pageSize = 10)
    {
        return await _service.GetAllServiceProvider(keyword, pageNumber, pageSize);
    }

    [HttpGet("get-service-provider-by-id/{serviceProviderId}")]
    public async Task<AppActionResult> GetServiceProviderById(Guid serviceProviderId)
    {
        return await _service.GetServiceProviderById(serviceProviderId);
    }

    [HttpPost("create-service-provider")]
    public async Task<AppActionResult> CreateServiceProvider(ServiceProviderDto dto)
    {
        return await _service.CreateServiceProvider(dto);
    }

    [HttpPut("change-status-service/{serviceId}")]
    public async Task<AppActionResult> ChangeStatusServiceId(Guid serviceId)
    {
        return await _service.ChangeStatusServiceId(serviceId);
    }
}