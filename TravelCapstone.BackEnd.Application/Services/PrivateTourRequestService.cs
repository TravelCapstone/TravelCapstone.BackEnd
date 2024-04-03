using AutoMapper;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class PrivateTourRequestService : GenericBackendService, IPrivateTourRequestService
{
    private readonly IMapper _mapper;
    private readonly IRepository<PrivateTourRequest> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PrivateTourRequestService(
        IRepository<PrivateTourRequest> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider
    ) : base(serviceProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AppActionResult> CreatePrivateTourRequest(PrivateTourRequestDTO privateTourequestDTO)
    {
        var result = new AppActionResult();
        try
        {
            var tourRepository = Resolve<IRepository<Tour>>();
            var tourDb = await tourRepository!.GetById(privateTourequestDTO.TourId);
            if (tourDb == null)
            {
                result = BuildAppActionResultError(result,
                    $"Không tìm thấy tour với id {privateTourequestDTO.TourId}");
                return result;
            }

            var accountRepository = Resolve<IRepository<Account>>();
            var accountDb = await accountRepository!.GetById(privateTourequestDTO.AccountId!);
            if (accountDb == null)
            {
                result = BuildAppActionResultError(result,
                    $"Không tìm thấy tài khoản với id {privateTourequestDTO.AccountId}");
                return result;
            }

            //Need improvement for condition
            if (!IsValidTime(tourDb.EndDate, tourDb.StartDate, privateTourequestDTO.EndDate,
                    privateTourequestDTO.StartDate))
            {
                result = BuildAppActionResultError(result, "Lộ trình yêu cầu ngắn hơn lộ trình của tour mẫu");
                return result;
            }

            var request = _mapper.Map<PrivateTourRequest>(privateTourequestDTO);
            request.Id = Guid.NewGuid();
            request.Status = PrivateTourStatus.NEW;
            await _repository.Insert(request);
            await _unitOfWork.SaveChangesAsync();
            result.Result = request;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    private bool IsValidTime(DateTime cloneEnd, DateTime cloneStart, DateTime requestEnd, DateTime requestStart)
    {
        var cloneResult = GetDaysAndNights(cloneStart, cloneEnd);
        var requestResult = GetDaysAndNights(requestStart, requestEnd);
        return cloneResult[0] <= requestResult[0] && cloneResult[1] <= requestResult[1];
    }

    private int[] GetDaysAndNights(DateTime start, DateTime end)
    {
        var res = new int[2];
        var curr = start;
        while (curr < end)
        {
            if (IsDayTime(curr))
                res[0]++;
            else
                res[1]++;
            curr = curr.AddHours(12);
        }

        return res;
    }

    private bool IsDayTime(DateTime start)
    {
        return start.Hour >= 6 && start.Hour < 18;
    }

    public async Task<AppActionResult> GetAllTourPrivate(int pageNumber, int pageSize)
    {
        var result = new AppActionResult();
        var accountRepository = Resolve<IRepository<Account>>();
        try
        {
            var data = await _repository.GetAllDataByExpression
            (null, pageNumber,
                pageSize,
                p => p.Tour, p => p.Account!);
            result.Result = _mapper.Map<PagedResult<PrivateTourResponeDto>>(data);
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetPrivateTourRequestById(Guid id)
    {
        var result = new AppActionResult();
        var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
        try
        {
            result.Result = await quotationDetailRepository!.GetAllDataByExpression(
                a => a.OptionQuotation!.PrivateTourRequestId == id,
                0,
                0,
                a => a.OptionQuotation!.PrivateTourRequest!,
                a => a.SellPriceHistory!.Service!
            );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto)
    {
        var result = new AppActionResult();
        var serviceRepository = Resolve<IRepository<Service>>();
        var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
        var optionDetailRepository = Resolve<IRepository<QuotationDetail>>();
        var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
        try
        {
            if (dto.OptionRequestDtos.Count != 3)
            {
                result = BuildAppActionResultError(result, $"Hệ thống chỉ hỗ trợ tạo 3 tùy chọn");
            }

            var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
            if (privateTourRequest == null)
            {
                result = BuildAppActionResultError(result, $"Không tồn tại request với id {dto.PrivateTourRequestId}");
            }

            foreach (var item in dto.OptionRequestDtos)
            {
                var serviceProvider = await serviceRepository!.GetById(item.ServiceId);
                if (serviceProvider == null)
                {
                    result = BuildAppActionResultError(result, $"Không tồn tại service với id {item.ServiceId}");
                }

                var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(
                    a => a.ServiceId == item.ServiceId,
                    0, 0, null
                );
                if (!sellPriceHistory.Items!.Any())
                {
                    result = BuildAppActionResultError(result,
                        $"Không tồn tại giá trong hệ thống với service id {item.ServiceId}");
                }
            }

            if (!BuildAppActionResultIsError(result))
            {
                List<QuotationDetail> quotationDetails = new List<QuotationDetail>();
                List<OptionQuotation> optionQuotations = new List<OptionQuotation>();
                foreach (var item in dto.OptionRequestDtos)
                {
                    double total = 0;
                    var quotationId = Guid.NewGuid();
                    OptionQuotation quotation = new OptionQuotation()
                    {
                        Id = quotationId,
                        OptionClass = item.OptionClass,
                        Status = OptionQuotationStatus.NEW,
                        PrivateTourRequestId = dto.PrivateTourRequestId,
                        Name = item.Name,
                        Description = item.Description
                    };

                    var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.ServiceId == item.ServiceId,
                        0, 0, null
                    );
                    var list = sellPriceHistory.Items!.OrderByDescending(o => o.Date);
                    QuotationDetail quotationDetail = new QuotationDetail()
                    {
                        Id = Guid.NewGuid(),
                        Quantity = item.Quantity,
                        OptionQuotationId = quotationId,
                        SellPriceHistoryId = list.First().Id
                    };
                    total = list.First().Price * quotationDetail.Quantity;
                    quotation.Total = total;
                    optionQuotations.Add(quotation);
                    quotationDetails.Add(quotationDetail);
                }

                await optionQuotationRepository!.InsertRange(optionQuotations);
                await optionDetailRepository!.InsertRange(quotationDetails);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> ConfirmOptionPrivateTour(Guid optionId, string accountId)
    {
        var result = new AppActionResult();
        var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
        var orderRepository = Resolve<IRepository<Order>>();
        var travelCompanionRepository = Resolve<IRepository<TravelCompanion>>();
        var option =
            await optionQuotationRepository!.GetByExpression(
                a => a!.Id == optionId, 
                a => a.PrivateTourRequest!
                );
        var travelCompanion = await travelCompanionRepository!.GetByExpression(
            a => a!.AccountId == accountId,
            null);
        try
        {
            if (option == null)
            {
                result = BuildAppActionResultError(result,
                    $"Option với id {optionId} không tồn tại trong hệ thống");
            }else if (option.Status != OptionQuotationStatus.NEW)
            {
                result = BuildAppActionResultError(result, 
                    $"Chỉ chấp nhận confirm với option trạng thái NEW");
            }
            if (travelCompanion == null)
            {
                result = BuildAppActionResultError(result, 
                    $"Không thấy thông tin trùng khớp với travel companion");

            }
            if (!BuildAppActionResultIsError(result))
            {
                
                option!.Status = OptionQuotationStatus.ACTIVE;
                var listOption =
                    await optionQuotationRepository.GetAllDataByExpression(
                        a => a.PrivateTourRequestId == option!.PrivateTourRequestId,
                        0,
                        0,
                        null);
                foreach (var item in listOption.Items!)
                {
                    item.Status = OptionQuotationStatus.IN_ACTIVE;
                }

                await orderRepository!.Insert(new Order()
                {
                    Id = Guid.NewGuid(),
                    Content = $"Thanh toán cho private tour {option.PrivateTourRequest!.Name}",
                    Total = option.Total,
                    OrderStatus = OrderStatus.NEW,
                    TravelCompanionId = travelCompanion!.Id,
                    TourId = option.PrivateTourRequest.TourId,
                    NumOfAdult = option.PrivateTourRequest.NumOfAdult,
                    NumOfChildren = option.PrivateTourRequest.NumOfChildren
                });
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }
        return result;
    }
}