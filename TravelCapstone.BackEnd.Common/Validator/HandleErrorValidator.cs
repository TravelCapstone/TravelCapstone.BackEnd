using FluentValidation.Results;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Common.Validator;

public class HandleErrorValidator
{
    public AppActionResult HandleError(ValidationResult result)
    {
        if (!result.IsValid)
        {
            var errorMessage = new List<string>();
            foreach (var error in result.Errors) errorMessage.Add(error.ErrorMessage);
            return new AppActionResult
            {
                IsSuccess = false,
                Messages = errorMessage,
                Result = null
            };
        }

        return new AppActionResult();
    }
}