using AutoMapper;
using Bogus;
using EnumsNET;
using NPOI.SS.Formula;
using System.ComponentModel.Design;
using System.Text;
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

            var request = new PrivateTourRequest
            {
                Id = Guid.NewGuid(),
                DietaryPreferenceId = privateTourequestDTO.DietaryPreference,
                PrivateTourStatusId = PrivateTourStatus.NEW,
                StartDate = privateTourequestDTO.StartDate,
                EndDate = privateTourequestDTO.EndDate,
                Description = privateTourequestDTO.Description,
                NumOfAdult = privateTourequestDTO.NumOfAdult,
                NumOfChildren = privateTourequestDTO.NumOfChildren,
                NumOfDay = privateTourequestDTO.NumOfDay,
                NumOfNight = privateTourequestDTO.NumOfNight,
                TourId = privateTourequestDTO.TourId,
                IsEnterprise = privateTourequestDTO.IsEnterprise,
                Note = privateTourequestDTO.Note,
                RecommendedTourUrl = privateTourequestDTO.RecommnendedTourUrl,
                StartLocation = privateTourequestDTO.StartLocation,
                StartLocationCommuneId = privateTourequestDTO.StartCommuneId,
                WishPrice = privateTourequestDTO.WishPrice,
                MainDestinationId = privateTourequestDTO.MainDestinationId,
                CreateBy = privateTourequestDTO.AccountId
            };
            var utility = Resolve<Utility>();
            request.CreateDate = utility!.GetCurrentDateTimeInTimeZone();

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
            var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
            var requestedLocationDb = await requestedLocationRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == privateTourRequestDb.Id, 0, 0, null, false, r => r.Province!);
            data.PrivateTourResponse.OtherLocation = requestedLocationDb.Items;
            var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            var optionsDb = await optionQuotationRepository!.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0, null, false, q => q.PrivateTourRequest.Commune.District.Province!, q => q.PrivateTourRequest.Province!);

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
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, q => q.District, q => q.FacilityRating.Rating);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, q => q.EndPoint,p => p.StartPoint, p => p.EndPointDistrict, p => p.StartPointDistrict);
                 
                option.QuotationDetails = quotationDetailDb.Items!.ToList();
                option.VehicleQuotationDetails = await GetOrderedList(vehicleQuotationDetailDb.Items!.ToList());
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

    private async Task<List<VehicleQuotationDetail>?> GetOrderedList(List<VehicleQuotationDetail> vehicleQuotationDetails)
    {
        List<VehicleQuotationDetail> result = null;
        try
        {
            if(vehicleQuotationDetails == null || vehicleQuotationDetails.Count == 0)
            {
                return result;
            }
            result = new List<VehicleQuotationDetail>();
            var tourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
            var optionRepository = Resolve<IRepository<OptionQuotation>>();
            var optionDb = await optionRepository!.GetByExpression(o => vehicleQuotationDetails[0].OptionQuotationId == o.Id);
            if(optionDb != null)
            {
                var tourRequestDb = await tourRequestRepository!.GetByExpression(t => t.Id == optionDb.PrivateTourRequestId, t => t.Commune.District!);
                if(tourRequestDb != null)
                {
                    Guid start = tourRequestDb.Commune.District!.ProvinceId;
                    List<VehicleQuotationDetail> current = null;
                    while(vehicleQuotationDetails.Count > 0)
                    {
                        current = vehicleQuotationDetails.Where(v => v.StartPointId == start).ToList();
                        if(current.Count == 0) {
                            break;
                        } else if(current.Count == 1)
                        {
                            result.Add(current[0]);
                            vehicleQuotationDetails.Remove(current[0]);
                            start = current[0].EndPointId;
                            current.Remove(current[0]);
                        } else
                        {
                            var inProvince = current.Where(v => v.EndPointId == null).ToList();
                            while(inProvince.Count > 0)
                            {
                                result.Add(inProvince[0]);
                                inProvince.Remove(inProvince[0]);
                                current.Remove(inProvince[0]);
                            }
                            inProvince = current.Where(v => v.EndPointId == v.StartPointId).ToList();
                            if (inProvince.Count > 1)
                            {
                                var listEndDistrictIds = inProvince.Select(i => i.EndPointDistrictId).ToList();
                                var startDistrict = inProvince.Where(i => !listEndDistrictIds.Contains(i.StartPointDistrictId)).FirstOrDefault()!.StartPointDistrictId;
                                VehicleQuotationDetail innerCurrent = null; 
                                while(inProvince.Count > 0)
                                {
                                    innerCurrent = inProvince.Where(i => i.StartPointDistrictId == startDistrict).FirstOrDefault()!;
                                    result.Add(innerCurrent);
                                    inProvince.Remove(innerCurrent);
                                    vehicleQuotationDetails.Remove(innerCurrent);
                                    startDistrict = (Guid)innerCurrent.EndPointDistrictId;
                                }
                            } else
                            {
                                result.Add(inProvince[0]);
                                vehicleQuotationDetails.Remove(inProvince[0]);
                            }
                        }
                    }

                }
            }
            

        } catch(Exception e)
        {
            
        }
        return result;
    }

    public async Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto)
    {
        var result = new AppActionResult();
        var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
        var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
        var districtRepository = Resolve<IRepository<District>>();
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
                var district = await districtRepository!.GetById(location.DistrictId);
                if (district == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
                }
                foreach (var hotel in location.Hotels)
                {
                    var listHotel = await facilityRepository!.GetAllDataByExpression(
                        a => a.Communce!.DistrictId == location.DistrictId
                       && hotel.Rating == a.FacilityRating!.RatingId &&
                       a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
                        0,
                        0, null, false,
                        null);
                    var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                        a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                        && a.FacilityService.ServiceTypeId == ServiceType.RESTING
                        && hotel.Rating == a.FacilityService.Facility.FacilityRating!.RatingId
                        ,
                        0, 0, null, false, null
                        );
                    if (!listHotel.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {hotel.Rating.ToString()} tại huyện {district.Name}");
                    }
                    if (!sellHotel.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {hotel.Rating.ToString()} tại huyện {district.Name}");
                    }
                }
                foreach (var restaurent in location.Restaurants)
                {
                    var listRestaurent = await facilityRepository!.GetAllDataByExpression(
                        a => a.Communce!.DistrictId == location.DistrictId
                        && restaurent.Rating == a.FacilityRating!.RatingId &&
                       (a.FacilityRating.FacilityTypeId == FacilityType.RESTAURANTS
                       || a.FacilityRating.FacilityTypeId == FacilityType.HOTEL),
                        0,
                        0, null, false,
                        null);
                    var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
                       a => a.Menu.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                         && a.Menu.FacilityService.ServiceTypeId == ServiceType.FOODANDBEVARAGE
                        && restaurent.Rating == a.Menu.FacilityService.Facility.FacilityRating!.RatingId,
                       0, 0, null, false, null
                       );
                    if (!listRestaurent.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy nhà hàng với phân loại {restaurent.Rating.ToString()} tại huyện {district.Name}");
                    }
                    if (!sellRestaurent.Items!.Any())
                    {

                        result = BuildAppActionResultError(result, $"Không tìm thấy giá nhà hàng với phân loại {restaurent.Rating.ToString()} tại huyện {district.Name}");
                    }
                }

                if (location.Entertainment != null)
                {
                    var entertaiment = await facilityRepository!.GetAllDataByExpression(
                         a => a.Communce!.DistrictId == location.DistrictId &&
                       a.FacilityRating!.FacilityTypeId == FacilityType.ENTERTAINMENT,
                         0,
                         0, null, false,
                         null);
                    var sellEntertaiment = await sellPriceRepository!.GetAllDataByExpression(
                         a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                           && a.FacilityService.ServiceTypeId == ServiceType.ENTERTAIMENT,
                         0, 0, null, false, null
                         );
                    if (!entertaiment.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ vui chơi giải trí tại huyện {district.Name}");
                    }
                    if (!sellEntertaiment.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy giá dịch vụ giải trí tại huyện {district.Name}");
                    }
                }
            }
            if (!BuildAppActionResultIsError(result))
            {
                var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
                var totalPeople = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren;
                //Calculate based on fixed formula: Children price = 70% Adult price
                // => NumOfAdult * Price + NumOfChildren * 0.7 * Price = Total
                // => Price = Total / (NumOfAdult + NumOfChildren * 0.7)
                double AdultBasedQuantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren * 0.7;

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
                    var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
                    var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                    foreach (var hotel in location.Hotels)
                    {
                        var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                            a => hotel.Rating == a.RatingId
                            && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == hotel.ServingQuantity
                            ).FirstOrDefault();
                        if(sellPriceHotel != null)
                        {
                            double min = sellPriceHotel!.MinPrice;

                            double max = sellPriceHotel!.MaxPrice;
                            double minQuotation = hotel.NumOfDay * hotel.NumOfRoom * min;
                            double maxQuotation = hotel.NumOfDay * hotel.NumOfRoom * max;
                            var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && hotel.Rating == a.RatingId);
                            quotationDetails.Add(new QuotationDetail
                            {
                                Id = Guid.NewGuid(),
                                OptionQuotationId = option.Id,
                                QuantityOfAdult = privateTourRequest.NumOfAdult,
                                QuantityOfChild = privateTourRequest.NumOfChildren,
                                ServingQuantity = hotel.ServingQuantity,
                                Quantity = hotel.NumOfRoom,
                                MinPrice = minQuotation,
                                MaxPrice = maxQuotation,
                                FacilityRatingId = hotelRating!.Id,
                                StartDate = hotel.StartDate,
                                EndDate = hotel.EndDate,
                                DistrictId = location.DistrictId
                            });
                        }
                        else
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng {hotel.ServingQuantity} người với phân loại {hotel.Rating}");
                        }
                    }
                    foreach (var restaurent in location.Restaurants)
                    {
                        var restaurentRating = await facilityRatingRepository!.GetByExpression(a => restaurent.Rating == a.RatingId);
                        var sellPriceRestaurent = estimate.RestaurantPrice.DetailedPriceReferences.Where(
                          a => restaurent.Rating == a.RatingId
                          && a.ServiceAvailability == restaurent.ServiceAvailability && a.ServingQuantity == restaurent.ServingQuantity
                          ).FirstOrDefault();
                        if(sellPriceRestaurent != null)
                        {
                            int total = restaurent.ServiceAvailability == ServiceAvailability.BOTH ? privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren :
                                              restaurent.ServiceAvailability == ServiceAvailability.ADULT ? privateTourRequest.NumOfAdult : privateTourRequest.NumOfChildren;
                            int quantity = (int)Math.Ceiling((double)(total) / restaurent.ServingQuantity);
                            quotationDetails.Add(new QuotationDetail
                            {
                                Id = Guid.NewGuid(),
                                OptionQuotationId = option.Id,
                                QuantityOfAdult = (restaurent.ServiceAvailability == ServiceAvailability.BOTH || restaurent.ServiceAvailability == ServiceAvailability.ADULT)? privateTourRequest.NumOfAdult : 0,
                                QuantityOfChild = (restaurent.ServiceAvailability == ServiceAvailability.BOTH || restaurent.ServiceAvailability == ServiceAvailability.CHILD) ? privateTourRequest.NumOfChildren : 0,
                                ServingQuantity = 10,
                                MealPerDay = restaurent.MealPerDay,
                                Quantity = quantity,
                                MinPrice = quantity * sellPriceRestaurent!.MinPrice * restaurent.MealPerDay,
                                MaxPrice = quantity * sellPriceRestaurent!.MaxPrice * restaurent.MealPerDay,
                                FacilityRatingId = restaurentRating!.Id,
                                StartDate = restaurent.StartDate,
                                EndDate = restaurent.EndDate,
                                DistrictId = location.DistrictId
                            });
                        } else
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy báo giá ăn uống tại huyện có id {location.DistrictId} tại nhà hàng {restaurent.Rating} cho dịch vụ {restaurent.ServiceAvailability} {restaurent.ServingQuantity} người ");
                            return result;
                        }
                    }
                   if(estimate.EntertainmentPrice.DetailedPriceReferences.Count > 0)
                    {
                        var entertaimentRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.ENTERTAINMENT);

                        var sellPriceAdultEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

                    && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.ADULT).FirstOrDefault();
                        var sellPriceChildrenEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

                      && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.CHILD).FirstOrDefault();

                        double minQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MinPrice);
                        minQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MinPrice);
                        minQuotationEntertaiment *= location.Entertainment.QuantityLocation;

                        double maxQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MaxPrice);
                        maxQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MaxPrice);
                        maxQuotationEntertaiment *= location.Entertainment.QuantityLocation;


                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            OptionQuotationId = option.Id,
                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                            QuantityOfChild = privateTourRequest.NumOfChildren,
                            ServingQuantity = 1,
                            Quantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren,
                            MinPrice = minQuotationEntertaiment,
                            MaxPrice = maxQuotationEntertaiment,
                            FacilityRatingId = entertaimentRating!.Id,
                            StartDate = null,
                            EndDate = null,
                            DistrictId = location.DistrictId
                        });
                    }


                }

                foreach (var vehicle in dto.Vehicles)
                {
                    if (vehicle.VehicleType == VehicleType.PLANE || vehicle.VehicleType == VehicleType.BOAT)
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
                                NumOfRentingDay = 1,
                                NumOfVehicle = 1,
                                StartPointId = (Guid)vehicle.StartPoint,
                                EndPointId = (Guid)vehicle.EndPoint,
                                StartPointDistrictId = vehicle.StartPointDistrict != null? (Guid)vehicle.StartPointDistrict : null,
                                EndPointDistrictId = vehicle.EndPointDistrict != null? (Guid)vehicle.EndPointDistrict : null,
                                VehicleType = vehicle.VehicleType,
                            });

                        }
                    }
                    else
                    {
                        var price = await sellPriceRepository!.GetAllDataByExpression(s => s.TransportServiceDetail.FacilityService.Facility.Communce.District.ProvinceId == vehicle.StartPoint
                        && s.TransportServiceDetail.VehicleTypeId == vehicle.VehicleType, 0, 0, null, false, s => s.TransportServiceDetail.FacilityService);
                        if (price.Items!.Any())
                        {
                            int totalVehicle = (int)Math.Ceiling((decimal)((decimal)(privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren) / price.Items[0].TransportServiceDetail.FacilityService.ServingQuantity));

                            vehicleQuotationDetails.Add(new VehicleQuotationDetail
                            {
                                Id = Guid.NewGuid(),
                                OptionQuotationId = option.Id,
                                MinPrice = totalVehicle * vehicle.NumOfRentingDay * price.Items!.Min(a => a.Price),
                                MaxPrice = totalVehicle * vehicle.NumOfRentingDay * price.Items!.Max(a => a.Price),
                                NumOfRentingDay = vehicle.NumOfRentingDay,
                                NumOfVehicle = vehicle.NumOfVehicle,
                                StartPointId = (Guid)vehicle.StartPoint,
                                EndPointId = (Guid)vehicle.EndPoint,
                                StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                VehicleType = vehicle.VehicleType
                            });
                        }
                    }


                }
                quotationDetails.ForEach(q =>
                    {
                        option.MinTotal += q.MinPrice;
                        option.MaxTotal += q.MaxPrice;

                    });
                vehicleQuotationDetails.ForEach(q =>
                {
                    option.MinTotal += q.MinPrice;
                    option.MaxTotal += q.MaxPrice;

                });


                option.MinTotal = Math.Ceiling(option.MinTotal / (totalPeople * 1000)) * 1000;
                option.MaxTotal = Math.Ceiling(option.MaxTotal / (totalPeople * 1000)) * 1000;

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
                    PrivateTourRequestId = option.PrivateTourRequestId,
                    Content = "Tạo order tour thành công"
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

            var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
            var facilityServiceListDb = await facilityServiceRepository!.GetAllDataByExpression(
                s => s.Facility!.Communce!.District!.ProvinceId == provinceId
             && s!.ServiceTypeId == serviceTypeId,
            0, 0, null, false);
            result.Result = facilityServiceListDb;

        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }

    public async Task<AppActionResult> SendToCustomer(Guid privateTourRequestId)
    {
        AppActionResult result = new();
        try
        {
            var privateTourRequest = await _repository.GetById(privateTourRequestId);
            if( privateTourRequest == null )
            {
                result = BuildAppActionResultError(result, $"Yêu cầu tạo tour riêng tư với id {privateTourRequestId} không tồn tại");
            }
            else if(privateTourRequest.PrivateTourStatusId !=PrivateTourStatus.NEW ){ 
                result = BuildAppActionResultError(result, $"Chức năng này chỉ hỗ trợ yêu cầu tạo tour riêng tư với status new với id {privateTourRequestId}");

            }
            if (!BuildAppActionResultIsError(result))
            {
                privateTourRequest!.PrivateTourStatusId = PrivateTourStatus.WAITINGFORCUSTOMER;
                await _repository.Update(privateTourRequest);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception e) {
            result = BuildAppActionResultError(result, e.Message);
        }
        return result;
    }
}