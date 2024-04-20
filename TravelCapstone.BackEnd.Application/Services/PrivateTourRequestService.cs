using AutoMapper;
using NPOI.SS.Formula;
using System.ComponentModel.Design;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using static TravelCapstone.BackEnd.Common.DTO.Response.MapInfo;

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

            if (!IsValidRequestTime(privateTourequestDTO.EndDate, privateTourequestDTO.StartDate, privateTourequestDTO.NumOfDay,
                    privateTourequestDTO.NumOfNight))
            {
                result = BuildAppActionResultError(result, "Thời gian có thể đi ngắn hơn dộ dài lộ trình yêu cầu");
                return result;
            }

            if (tourDb.EndDate - tourDb.StartDate > privateTourequestDTO.EndDate - privateTourequestDTO.StartDate)
            {
                result = BuildAppActionResultError(result, "Thời gian có thể đi ngắn hơn lộ trình của tour mẫu");
                return result;
            }
            //Need improvement for condition
            if (!IsValidTime(tourDb.EndDate, tourDb.StartDate, privateTourequestDTO.NumOfDay,
                    privateTourequestDTO.NumOfNight))
            {
                result = BuildAppActionResultError(result, "Lộ trình yêu cầu ngắn hơn lộ trình của tour mẫu");
                return result;
            }

            var request = _mapper.Map<PrivateTourRequest>(privateTourequestDTO);
            var utility = Resolve<Utility>();
            request.CreateDate = utility!.GetCurrentDateTimeInTimeZone();
            request.Id = Guid.NewGuid();
            request.PrivateTourStatusId = PrivateTourStatus.NEW;
            await _repository.Insert(request);
            if (privateTourequestDTO.OtherLocationIds != null && privateTourequestDTO.OtherLocationIds.Count > 0)
            {
                var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
                foreach (var requestedLocationId in privateTourequestDTO.OtherLocationIds)
                {
                    await requestedLocationRepository!.Insert(new RequestedLocation
                    {
                        Id = Guid.NewGuid(),
                        PrivateTourRequestId = request.Id,
                        ProvinceId = requestedLocationId
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

    private bool IsValidRequestTime(DateTime cloneEnd, DateTime cloneStart, int numOfDay, int numOfNight)
    {
        var cloneResult = GetDaysAndNights(cloneStart, cloneEnd);
        return cloneResult[0] >= numOfDay && cloneResult[1] >= numOfNight;
    }

    private bool IsValidTime(DateTime cloneEnd, DateTime cloneStart, int numOfDay, int numOfNight)
    {
        var cloneResult = GetDaysAndNights(cloneStart, cloneEnd);
        return cloneResult[0] <= numOfDay && cloneResult[1] <= numOfNight;
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
        var requestLocationRepository = Resolve<IRepository<RequestedLocation>>();
        try
        {
            var data = await _repository.GetAllDataByExpression
            (null, pageNumber,
                pageSize,
                p => p.Tour, p => p.CreateByAccount!, p => p.Province);
            
            var responseList = _mapper.Map<PagedResult<PrivateTourResponeDto>>(data);
            foreach (var item in responseList.Items!)
            {
                var requestLocationDb = await requestLocationRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == item.Id, 0, 0, r => r.Province);
                item.OtherLocation = requestLocationDb.Items!.Select(r => r.Province).ToList()!;
            }
            result.Result = responseList;
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
            if (privateTourRequestDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour riêng tư với id {id}");
                return result;
            }
            data.PrivateTourRespone = _mapper.Map<PrivateTourResponeDto>(privateTourRequestDb);
            var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            var optionsDb = await optionQuotationRepository!.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0);
            if (optionsDb.Items!.Count != 3)
            {
                result.Messages.Add($"Số lượng lựa chọn hiện tại: {optionsDb.Items.Count} không phù hợp");
            }
            //Remind: add check 3 optionType
            int fullOption = 0;
            optionsDb.Items.ForEach(o => fullOption ^= (int)(o.OptionClassId));
            if (fullOption != 3)
            {
                result.Messages.Add("Danh sách lựa chọn không đủ các hạng mục");
            }

            foreach (var item in optionsDb.Items)
            {
                OptionResponseDto option = new OptionResponseDto();
                option.OptionQuotation = item;
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null);
                option.QuotationDetails = quotationDetailDb.Items!.ToList();
                option.VehicleQuotationDetails = vehicleQuotationDetailDb.Items!.ToList();
                //Option to order of OptionClass
                if (item.OptionClassId == OptionClass.ECONOMY)
                    data.Option1 = option;
                else if (item.OptionClassId == OptionClass.MIDDLE)
                    data.Option2 = option;
                else data.Option3 = option;
            }
            result.Result = data;
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
        var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
        var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
        var provinceRepository = Resolve<IRepository<Province>>();
        var serviceRepository = Resolve<IRepository<Service>>();
        var serviceRatingRepository = Resolve<IRepository<ServiceRating>>();
        var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
        var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
        var referenceTransportPriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
        var serviceService = Resolve<IServiceService>();
        try
        {


            foreach (var location in dto.Locations)
            {
                var province = await provinceRepository!.GetById(location.ProvinceId);
                if (province == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy tỉnh với id {location.ProvinceId}");
                }
                foreach (var hotel in location.Hotels)
                {
                    var listHotel = await serviceRepository!.GetAllDataByExpression(
                        a => a.Communce!.District!.ProvinceId == location.ProvinceId
                         && a.ServiceRating!.Rating == hotel.Rating
                         && a.ServiceRating.ServiceTypeId == ServiceType.HOTEL,
                        0,
                        0,
                        null);
                    var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.Service!.Communce!.District!.ProvinceId == location.ProvinceId
                        && a.Service.ServiceRating!.ServiceTypeId == ServiceType.HOTEL &&
                        a.Service.ServiceRating.Rating == hotel.Rating,
                        0, 0, null
                        );
                    if (!listHotel.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {hotel.Rating} tại tỉnh thuộc id {location.ProvinceId}");
                    }
                    if (sellHotel.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {hotel.Rating} tại tỉnh thuộc id {location.ProvinceId}");
                    }
                }
                foreach (var restaurent in location.Restaurants)
                {
                    var listRestaurent = await serviceRepository!.GetAllDataByExpression(
                        a => a.Communce!.District!.ProvinceId == location.ProvinceId
                         && a.ServiceRating!.Rating == restaurent.Rating
                         && a.ServiceRating.ServiceTypeId == ServiceType.RESTAURANTS,
                        0,
                        0,
                        null);
                    var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
                       a => a.Service!.Communce!.District!.ProvinceId == location.ProvinceId
                       && a.Service.ServiceRating!.ServiceTypeId == ServiceType.RESTAURANTS &&
                       a.Service.ServiceRating.Rating == restaurent.Rating,
                       0, 0, null
                       );
                    if (!listRestaurent.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy nhà hàng với phân loại {restaurent.Rating} tại tỉnh thuộc id {location.ProvinceId}");
                    }
                    if (!sellRestaurent.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy giá nhà hàng với phân loại {restaurent.Rating} tại tỉnh thuộc id {location.ProvinceId}");
                    }
                }

                var entertaiment = await serviceRepository!.GetAllDataByExpression(
                         a => a.Communce!.District!.ProvinceId == location.ProvinceId
                          && a.ServiceRating!.ServiceTypeId == ServiceType.ENTERTAINMENT,
                         0,
                         0,
                         null);
                if (entertaiment.Items!.Any())
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ vui chơi giải trí tại tỉnh thuộc id {location.ProvinceId}");
                }
            }
            if (!BuildAppActionResultIsError(result))
            {
                var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
                var totalPeople = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren;


                OptionQuotation option = new OptionQuotation
                {
                    Id = Guid.NewGuid(),
                    MinTotal = 0,
                    MaxTotal = 0,
                    OptionClassId = dto.OptionClass,
                    OptionQuotationStatusId = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId
                };

                List<QuotationDetail> quotationDetails = new List<QuotationDetail>();
                List<VehicleQuotationDetail> vehicleQuotationDetails = new List<VehicleQuotationDetail>();

                foreach (var location in dto.Locations)
                {
                    var estimateService = await serviceService!.GetServiceByProvinceIdAndRequestId(location.ProvinceId, dto.PrivateTourRequestId);
                    var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                    foreach (var hotel in location.Hotels)
                    {
                        var serviceRating = await serviceRatingRepository!.GetByExpression(a => a.ServiceTypeId == ServiceType.HOTEL && a.Rating == hotel.Rating);
                        var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                            a => a.ServiceRating.ServiceTypeId == ServiceType.HOTEL && a.ServiceRating.Rating == hotel.Rating
                            && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 4

                            ).FirstOrDefault();
                        double min = sellPriceHotel!.MinPrice;

                        double max = sellPriceHotel!.MaxPrice;
                        double minQuotation = 0;
                        double maxQuotation = 0;

                        if (privateTourRequest.NumOfChildren == 0)
                        {
                            if (privateTourRequest.NumOfAdult % sellPriceHotel.ServingQuantity == 0)
                            {
                                minQuotation = privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * min;
                                maxQuotation = privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * max;

                            }
                            else
                            {
                                minQuotation = (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity + 1) * min;
                                maxQuotation = (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity + 1) * max;
                            }

                        }
                        else if (privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren == sellPriceHotel.ServingQuantity)
                        {
                            minQuotation = privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * min;
                            maxQuotation = privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * max;

                        }
                        else
                        {
                            if (privateTourRequest.NumOfAdult % sellPriceHotel.ServingQuantity == 0)
                            {
                                minQuotation = (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * min) +
                                    (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * min) * (privateTourRequest.NumOfChildren * sellPriceHotel.MinSurChange);
                                maxQuotation = privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * max +
                                    (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * max) * (privateTourRequest.NumOfChildren * sellPriceHotel.MaxSurChange);

                            }
                            else
                            {
                                minQuotation = (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity + 1) * min +
                                    (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * min) * (privateTourRequest.NumOfChildren * sellPriceHotel.MinSurChange);
                                maxQuotation = (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity + 1) * max +
                                    (privateTourRequest.NumOfAdult / sellPriceHotel.ServingQuantity * max) * (privateTourRequest.NumOfChildren * sellPriceHotel.MaxSurChange);
                            }
                        }
                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                            QuantityOfChild = privateTourRequest.NumOfChildren,
                            MinPrice = minQuotation,
                            MaxPrice = maxQuotation,
                            ServiceRatingId = serviceRating!.Id,
                            StartDate = hotel.StartDate,
                            EndDate = hotel.EndDate,

                        });
                    }
                    foreach (var restaurent in location.Restaurants)
                    {
                        var serviceRating = await serviceRatingRepository!.GetByExpression(a => a.ServiceTypeId == ServiceType.RESTAURANTS && a.Rating == restaurent.Rating);
                        var sellPriceRestaurent = estimate.RestaurantPrice.DetailedPriceReferences.Where(a => a.ServiceRating.ServiceTypeId == ServiceType.RESTAURANTS &&
                        a.ServiceRating.Rating == restaurent.Rating && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.BOTH).FirstOrDefault();

                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                            QuantityOfChild = privateTourRequest.NumOfChildren,
                            MinPrice = totalPeople * sellPriceRestaurent!.MinPrice,
                            MaxPrice = totalPeople * sellPriceRestaurent!.MaxPrice,
                            ServiceRatingId = serviceRating!.Id,
                            StartDate = restaurent.StartDate,
                            EndDate = restaurent.EndDate,
                        });

                    }



                    var serviceEntertaimentRating = await serviceRatingRepository!.GetByExpression(a => a.ServiceTypeId == ServiceType.ENTERTAINMENT);
                    var sellPriceAdultEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.ServiceRating.ServiceTypeId == ServiceType.ENTERTAINMENT 

                && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.ADULT).FirstOrDefault();
                    var sellPriceChildrenEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.ServiceRating.ServiceTypeId == ServiceType.ENTERTAINMENT

                  && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.CHILD).FirstOrDefault();

                    double minQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MinPrice) + privateTourRequest.NumOfChildren * sellPriceChildrenEntertainment.MinPrice;
                    double maxQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MaxPrice) + privateTourRequest.NumOfChildren * sellPriceChildrenEntertainment.MaxPrice;

                    quotationDetails.Add(new QuotationDetail
                    {
                        Id = Guid.NewGuid(),
                        OptionQuotationId = option.Id,
                        QuantityOfAdult = privateTourRequest.NumOfAdult,
                        QuantityOfChild = privateTourRequest.NumOfChildren,
                        MinPrice = minQuotationEntertaiment,
                        MaxPrice = maxQuotationEntertaiment,
                        ServiceRatingId = serviceEntertaimentRating!.Id,
                        StartDate = null,
                        EndDate = null,
                    });

                
                }

                foreach (var vehicle in dto.Vehicles)
                {
                    var price = await referenceTransportPriceRepository!.GetAllDataByExpression(a => a.Departure!.Commune!.District!.ProvinceId == vehicle.StartPoint && a.Arrival.Commune.District.ProvinceId == vehicle.EndPoint
                    || a.Departure.Commune.District.ProvinceId == vehicle.EndPoint && a.Arrival!.Commune!.District!.ProvinceId == vehicle.StartPoint
                    , 0, 0, null
                    );
                    if (price.Items!.Any())
                    {
                        vehicleQuotationDetails.Add(new VehicleQuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                            MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                            StartPointId = vehicle.StartPoint,
                            EndPointId= vehicle.EndPoint,
                            VehicleType = vehicle.VehicleType,
                        });

                    }
                }

                    option.MinTotal = quotationDetails.Min(a => a.MinPrice) + vehicleQuotationDetails.Min(a=> a.MinPrice);
                option.MaxTotal = quotationDetails.Min(a => a.MaxPrice) + vehicleQuotationDetails.Max(a => a.MaxPrice);
                await optionQuotationRepository!.Insert(option);
                await quotationDetailRepository!.InsertRange(quotationDetails);
                await vehicleQuotationDetailRepository!.InsertRange(vehicleQuotationDetails);
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
            //         else if (option.Status != OptionQuotationStatus.NEW)
            //  {
            //             result = BuildAppActionResultError(result,
            //                 $"Chỉ chấp nhận confirm với option trạng thái NEW");
            //       }
            if (travelCompanion == null)
            {
                result = BuildAppActionResultError(result,
                    $"Không thấy thông tin trùng khớp với travel companion");
            }
            if (!BuildAppActionResultIsError(result))
            {
                //    option!.PrivateTourRequest!.Status = PrivateTourStatus.APPROVED;
                //     option!.Status = OptionQuotationStatus.ACTIVE;
                var listOption =
                    await optionQuotationRepository.GetAllDataByExpression(
                        a => a.PrivateTourRequestId == option!.PrivateTourRequestId,
                        0,
                        0,
                        null);
                foreach (var item in listOption.Items!)
                {
                    item.OptionQuotationStatusId = OptionQuotationStatus.IN_ACTIVE;
                }

                await orderRepository!.Insert(new Order()
                {
                    Id = Guid.NewGuid(),
                    //     OrderStatus = OrderStatus.NEW,
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

    public async Task<AppActionResult> GetServiceRatingListByServiceType(Guid provinceId, Domain.Enum.ServiceType serviceTypeId)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var districtRepository = Resolve<IRepository<District>>();
            var communeRepository = Resolve<IRepository<Commune>>();
            var districtListDb = await districtRepository.GetAllDataByExpression(d => d.ProvinceId == provinceId, 0, 0);
            var communeListDb = await communeRepository.GetAllDataByExpression(c => districtListDb.Items!.Select(d => d.Id).Contains(c.DistrictId), 0, 0);
            if (communeListDb.Items.Count > 0)
            {
                var communeIds = communeListDb.Items.Select(c => c.Id).ToList();
                var serviceRepository = Resolve<IRepository<Service>>();
                var serviceListDb = await serviceRepository.GetAllDataByExpression(s => communeIds.Contains(s.CommunceId)
                                                                                    && s.ServiceRating.ServiceTypeId == serviceTypeId,
                                                                                    0, 0, s => s.ServiceRating);
                List<ServiceRating> serviceRatings = serviceListDb.Items.Select(x => x.ServiceRating).ToList();
                result.Result = serviceRatings;
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }

    //public async Task<AppActionResult> GetServicePriceRangeOfCommune(Guid provinceId, Guid serviceRatingId, int adultQuantity)
    //{
    //    AppActionResult result = new AppActionResult();
    //    try
    //    {
    //        ServicePriceRangeResponse data = new ServicePriceRangeResponse();
    //        var serviceRepository = Resolve<IRepository<Service>>();
    //        var districtRepository = Resolve<IRepository<District>>();
    //        var communeRepository = Resolve<IRepository<Commune>>();
    //        var districtListDb = await districtRepository!.GetAllDataByExpression(d => d.ProvinceId == provinceId, 0, 0);
    //        var communeListDb = await communeRepository!.GetAllDataByExpression(c => districtListDb.Items!.Select(d => d.Id).Contains(c.DistrictId), 0, 0);
    //        if (communeListDb.Items != null && communeListDb.Items.Count > 0)
    //        {
    //            var communeIds = communeListDb.Items.Select(c => c.Id).ToList();
    //            var serviceListDb = await serviceRepository!.GetAllDataByExpression(s => communeIds.Contains(s.CommunceId)
    //                                                                            && s.ServiceRatingId == serviceRatingId,
    //                                                                            0, 0);
    //            var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
    //            PagedResult<SellPriceHistory> serviceSellPriceListDb = null;
    //            SellPriceHistory serviceSellPriceDb = null;
    //            if(serviceListDb.Items != null && serviceListDb.Items.Count > 0)
    //            {
    //                var serviceRatingRepository = Resolve<IRepository<ServiceRating>>();
    //                data.ServiceRating = await serviceRatingRepository!.GetById(serviceRatingId);
    //            serviceListDb.Items.ForEach(async s =>
    //            {
    //                serviceSellPriceListDb = await sellPriceRepository!.GetAllDataByExpression(sp => sp.ServiceId == s.Id && sp.MOQ <= adultQuantity, 0, 0);
    //                if (serviceSellPriceListDb.Items != null && serviceSellPriceListDb.Items.Count <= 0)
    //                {
    //                    result.Messages.Add($"Không tìm thấy giá của dịch vụ {s.Name} với số lượng tối thiểu {adultQuantity} người");
    //                }
    //                else
    //                {
    //                    serviceSellPriceDb = serviceSellPriceListDb.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault();
    //                    //if (serviceSellPriceDb.PricePerAdult > data.AdultMaxPrice)
    //                    //{
    //                    //    data.AdultMaxPrice = serviceSellPriceDb.PricePerAdult;
    //                    //}
    //                    //else if (serviceSellPriceDb.PricePerAdult < data.AdultMinPrice)
    //                    //{
    //                    //    data.AdultMinPrice = serviceSellPriceDb.PricePerAdult;
    //                    //}

    //                    //if (serviceSellPriceDb.PricePerChild > data.ChildMaxPrice)
    //                    //{
    //                    //    data.ChildMaxPrice = serviceSellPriceDb.PricePerAdult;
    //                    //}
    //                    //else if (serviceSellPriceDb.PricePerChild < data.ChildMinPrice)
    //                    //{
    //                    //    data.ChildMinPrice = serviceSellPriceDb.PricePerChild;
    //                    //}

    //                }
    //            });
    //            }
    //            result.Result = data;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        result = BuildAppActionResultError(result, e.Message);
    //    }
    //    return result;
    //}

    //public async Task<AppActionResult> GetServicePriceRange(Guid provinceId, ServiceType serviceTypeId, Guid requestTourId)
    //{
    //    AppActionResult result = new AppActionResult();
    //    try
    //    {
    //        var tourRequestDb = await _repository.GetById(requestTourId);
    //        if(tourRequestDb == null)
    //        {
    //            result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tour có id {requestTourId}");
    //            return result;
    //        }
    //        List<ServicePriceRangeResponse> data = new List<ServicePriceRangeResponse>();
    //        var serviceRepository = Resolve<IRepository<Service>>();
    //        var serviceRatingRepository = Resolve<IRepository<ServiceRating>>();
    //        var districtRepository = Resolve<IRepository<District>>();
    //        var communeRepository = Resolve<IRepository<Commune>>();
    //        var districtListDb = await districtRepository!.GetAllDataByExpression(d => d.ProvinceId == provinceId, 0, 0);
    //        var communeListDb = await communeRepository!.GetAllDataByExpression(c => districtListDb.Items!.Select(d => d.Id).Contains(c.DistrictId), 0, 0);
    //        if (communeListDb.Items != null && communeListDb.Items.Count > 0)
    //        {
    //            var communeIds = communeListDb.Items.Select(c => c.Id).ToList();
    //            var serviceRatingDb = await serviceRatingRepository!.GetAllDataByExpression(s => s.ServiceTypeId == serviceTypeId,0,0);
    //            var serviceListDb = await serviceRepository!.GetAllDataByExpression(s => communeIds.Contains(s.CommunceId)
    //                                                                            && serviceRatingDb.Items.Select(s => s.Id).Contains(s.ServiceRatingId),
    //                                                                            0, 0);

    //            var groupedServiceList = serviceListDb.Items.GroupBy(s => s.ServiceRatingId)
    //            .ToDictionary(
    //                // Key selector function
    //                group => group.Key,
    //                // Element selector function, you can choose what you want to store in the dictionary
    //                group => group.ToList()
    //            );

    //            var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
    //            PagedResult<SellPriceHistory> serviceSellPriceListDb = null;
    //            SellPriceHistory serviceSellPriceDb = null;

    //            foreach(var kvp in groupedServiceList)
    //            {
    //                ServicePriceRangeResponse serviceRatingRange = new ServicePriceRangeResponse();
    //                serviceRatingRange.ServiceRating = await serviceRatingRepository.GetById(kvp.Key);
    //                if(kvp.Value.Count > 0)
    //                {
    //                    kvp.Value.ForEach(
    //                        async s =>
    //                        {
    //                            serviceSellPriceListDb = await sellPriceRepository!.GetAllDataByExpression(sp => sp.ServiceId == s.Id && sp.MOQ <= tourRequestDb.NumOfAdult, 0, 0);
    //                            if (serviceSellPriceListDb.Items.Count <= 0)
    //                            {
    //                                result.Messages.Add($"Không tìm thấy giá của dịch vụ {s.Name} với số lượng tối thiểu {tourRequestDb.NumOfAdult} người");
    //                            }
    //                            else
    //                            {
    //                                serviceSellPriceDb = serviceSellPriceListDb.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault();
    //                                //if (serviceSellPriceDb.PricePerAdult > serviceRatingRange.AdultMaxPrice)
    //                                //{
    //                                //    serviceRatingRange.AdultMaxPrice = serviceSellPriceDb.PricePerAdult;
    //                                //}
    //                                //else if (serviceSellPriceDb.PricePerAdult < serviceRatingRange.AdultMinPrice)
    //                                //{
    //                                //    serviceRatingRange.AdultMinPrice = serviceSellPriceDb.PricePerAdult;
    //                                //}

    //                                //if (serviceSellPriceDb.PricePerChild > serviceRatingRange.ChildMaxPrice)
    //                                //{
    //                                //    serviceRatingRange.ChildMaxPrice = serviceSellPriceDb.PricePerAdult;
    //                                //}
    //                                //else if (serviceSellPriceDb.PricePerChild < serviceRatingRange.ChildMinPrice)
    //                                //{
    //                                //    serviceRatingRange.ChildMinPrice = serviceSellPriceDb.PricePerChild;
    //                                //}

    //                            }
    //                        }
    //                        );
    //                }
    //                data.Add(serviceRatingRange);
    //            }
    //            result.Result = data;
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        result = BuildAppActionResultError(result, e.Message);
    //    }
    //    return result;
    //}

}