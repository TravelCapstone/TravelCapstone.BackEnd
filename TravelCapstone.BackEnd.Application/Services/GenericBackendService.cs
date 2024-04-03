using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.Services;

public class GenericBackendService
{
    private readonly IServiceProvider _serviceProvider;

    public GenericBackendService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T? Resolve<T>()
    {
        return (T)_serviceProvider.GetService(typeof(T))!;
    }

    public AppActionResult BuildAppActionResultError(AppActionResult result, string messageError)
    {
        List<string?> errors;
        errors = new List<string?> { messageError };

        return new AppActionResult
        {
            IsSuccess = false,
            Messages = errors,
            Result = null
        };
    }

    public bool BuildAppActionResultIsError(AppActionResult result)
    {
        return !result.IsSuccess ? true : false;
    }
}