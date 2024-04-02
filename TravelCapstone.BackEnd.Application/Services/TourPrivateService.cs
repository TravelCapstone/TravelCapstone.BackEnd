using AutoMapper;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class TourPrivateService : GenericBackendService, ITourPrivateService
{
    private IRepository<PrivateTourRequest> _repository;
    private IMapper _mapper;

    public TourPrivateService(IServiceProvider serviceProvider, IRepository<PrivateTourRequest> repository, IMapper mapper) : base(serviceProvider)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AppActionResult> GetAllTourPrivate(int pageNumber, int pageSize)
    {
        AppActionResult result = new AppActionResult();
        var accountRepository = Resolve<IRepository<Account>>();
        try
        {
            var data = await _repository.GetAllDataByExpression
            (null, pageNumber: pageNumber,
                pageSize: pageSize,
                p => p.Tour, p => p.Account!);
            result.Result =   _mapper.Map<PagedResult<PrivateTourRequestDto>>(data);
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }
}