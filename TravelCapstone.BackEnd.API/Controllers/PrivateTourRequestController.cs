using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Validator;

namespace TravelCapstone.BackEnd.API.Controllers;

public class PrivateTourRequestController : Controller
{
    private readonly HandleErrorValidator _handleErrorValidator;
    private readonly IPrivateTourRequestService _service;
    private readonly IValidator<PrivateTourRequestDTO> _validator;

    public PrivateTourRequestController(IPrivateTourRequestService service, HandleErrorValidator handleErrorValidator,
        IValidator<PrivateTourRequestDTO> validator)
    {
        _service = service;
        _handleErrorValidator = handleErrorValidator;
        _validator = validator;
    }

    [HttpPost("create-private-tour-request")]
    public async Task<AppActionResult> CreatePrivateTourRequest([FromBody] PrivateTourRequestDTO request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid) return _handleErrorValidator.HandleError(result);
        return await _service.CreatePrivateTourRequest(request);
    }

    [HttpGet("get-all-private-tour-request/{pageNumber:int}/{pageSize:int}")]
    public async Task<AppActionResult> GetAllPrivateTourRequest(int pageNumber = 1, int pageSize = 10)
    {
        return await _service.GetAllPrivateTourRequest(pageNumber, pageSize);
    }

    [HttpGet("get-private-tour-request-by-id/{id}")]
    public async Task<AppActionResult> GetPrivateTourRequestById(Guid id)
    {
        return await _service.GetPrivateTourRequestById(id);
    }

    [HttpGet("get-private-tour-request-by-id-for-customer/{id}")]
    public async Task<AppActionResult> GetPrivateTourRequestByIdForCustomer(Guid id)
    {
        return await _service.GetPrivateTourRequestByIdForCustomer(id);
    }

    [HttpPost("create-options-private-tour")]
    public async Task<AppActionResult> CreateOptionsPrivateTour([FromBody] CreateOptionsPrivateTourDto request)
    {
        return await _service.CreateOptionsPrivateTour(request);
    }

    [HttpPost("confirm-options-private-tour")]
    public async Task<AppActionResult> ConfirmOptionPrivateTour(Guid optionId, string accountId)
    {
        return await _service.ConfirmOptionPrivateTour(optionId, accountId);
    }
    [HttpPut("send-to-customer/{privateTourRequestId}")]
    public async Task<AppActionResult> SendToCustomer(Guid privateTourRequestId)
    {
        return await _service.SendToCustomer(privateTourRequestId);
    }

    [HttpGet("get-excel-quotation/{privateTourRequestId}")]
    public async Task<IActionResult> GetExcelQuotation(Guid privateTourRequestId)
    {
        return await _service.GetExcelQuotation(privateTourRequestId);
    }
}