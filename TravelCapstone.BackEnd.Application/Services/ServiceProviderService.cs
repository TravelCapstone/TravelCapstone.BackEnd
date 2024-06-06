using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class ServiceProviderService : GenericBackendService, IServiceProviderService
{
    private readonly IRepository<ServiceProvider> _serviceProviderRepository;
    private readonly IRepository<Facility> _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ServiceProviderService(IServiceProvider serviceProvider,
        IRepository<ServiceProvider> serviceProviderRepository,
        IRepository<Facility> serviceRepository,
        IUnitOfWork unitOfWork
    ) : base(serviceProvider)
    {
        _serviceProviderRepository = serviceProviderRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppActionResult> GetAllServiceProvider(string? keyword, int pageNumber, int pageSize)
    {
        var result = new AppActionResult();
        try
        {
            if (string.IsNullOrEmpty(keyword))
                result.Result = await _serviceProviderRepository.GetAllDataByExpression(
                    null,
                    pageNumber,
                    pageSize,
                    null
                );
            else
                result.Result = await _serviceProviderRepository.GetAllDataByExpression(
                    a => a.Name.ToLower().Trim().Contains(keyword.ToLower().Trim()),
                    pageNumber,
                    pageSize,
                    null
                );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> CreateServiceProvider(ServiceProviderDto serviceProviderDto)
    {
        var result = new AppActionResult();
        try
        {
            var serviceProviderId = Guid.NewGuid();
            var listService = new List<Facility>();
            await _serviceProviderRepository.Insert(
                new ServiceProvider
                {
                    Name = serviceProviderDto.Name,
                    Id = serviceProviderId
                });
            foreach (var item in serviceProviderDto.Services)
            {
                var service = new Facility
                {
                    Name = item.Name,
                    Id = Guid.NewGuid(),
                    Address = item.Address,
                    Description = item.Description,
                    CommunceId = item.CommunceId,
                    IsActive = true,
                    PhoneNumber = item.PhoneNumber,
                    //Type = item.Type,
                    ServiceProviderId = serviceProviderId
                };
                listService.Add(service);
            }

            await _serviceRepository.InsertRange(listService);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> ChangeStatusServiceId(Guid serviceId)
    {
        var result = new AppActionResult();
        try
        {
            var service = await _serviceRepository!.GetById(serviceId);
            if (service == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ với ID {serviceId}");
            }
            else
            {
                service.IsActive = !service.IsActive;
                if (!BuildAppActionResultIsError(result))
                {
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetServiceProviderById(Guid id)
    {
        var result = new AppActionResult();
        try
        {
            result.Result =
                await _serviceRepository.GetAllDataByExpression(a => a.ServiceProvider!.Id == id, 0, 0, null);
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }
}