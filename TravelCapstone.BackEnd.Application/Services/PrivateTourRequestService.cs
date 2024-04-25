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
    private IRepository<PrivateTourRequest> _repository;
    private IUnitOfWork _unitOfWork;

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
            if (privateTourequestDTO.OtherLocation != null && privateTourequestDTO.OtherLocation.Count > 0)
            {
                var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
                foreach (var requestedLocationId in privateTourequestDTO.OtherLocation)
                {
                    await requestedLocationRepository!.Insert(new RequestedLocation
                    {
                        Id = Guid.NewGuid(),
                        PrivateTourRequestId = request.Id,
                        ProvinceId = requestedLocationId.ProvinceId,
                        Address = requestedLocationId.Address,
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

    public async Task<AppActionResult> GetAllPrivateTourRequest(int pageNumber, int pageSize)
    {
        var result = new AppActionResult();
        var accountRepository = Resolve<IRepository<Account>>();
        var requestLocationRepository = Resolve<IRepository<RequestedLocation>>();
        try
        {
            var data = await _repository.GetAllDataByExpression
            (null, pageNumber,
                pageSize, a => a.CreateDate, false,
                p => p.Tour, p => p.CreateByAccount!, p => p.Province!);
            var responseList = _mapper.Map<PagedResult<PrivateTourResponseDto>>(data);
            foreach (var item in responseList.Items!)
            {
                var requestLocationDb = await requestLocationRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == item.Id, 0, 0, null, false, r => r.Province!);
                item.OtherLocation = requestLocationDb.Items;
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
        var requestLocationRepository = Resolve<IRepository<RequestedLocation>>();
        try
        {
            var privateTourRequestDb = await _repository.GetByExpression(a => a.Id == id, a => a.CreateByAccount!, a => a.Province!);
            if (privateTourRequestDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour riêng tư với id {id}");
                return result;
            }
            data.PrivateTourResponse = _mapper.Map<PrivateTourResponseDto>(privateTourRequestDb);
            var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            var optionsDb = await optionQuotationRepository!.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0, null, false, null);
            if (optionsDb.Items!.Count != 3)
            {
                result.Messages.Add($"Số lượng lựa chọn hiện tại: {optionsDb.Items.Count} không phù hợp");
            }
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
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, null);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, null);
                option.QuotationDetails = quotationDetailDb.Items!.ToList();
                option.VehicleQuotationDetails = vehicleQuotationDetailDb.Items!.ToList();
                if (item.OptionClassId == OptionClass.ECONOMY)
                    data.Option1 = option;
                else if (item.OptionClassId == OptionClass.MIDDLE)
                    data.Option2 = option;
                else data.Option3 = option;
            }
            List<Province> provinces = new List<Province>();
            var list = await requestLocationRepository!.GetAllDataByExpression(a => a.PrivateTourRequestId == id, 0, 0, null, false, a => a.Province!);
            foreach (var item in list.Items!)
            {
                data.PrivateTourResponse.OtherLocation = list.Items;
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
        var facilityRepository = Resolve<IRepository<Facility>>();
        var facilityRatingRepository = Resolve<IRepository<FacilityRating>>();
        var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
        var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
        var referenceTransportPriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
        var facilityService = Resolve<IFacilityServiceService>();
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



                    var listHotel = await facilityRepository!.GetAllDataByExpression(
                        a => a.Communce!.District!.ProvinceId == location.ProvinceId
                       && a.FacilityRating!.RatingId == hotel.Rating &&
                       a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
                        0,
                        0, null, false,
                        null);
                    var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.FacilityService!.Facility!.Communce!.District!.ProvinceId == location.ProvinceId
                        && a.FacilityService.ServiceTypeId == ServiceType.RESTING
                        && a.FacilityService.Facility.FacilityRating!.RatingId == hotel.Rating
                        ,
                        0, 0, null, false, null
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
                    var listRestaurent = await facilityRepository!.GetAllDataByExpression(
                        a => a.Communce!.District!.ProvinceId == location.ProvinceId
                        && a.FacilityRating!.RatingId == restaurent.Rating &&
                       a.FacilityRating.FacilityTypeId == FacilityType.RESTAURANTS,
                        0,
                        0, null, false,
                        null);
                    var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
                       a => a.FacilityService!.Facility!.Communce!.District!.ProvinceId == location.ProvinceId
                         && a.FacilityService.ServiceTypeId == ServiceType.FOODANDBEVARAGE
                        && a.FacilityService.Facility.FacilityRating!.RatingId == restaurent.Rating,
                       0, 0, null, false, null
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

                var entertaiment = await facilityRepository!.GetAllDataByExpression(
                         a => a.Communce!.District!.ProvinceId == location.ProvinceId &&
                       a.FacilityRating!.FacilityTypeId == FacilityType.ENTERTAINMENT,
                         0,
                         0, null, false,
                         null);
                var sellEntertaiment = await sellPriceRepository!.GetAllDataByExpression(
                     a => a.FacilityService!.Facility!.Communce!.District!.ProvinceId == location.ProvinceId
                       && a.FacilityService.ServiceTypeId == ServiceType.ENTERTAIMENT,
                     0, 0, null, false, null
                     );
                if (entertaiment.Items!.Any())
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ vui chơi giải trí tại tỉnh thuộc id {location.ProvinceId}");
                }
                if (!sellEntertaiment.Items!.Any())
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy giá dịch vụ giải trí tại tỉnh thuộc id {location.ProvinceId}");
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
                    var estimateService = await facilityService!.GetServiceByProvinceIdAndRequestId(location.ProvinceId, dto.PrivateTourRequestId);
                    var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                    foreach (var hotel in location.Hotels)
                    {
                        var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                            a => a.RatingId == hotel.Rating
                            && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == hotel.ServingQuantity
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
                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && a.RatingId == hotel.Rating);
                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                            QuantityOfChild = privateTourRequest.NumOfChildren,
                            MinPrice = minQuotation,
                            MaxPrice = maxQuotation,
                            FacilityRatingId = hotelRating!.Id,
                            StartDate = hotel.StartDate,
                            EndDate = hotel.EndDate,

                        });
                    }
                    foreach (var restaurent in location.Restaurants)
                    {
                        var restaurentRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.RESTAURANTS);
                        var sellPriceRestaurent = estimate.HotelPrice.DetailedPriceReferences.Where(
                          a => a.RatingId == restaurent.Rating
                          && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 10
                          ).FirstOrDefault();
                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                            QuantityOfChild = privateTourRequest.NumOfChildren,
                            MinPrice = totalPeople * sellPriceRestaurent!.MinPrice,
                            MaxPrice = totalPeople * sellPriceRestaurent!.MaxPrice,
                            FacilityRatingId = restaurentRating!.Id,
                            StartDate = restaurent.StartDate,
                            EndDate = restaurent.EndDate,
                        });
                    }
                    var entertaimentRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.ENTERTAINMENT);

                    var sellPriceAdultEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

                && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.ADULT).FirstOrDefault();
                    var sellPriceChildrenEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

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
                        FacilityRatingId = entertaimentRating!.Id,
                        StartDate = null,
                        EndDate = null,
                    });


                }

                foreach (var vehicle in dto.Vehicles)
                {
                    var price = await referenceTransportPriceRepository!.GetAllDataByExpression(a => a.Departure!.Commune!.District!.ProvinceId == vehicle.StartPoint && a.Arrival!.Commune!.District!.ProvinceId == vehicle.EndPoint
                    || a.Departure.Commune.District.ProvinceId == vehicle.EndPoint && a.Arrival!.Commune!.District!.ProvinceId == vehicle.StartPoint
                    , 0, 0, null, false, null
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
                            EndPointId = vehicle.EndPoint,
                            VehicleType = vehicle.VehicleType,
                        });

                    }
                }

                option.MinTotal = quotationDetails.Min(a => a.MinPrice) + vehicleQuotationDetails.Min(a => a.MinPrice);
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
            else if (option.OptionQuotationStatusId != OptionQuotationStatus.NEW)
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
                option!.PrivateTourRequest!.PrivateTourStatusId = PrivateTourStatus.APPROVED;
                option!.OptionQuotationStatusId = OptionQuotationStatus.ACTIVE;
                var listOption =
                    await optionQuotationRepository.GetAllDataByExpression(
                        a => a.PrivateTourRequestId == option!.PrivateTourRequestId,
                        0,
                        0, null, false,
                        null);
                foreach (var item in listOption.Items!)
                {
                    item.OptionQuotationStatusId = OptionQuotationStatus.IN_ACTIVE;
                }

                await orderRepository!.Insert(new Order()
                {
                    Id = Guid.NewGuid(),
                    OrderStatusId = OrderStatus.NEW,
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
        //try
        //{
            
        //        var facilityServiceRepository = Resolve<IRepository<FacilityService>>();
        //        var facilityServiceListDb = await facilityServiceRepository!.GetAllDataByExpression(
        //            s => s.Facility!.Communce!.District!.ProvinceId== provinceId
        //         && s!.ServiceTypeId == serviceTypeId,
        //        0, 0, null, false);
        //        result.Result = facilityServiceListDb;
            
        //}
        //catch (Exception e)
        //{
        //    result = BuildAppActionResultError(result, e.Message);
        //}
        return result;
    }
}