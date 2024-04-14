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
            if(privateTourequestDTO.RequestedLocations != null && privateTourequestDTO.RequestedLocations.Count > 0)
            {
                var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
                foreach(var requestedLocation in privateTourequestDTO.RequestedLocations)
                {
                    await requestedLocationRepository!.Insert(new RequestedLocation
                    {
                        Id = Guid.NewGuid(),
                        PrivateTourRequestId = request.Id,
                        ProvinceId = requestedLocation.Id
                    });
                }
            }

            if (!BuildAppActionResultIsError(result))
            {
                await _unitOfWork.SaveChangesAsync();
            }
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
        OptionListResponseDto data = new OptionListResponseDto();
        try 
        {
            var privateTourRequestDb = await _repository.GetById(id);
            if(privateTourRequestDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour riêng tư với id {id}");
                return result;
            }
            data.PrivateTourRespone = _mapper.Map<PrivateTourResponeDto>(privateTourRequestDb);
            var optionQuotationRepositry = Resolve<IRepository<OptionQuotation>>();
            var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            var optionsDb = await optionQuotationRepositry.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0);
            if (optionsDb.Items.Count != 3)
            {
                result.Messages.Add($"Số lượng lựa chọn hiện tại: {optionsDb.Items.Count} không phù hợp");
            }
            //Remind: add check 3 optionType
            int fullOption = 0;
            optionsDb.Items.ForEach(o => fullOption ^= (int)(o.OptionClass));
            if(fullOption != 3)
            {
                result.Messages.Add("Danh sách lựa chọn không đủ các hạng mục");
            }

            foreach( var item in optionsDb.Items ) {
                OptionResponseDto option = new OptionResponseDto();
                option.OptionQuotation = item;
           //     var quotationDetailDb = await quotationDetailRepository.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, q => q.SellPriceHistory);
        //        option.QuotationDetails = quotationDetailDb.Items.ToList();
                //Option to order of OptionClass
                if(item.OptionClass == OptionClass.ECONOMY)
                    data.Option1 = option;
                else if(item.OptionClass == OptionClass.MIDDLE)
                    data.Option2 = option;
                else data.Option3 = option;
            }
            result.Result = data;

        } catch (Exception e)
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
            

            var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
            if (privateTourRequest == null)
            {
                result = BuildAppActionResultError(result, $"Không tồn tại request với id {dto.PrivateTourRequestId}");
            }

            foreach (var item in dto.Option1.Services)
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
            foreach (var item in dto.Option2.Services)
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
            foreach (var item in dto.Option3.Services)
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
                OptionQuotation quotation1 = new OptionQuotation()
                {
                    Id =   Guid.NewGuid(),
                    OptionClass = dto.Option1.OptionClass,
                    Status = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId,
                    Name = dto.Option1.Name,
                };
                OptionQuotation quotation2 = new OptionQuotation()
                {
                    Id = Guid.NewGuid(),
                    OptionClass = dto.Option2.OptionClass,
                    Status = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId,
                    Name = dto.Option2.Name,
                };
                OptionQuotation quotation3 = new OptionQuotation()
                {
                    Id = Guid.NewGuid(),
                    OptionClass = dto.Option3.OptionClass,
                    Status = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId,
                    Name = dto.Option3.Name,
                };
                optionQuotations.Add(quotation1);
                optionQuotations.Add(quotation2);
                optionQuotations.Add(quotation3);

                foreach (var item in dto.Option1.Services)
                {
                    double total = 0;
                   

                    var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.ServiceId == item.ServiceId,
                        0, 0, null
                    );
                    var list = sellPriceHistory.Items!.OrderByDescending(o => o.Date);
                    QuotationDetail quotationDetail = new QuotationDetail()
                    {
                        Id = Guid.NewGuid(),
                        //Quantity = item.Quantity,
                        OptionQuotationId = quotation1.Id,
                  //      SellPriceHistoryId = list.First().Id
                    };
                    var priceHistoryMatchMOQ = new SellPriceHistory();
                    foreach (var price in list)
                    {
                        if (privateTourRequest!.NumOfAdult > price.MOQ)
                        {
                      //      total = price.PricePerAdult * quotationDetail.Quantity;
                        //    total += price.PricePerChild * quotationDetail.Quantity;
                            break;
                        }
                    }
                    quotation1.Total = total;
                    quotationDetails.Add(quotationDetail);
                }
                foreach (var item in dto.Option2.Services)
                {
                    double total = 0;
                  
                    var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.ServiceId == item.ServiceId,
                        0, 0, null
                    );
                    var list = sellPriceHistory.Items!.OrderByDescending(o => o.Date);
                    QuotationDetail quotationDetail = new QuotationDetail()
                    {
                        Id = Guid.NewGuid(),
                //        Quantity = item.Quantity,
                        OptionQuotationId = quotation2.Id,
                  //      SellPriceHistoryId = list.First().Id
                    };
                    var priceHistoryMatchMOQ = new SellPriceHistory();
                    foreach (var price in list)
                    {
                        if (privateTourRequest!.NumOfAdult > price.MOQ)
                        {
                   //         total = price.PricePerAdult * quotationDetail.Quantity;
                    //        total += price.PricePerChild * quotationDetail.Quantity;
                            break;
                        }
                    }
                    quotation2.Total = total;
                    quotationDetails.Add(quotationDetail);
                }

                foreach (var item in dto.Option3.Services)
                {
                    double total = 0;

                    var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.ServiceId == item.ServiceId,
                        0, 0, null
                    );
                    var list = sellPriceHistory.Items!.OrderByDescending(o => o.Date);
                    QuotationDetail quotationDetail = new QuotationDetail()
                    {
                        Id = Guid.NewGuid(),
                  //      Quantity = item.Quantity,
                        OptionQuotationId = quotation3.Id,
                //        SellPriceHistoryId = list.First().Id
                    };
                    var priceHistoryMatchMOQ = new SellPriceHistory();
                    foreach (var price in list)
                    {
                        if (privateTourRequest!.NumOfAdult > price.MOQ)
                        {
                     //       total = price.PricePerAdult * quotationDetail.Quantity;
                   //         total += price.PricePerChild * quotationDetail.Quantity;
                            break;
                        }
                    }
                    quotation3.Total = total;
                    quotationDetails.Add(quotationDetail);
                }

                privateTourRequest!.Status = PrivateTourStatus.WAITINGFORCUSTOMER;
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
        var travelCompanionRepository = Resolve<IRepository<Customer>>();
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
            }
            else if (option.Status != OptionQuotationStatus.NEW)
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
                option!.PrivateTourRequest!.Status = PrivateTourStatus.APPROVED;
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
                    CustomerId = travelCompanion!.Id,
                    TourId = option.PrivateTourRequest.TourId,
                    NumOfAdult = option.PrivateTourRequest.NumOfAdult,
                    NumOfChildren = option.PrivateTourRequest.NumOfChildren,
                    PrivateTourRequestId = option.PrivateTourRequestId
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