using FluentValidation;
using HCQS.BackEnd.Common.Validator;
using Microsoft.AspNetCore.Mvc;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;

namespace TravelCapstone.BackEnd.API.Controllers
{
    public class PrivateTourRequestController : Controller
    {
        private readonly IPrivateTourRequestService _service;
        private readonly IValidator<PrivateTourRequestDTO> _validator;
        private readonly HandleErrorValidator _handleErrorValidator;
        public PrivateTourRequestController(IPrivateTourRequestService service, HandleErrorValidator handleErrorValidator, IValidator<PrivateTourRequestDTO> validator)
        {
            _service = service;
            _handleErrorValidator = handleErrorValidator;
            _validator = validator;
        }

        [HttpPost("create-private-tour-request")]
        public async Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO request)
        {
            var result = await _validator.ValidateAsync(request);
            if (!result.IsValid)
            {
                return _handleErrorValidator.HandleError(result);
            }
            return await _service.CreatePrivateTourRequest(request);
        }
    }
}
