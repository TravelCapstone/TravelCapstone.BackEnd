namespace TravelCapstone.BackEnd.Application.Services;

public class GenericBackendService
{
    private IServiceProvider _serviceProvider;

    public GenericBackendService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T? Resolve<T>()
    {
        return (T)_serviceProvider.GetService(typeof(T))!;
    }
}