using AutoMapper;
using Bogus;
using EnumsNET;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                CreateBy = privateTourequestDTO.AccountId!,
                MinimumHotelRatingId = privateTourequestDTO.MinimumHotelRatingId,
                MinimumRestaurantRatingId = privateTourequestDTO.MinimumRestaurantRatingId, 
            };
            var utility = Resolve<Utility>();
            request.CreateDate = utility!.GetCurrentDateTimeInTimeZone();

            var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
            await requestedLocationRepository!.Insert(new RequestedLocation
            {
                Id = Guid.NewGuid(),
                PrivateTourRequestId = request.Id,
                ProvinceId = privateTourequestDTO.MainDestinationId,
            });

            await _repository.Insert(request);
            if (privateTourequestDTO.OtherLocation != null && privateTourequestDTO.OtherLocation.Count > 0)
            {
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

            if (privateTourequestDTO.RoomQuantityDetailRequest != null && privateTourequestDTO.RoomQuantityDetailRequest.Count > 0)
            {
                var roomQuantityDetailRepository = Resolve<IRepository<RoomQuantityDetail>>();
                foreach (var detail in privateTourequestDTO.RoomQuantityDetailRequest)
                {
                    await roomQuantityDetailRepository!.Insert(new RoomQuantityDetail
                    {
                        Id = Guid.NewGuid(),
                        PrivateTourRequestId = request.Id,
                        QuantityPerRoom = detail.QuantityPerRoom,
                        TotalRoom = detail.TotalRoom,
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
        var roomQuantityDetailRepository = Resolve<IRepository<RoomQuantityDetail>>();
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

                var roomQuantityDetailDb = await roomQuantityDetailRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == item.Id, 0, 0, null, false, null);
                item.RoomDetails = roomQuantityDetailDb.Items;
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
        var communeRepository = Resolve<IRepository<Commune>>();
        try
        {
            var privateTourRequestDb = await _repository.GetByExpression(a => a.Id == id, a => a.CreateByAccount!, a => a.Province!, a => a.HotelFacilityRating, a => a.RestaurantFacilityRating);
            if (privateTourRequestDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour riêng tư với id {id}");
                return result;
            }
            data.PrivateTourResponse = _mapper.Map<PrivateTourResponseDto>(privateTourRequestDb);
            data.PrivateTourResponse.StartLocationCommune = await communeRepository!.GetByExpression(c => c.Id == privateTourRequestDb.StartLocationCommuneId, c => c.District.Province);
            var requestedLocationRepository = Resolve<IRepository<RequestedLocation>>();
            var roomQuantityDetailRepository = Resolve<IRepository<RoomQuantityDetail>>();
            var requestedLocationDb = await requestedLocationRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == privateTourRequestDb.Id, 0, 0, null, false, r => r.Province!);
            data.PrivateTourResponse.OtherLocation = requestedLocationDb.Items;
            var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            var optionEventRepository = Resolve<IRepository<OptionEvent>>();
            var tourguideQuotationDetailRepository = Resolve<IRepository<TourguideQuotationDetail>>();
            var optionsDb = await optionQuotationRepository!.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0, null, false, q => q.PrivateTourRequest!.Commune!.District!.Province!, q => q.PrivateTourRequest!.Province!);

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
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false,a => a.District!.Province!, a => a.ServiceType!, a=> a.FacilityRating!.FacilityType!, a => a.FacilityRating!.Rating!);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a=> a.StartPoint!, a => a.EndPoint!);
                var tourGuideQuotationDetailDb = await tourguideQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionId == item.Id, 0, 0, null, false, q => q.Province!);
                var eventOptionDb = await optionEventRepository!.GetAllDataByExpression(o => o.OptionId == item.Id, 0, 0, null, false, o => o.Event);
                option.OptionEvent = eventOptionDb.Items;
                option.TourguideQuotationDetails = tourGuideQuotationDetailDb.Items;
                option.QuotationDetails = quotationDetailDb.Items!.ToList();
                option.VehicleQuotationDetails = await GetOrderedList(vehicleQuotationDetailDb.Items!.ToList());
                if (item.OptionClassId == OptionClass.ECONOMY)
                    data.Option1 = option;
                else if (item.OptionClassId == OptionClass.MIDDLE)
                    data.Option2 = option;
                else data.Option3 = option;

            }

            var locationList = await requestLocationRepository!.GetAllDataByExpression(a => a.PrivateTourRequestId == id, 0, 0, null, false, a => a.Province!);
            if(locationList.Items != null && locationList.Items.Count > 0)
                data.PrivateTourResponse.OtherLocation = locationList.Items;

            var roomDetails = await roomQuantityDetailRepository!.GetAllDataByExpression(a => a.PrivateTourRequestId == id, 0, 0, null, false, null);
            if (roomDetails.Items != null && roomDetails.Items.Count > 0)
                data.PrivateTourResponse.RoomDetails = roomDetails.Items;

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
            if (vehicleQuotationDetails == null || vehicleQuotationDetails.Count == 0)
            {
                return result;
            }
            result = new List<VehicleQuotationDetail>();
            var tourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
            var optionRepository = Resolve<IRepository<OptionQuotation>>();
            var optionDb = await optionRepository!.GetByExpression(o => vehicleQuotationDetails[0].OptionQuotationId == o.Id);
            if (optionDb != null)
            {
                var tourRequestDb = await tourRequestRepository!.GetByExpression(t => t.Id == optionDb.PrivateTourRequestId, t => t.Commune!.District!);
                if (tourRequestDb != null)
                {
                    Guid? start = tourRequestDb.Commune!.District!.ProvinceId;
                    List<VehicleQuotationDetail> current = null;
                    while (vehicleQuotationDetails.Count > 0)
                    {
                        current = vehicleQuotationDetails.Where(v => v.StartPointId == start).ToList();
                        if (current.Count == 0)
                        {
                            break;
                        }
                        else if (current.Count == 1)
                        {
                            result.Add(current[0]);
                            vehicleQuotationDetails.Remove(current[0]);
                            if (current[0].EndPointId != null) start = current[0].EndPointId;
                        }
                        else
                        {

                            var inProvince = current.Where(v => v.EndPointId == null).ToList();
                            while(inProvince.Count > 0)
                            {
                                result.Add(inProvince[0]);
                                vehicleQuotationDetails.Remove(inProvince[0]);
                                inProvince.Remove(inProvince[0]);
                            }
                            inProvince = current.Where(v => v.EndPointId == v.StartPointId).ToList();
                            if (inProvince.Count > 1)
                            {
                                var listEndDistrictIds = inProvince.Select(i => i.EndPointDistrictId).ToList();
                                var startDistrict = inProvince.Where(i => !listEndDistrictIds.Contains(i.StartPointDistrictId)).FirstOrDefault()!.StartPointId;
                                VehicleQuotationDetail innerCurrent = null;
                                while (inProvince.Count > 0)
                                {
                                    innerCurrent = inProvince.Where(i => i.StartPointId == startDistrict).FirstOrDefault()!;
                                    result.Add(innerCurrent);
                                    inProvince.Remove(innerCurrent);
                                    vehicleQuotationDetails.Remove(innerCurrent);
                                    startDistrict = (Guid)innerCurrent.EndPointDistrictId!;
                                }
                            }
                            else
                            {
                                result.Add(inProvince[0]);
                                vehicleQuotationDetails.Remove(inProvince[0]);
                            }
                        }
                    }

                }
            }


        }
        catch (Exception e)
        {
            result = null;
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
        var materialPriceHistoryRepository = Resolve<IRepository<MaterialPriceHistory>>();
        var eventRepository = Resolve<IRepository<Event>>();
        var eventOptionRepository = Resolve<IRepository<OptionEvent>>();
        var eventDetailPriceRepository = Resolve<IRepository<EventDetailPriceHistory>>();
        var tourguideQuotationDetailRepository = Resolve<IRepository<TourguideQuotationDetail>>();
        var facilityService = Resolve<IFacilityServiceService>();
        var humanResourceFeeService = Resolve<IHumanResourceFeeService>();
        try
        {
            var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
            if (privateTourRequest == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {dto.PrivateTourRequestId}");
                return result;
            }
            var totalPeople = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren;
            if (dto.ContingencyFee <= 0)
            {
                result = BuildAppActionResultError(result, $"Phí dự phòng/người phải lớn hơn 0");
                return result;
            }

            if (dto.OrganizationCost <= 0)
            {
                result = BuildAppActionResultError(result, $"Phí tổ chức phải lớn hơn 0");
                return result;
            }

            if (dto.EscortFee <= 0)
            {
                result = BuildAppActionResultError(result, $"Phí dẫn đường phải lớn hơn 0");
                return result;
            }

            if (dto.OperatingFee <= 0)
            {
                result = BuildAppActionResultError(result, $"Phí vận hành phải lớn hơn 0");
                return result;
            }

            if(dto.TourGuideCosts.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Cần thêm báo giá về lương hướng dẫn viên");
                return result;
            }

            if (dto.MaterialCosts.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Cần thêm báo giá về chi phí khác");
                return result;
            }

            double materialTotal = 0;
            List<MaterialPriceHistory> materialPriceHistories = new List<MaterialPriceHistory>();
            foreach(var material in dto.MaterialCosts)
            {
                var materialPrice = await materialPriceHistoryRepository!.GetById(material.MaterialPriceHistoryId);
                if(materialPrice == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy báo giá về vật liệu có id {material.MaterialPriceHistoryId}");
                    return result;
                }
                materialPriceHistories.Add(materialPrice );
                materialTotal += material.Quantity * materialPrice.Price;
            }
            int errorCount = 0;
            var menuRepository = Resolve<IRepository<Menu>>();
            foreach (var item in dto.provinceServices)
            {
                District district = null;
                foreach (var location in item.Hotels)
                {
                    district = await districtRepository!.GetById(location.DistrictId);
                    if (district == null)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
                        return result;
                    }
                   if(location.HotelOptionRatingOption1 != null)
                    {
                        var listHotel = await facilityRepository!.GetAllDataByExpression(
                           a => a.Communce!.DistrictId == location.DistrictId
                          && location.HotelOptionRatingOption1 == a.FacilityRating!.RatingId &&
                          a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
                           0,
                           0, null, false,
                           null);
                        var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                            a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                            && a.FacilityService.ServiceTypeId == ServiceType.RESTING
                            && location.HotelOptionRatingOption1 == a.FacilityService.Facility.FacilityRating!.RatingId
                            ,
                            0, 0, null, false, null
                            );
                        if (!listHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {location.HotelOptionRatingOption1} tại huyện {district!.Name}");
                            errorCount++;
                            break;
                        }
                        if (!sellHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {location.HotelOptionRatingOption1} tại huyện {district!.Name}");
                            errorCount++;
                            break;
                        }
                    }

                   if(location.HotelOptionRatingOption2 != null)
                    {
                        var listHotel = await facilityRepository!.GetAllDataByExpression(
                           a => a.Communce!.DistrictId == location.DistrictId
                          && location.HotelOptionRatingOption2 == a.FacilityRating!.RatingId &&
                          a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
                           0,
                           0, null, false,
                           null);
                        var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                            a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                            && a.FacilityService.ServiceTypeId == ServiceType.RESTING
                            && location.HotelOptionRatingOption2 == a.FacilityService.Facility.FacilityRating!.RatingId
                            ,
                            0, 0, null, false, null
                            );
                        if (!listHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {location.HotelOptionRatingOption2} tại huyện {district.Name}");
                            errorCount++;
                            break;
                        }
                        if (!sellHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {location.HotelOptionRatingOption2} tại huyện {district.Name}");
                            errorCount++;
                            break;
                        }
                    }

                    if (location.HotelOptionRatingOption3 != null)
                    {
                        var listHotel = await facilityRepository!.GetAllDataByExpression(
                           a => a.Communce!.DistrictId == location.DistrictId
                          && location.HotelOptionRatingOption3 == a.FacilityRating!.RatingId &&
                          a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
                           0,
                           0, null, false,
                           null);
                        var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
                            a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
                            && a.FacilityService.ServiceTypeId == ServiceType.RESTING
                            && location.HotelOptionRatingOption3 == a.FacilityService.Facility.FacilityRating!.RatingId
                            ,
                            0, 0, null, false, null
                            );
                        if (!listHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {location.HotelOptionRatingOption3} tại huyện {district.Name}");
                            errorCount++;
                            break;
                        }
                        if (!sellHotel.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {location.HotelOptionRatingOption3} tại huyện {district.Name}");
                            errorCount++;
                            break;
                        }
                    }

                }

                if (errorCount > 0)
                {
                    break;
                }
                foreach (var location in item.Restaurants)
                {
                    district = await districtRepository!.GetById(location.DistrictId);
                    if (district == null)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
                    }
                   foreach(var menuOption in location.MenuQuotations)
                    {
                        var menu = await menuRepository!.GetByExpression(m => m.Id == menuOption.MenuId, null);
                        if (menu == null)
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy menu với id {menuOption.MenuId}");
                            errorCount++;
                            break;
                        }
                        var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
                           s => s.MenuId == menuOption.MenuId,
                           0, 0, null, false, null
                           );

                        if (!sellRestaurent.Items!.Any())
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy menu {menu.Name}");
                            errorCount++;
                            break;
                        }
                    }
                    if(errorCount > 0)
                    {
                        break;
                    }
                    
                }

                if (errorCount > 0)
                {
                    break;
                }

                foreach (var location in item.Entertainments)
                {
                    district = await districtRepository!.GetById(location.DistrictId);
                    if (district == null)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
                    }
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
                        errorCount++;
                        break;
                    }
                    if (!sellEntertaiment.Items!.Any())
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy giá dịch vụ giải trí tại huyện {district.Name}");
                        errorCount++;
                        break;
                    }
                }


            }

            if (errorCount == 0)
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                Province province = null;
                foreach (var item in dto.Vehicles)
                {
                    if (item.StartPoint != null)
                    {
                        if ((await provinceRepository!.GetById(item.StartPoint)) == null)
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy tỉnh với id {item.StartPoint}");
                            break;
                        }
                    }
                    if (item.EndPoint != null)
                    {
                        if ((await provinceRepository!.GetById(item.EndPoint)) == null)
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy tỉnh với id {item.EndPoint}");
                            break;
                        }
                    }

                    if (item.StartPointDistrict != null)
                    {
                        if ((await districtRepository!.GetById(item.StartPointDistrict!)) == null)
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {item.StartPointDistrict}");
                            break;
                        }
                    }

                    if (item.EndPointDistrict != null)
                    {
                        if ((await districtRepository!.GetById(item.EndPointDistrict!)) == null)
                        {
                            result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {item.EndPointDistrict}");
                            break;
                        }
                    }
                }
            }

            if (!BuildAppActionResultIsError(result))
            {
                
                //Calculate based on fixed formula: Children price = 70% Adult price
                // => NumOfAdult * Price + NumOfChildren * 0.7 * Price = Total
                // => Price = Total / (NumOfAdult + NumOfChildren * 0.7)
                double AdultBasedQuantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren * 0.7;

                OptionQuotation option1 = new OptionQuotation
                {
                    Id = Guid.NewGuid(),
                    MinTotal = 0,
                    MaxTotal = 0,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    EscortFee = dto.EscortFee,
                    OrganizationCost = dto.OrganizationCost,
                    OperatingFee = dto.OperatingFee,
                    ContingencyFee = dto.ContingencyFee,
                    AssurancePriceHistoryId = dto.AssurancePriceHistoryOptionId,
                    OptionClassId = OptionClass.ECONOMY,
                    OptionQuotationStatusId = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId
                };

                OptionQuotation option2 = new OptionQuotation
                {
                    Id = Guid.NewGuid(),
                    MinTotal = 0,
                    MaxTotal = 0,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    EscortFee = dto.EscortFee,
                    OrganizationCost = dto.OrganizationCost,
                    OperatingFee = dto.OperatingFee,
                    ContingencyFee = dto.ContingencyFee,
                    AssurancePriceHistoryId = dto.AssurancePriceHistoryOptionId,
                    OptionClassId = OptionClass.MIDDLE,
                    OptionQuotationStatusId = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId
                };

                OptionQuotation option3 = new OptionQuotation
                {
                    Id = Guid.NewGuid(),
                    MinTotal = 0,
                    MaxTotal = 0,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    EscortFee = dto.EscortFee,
                    OrganizationCost = dto.OrganizationCost,
                    OperatingFee = dto.OperatingFee,
                    ContingencyFee = dto.ContingencyFee,
                    AssurancePriceHistoryId = dto.AssurancePriceHistoryOptionId,
                    OptionClassId = OptionClass.PREMIUM,
                    OptionQuotationStatusId = OptionQuotationStatus.NEW,
                    PrivateTourRequestId = dto.PrivateTourRequestId
                };

                List<TourguideQuotationDetail> tourguideQuotationDetails = new List<TourguideQuotationDetail>();
                List<QuotationDetail> quotationDetails = new List<QuotationDetail>();
                List<VehicleQuotationDetail> vehicleQuotationDetails = new List<VehicleQuotationDetail>();
                List<Event> events = new List<Event>();
                List<EventDetail> eventDetails = new List<EventDetail>();
                List<EventDetailPriceHistory> eventDetailPriceHistories = new List<EventDetailPriceHistory>();

                double tourguideTotal = 0;
                double eventTotal = 0;
                foreach (var item in dto.TourGuideCosts)
                {
                    tourguideQuotationDetails.Add(new TourguideQuotationDetail
                    {
                        Id = Guid.NewGuid(),
                        IsMainTourGuide = item.ProvinceId == null,
                        Quantity = item.Quantity,
                        ProvinceId = item.ProvinceId,
                        OptionId = option1.Id
                    });

                    tourguideQuotationDetails.Add(new TourguideQuotationDetail
                    {
                        Id = Guid.NewGuid(),
                        IsMainTourGuide = item.ProvinceId == null,
                        Quantity = item.Quantity,
                        ProvinceId = item.ProvinceId,
                        OptionId = option2.Id
                    });
                    tourguideQuotationDetails.Add(new TourguideQuotationDetail
                    {
                        Id = Guid.NewGuid(),
                        IsMainTourGuide = item.ProvinceId == null,
                        Quantity = item.Quantity,
                        ProvinceId = item.ProvinceId,
                        OptionId = option3.Id
                    });
                }
                var tourguideCost = await humanResourceFeeService!.GetSalary(dto.TourGuideCosts, true);
                tourguideTotal = (double)tourguideCost.Result!; 

                foreach(var item in dto.MaterialCosts)
                {
                    var materialCostHostoryDb = await materialPriceHistoryRepository!.GetById(item.MaterialPriceHistoryId);
                    quotationDetails.Add(new QuotationDetail
                    {
                        Id = Guid.NewGuid(),
                        MaterialPriceHistoryId = materialCostHostoryDb.Id,
                        OptionQuotationId = option1.Id,
                        MinPrice = materialCostHostoryDb.Price * item.Quantity,
                        MaxPrice = materialCostHostoryDb.Price * item.Quantity,
                        Quantity = item.Quantity
                    });
                }            
                foreach (var item in dto.provinceServices)
                {
                    foreach (var location in item.Hotels)
                    {
                        var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
                        var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                        if(location.HotelOptionRatingOption1 != null)
                        {
                            var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                a => location.HotelOptionRatingOption1 == a.RatingId
                                && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == location.ServingQuantity
                                ).FirstOrDefault();
                            if (sellPriceHotel != null)
                            {
                                double min = sellPriceHotel!.MinPrice;

                                double max = sellPriceHotel!.MaxPrice;
                                int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                double minQuotation = numOfDay * location.NumOfRoom * min;
                                double maxQuotation = numOfDay * location.NumOfRoom * max;
                                var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption1 == a.RatingId);
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option1.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = location.ServingQuantity,
                                    Quantity = location.NumOfRoom,
                                    MinPrice = minQuotation,
                                    MaxPrice = maxQuotation,
                                    FacilityRatingId = hotelRating!.Id,
                                    StartDate = location.StartDate,
                                    EndDate = location.EndDate,
                                    DistrictId = location.DistrictId
                                });
                            }
                            else
                            {
                                result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng {location.ServingQuantity} người với phân loại {location.HotelOptionRatingOption1}");
                            }

                        }

                        if (location.HotelOptionRatingOption2 != null)
                        {
                            var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                a => location.HotelOptionRatingOption2 == a.RatingId
                                && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == location.ServingQuantity
                                ).FirstOrDefault();
                            if (sellPriceHotel != null)
                            {
                                double min = sellPriceHotel!.MinPrice;

                                double max = sellPriceHotel!.MaxPrice;
                                int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                double minQuotation = numOfDay * location.NumOfRoom * min;
                                double maxQuotation = numOfDay * location.NumOfRoom * max;
                                var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption2 == a.RatingId);
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option2.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = location.ServingQuantity,
                                    Quantity = location.NumOfRoom,
                                    MinPrice = minQuotation,
                                    MaxPrice = maxQuotation,
                                    FacilityRatingId = hotelRating!.Id,
                                    StartDate = location.StartDate,
                                    EndDate = location.EndDate,
                                    DistrictId = location.DistrictId
                                });
                            }
                            else
                            {
                                result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng {location.ServingQuantity} người với phân loại {location.HotelOptionRatingOption2}");
                            }
                        }
                        if (location.HotelOptionRatingOption3 != null)
                        {
                            var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                a => location.HotelOptionRatingOption3 == a.RatingId
                                && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == location.ServingQuantity
                                ).FirstOrDefault();
                            if (sellPriceHotel != null)
                            {
                                double min = sellPriceHotel!.MinPrice;

                                double max = sellPriceHotel!.MaxPrice;
                                int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                double minQuotation = numOfDay * location.NumOfRoom * min;
                                double maxQuotation = numOfDay * location.NumOfRoom * max;
                                var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption3 == a.RatingId);
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option3.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = location.ServingQuantity,
                                    Quantity = location.NumOfRoom,
                                    MinPrice = minQuotation,
                                    MaxPrice = maxQuotation,
                                    FacilityRatingId = hotelRating!.Id,
                                    StartDate = location.StartDate,
                                    EndDate = location.EndDate,
                                    DistrictId = location.DistrictId
                                });
                            }
                            else
                            {
                                result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng {location.ServingQuantity} người với phân loại {location.HotelOptionRatingOption3}");
                            }
                        }
                    }
                    foreach (var location in item.Restaurants)
                    {                
                        PagedResult<SellPriceHistory> menuPriceList = null;
                        SellPriceHistory menuPrice = null;
                        Menu menu = null;
                        foreach (var dayMenu in location.MenuQuotations)
                        {
                            menuPriceList = null;
                            menuPrice = null;
                            menu = null;
                            int quantity = 0;
                            int totalService = 0;
                            menu = await menuRepository!.GetByExpression(m => m.Id == dayMenu.MenuId, m => m.FacilityService);
                            quantity = menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.BOTH ? privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren :
                                           menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.ADULT ? privateTourRequest.NumOfAdult : privateTourRequest.NumOfChildren;
                            totalService = (int)Math.Ceiling((double)(quantity / menu.FacilityService.ServingQuantity));
                            menuPriceList = await sellPriceRepository!.GetAllDataByExpression(s => s.MenuId == dayMenu.MenuId && totalService >= s.MOQ, 0, 0, null, false, m => m.Menu.FacilityService!.Facility!);
                            if (menuPriceList.Items != null && menuPriceList.Items.Count > 0)
                            {
                                menuPrice = menuPriceList.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault();
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = dayMenu.option == OptionClass.ECONOMY? option1.Id : dayMenu.option == OptionClass.MIDDLE ? option2.Id : option3.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = menu.FacilityService.ServingQuantity,
                                    Quantity = totalService,
                                    MinPrice = menuPrice.Price,
                                    MaxPrice = menuPrice.Price,
                                    FacilityRatingId = menuPrice.Menu.FacilityService.Facility.FacilityRatingId,
                                    StartDate = dayMenu.Date,
                                    EndDate = dayMenu.Date,
                                    DistrictId = location.DistrictId,
                                    MenuId = menu.Id
                                });
                            }
                        }
                    }
                    foreach (var location in item.Entertainments)
                    {
                        var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
                        var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                        if (estimate.EntertainmentPrice.DetailedPriceReferences.Count > 0)
                        {
                            var entertaimentRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.ENTERTAINMENT);

                            var sellPriceAdultEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

                        && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.ADULT).FirstOrDefault();
                            var sellPriceChildrenEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA

                          && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.CHILD).FirstOrDefault();

                            double minQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MinPrice);
                            minQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MinPrice);

                            double maxQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MaxPrice);
                            maxQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MaxPrice);


                           if(location.QuantityLocationOption1 != null)
                            {
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option1.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = 1,
                                    Quantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren,
                                    MinPrice = (double)(minQuotationEntertaiment * location.QuantityLocationOption1),
                                    MaxPrice = maxQuotationEntertaiment,
                                    FacilityRatingId = entertaimentRating!.Id,
                                    StartDate = null,
                                    EndDate = null,
                                    DistrictId = location.DistrictId
                                });
                            }

                            if (location.QuantityLocationOption2 != null)
                            {
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option2.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = 1,
                                    Quantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren,
                                    MinPrice = (double)(minQuotationEntertaiment * location.QuantityLocationOption2),
                                    MaxPrice = maxQuotationEntertaiment,
                                    FacilityRatingId = entertaimentRating!.Id,
                                    StartDate = null,
                                    EndDate = null,
                                    DistrictId = location.DistrictId
                                });
                            }

                            if (location.QuantityLocationOption3 != null)
                            {
                                quotationDetails.Add(new QuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option3.Id,
                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
                                    QuantityOfChild = privateTourRequest.NumOfChildren,
                                    ServingQuantity = 1,
                                    Quantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren,
                                    MinPrice = (double)(minQuotationEntertaiment * location.QuantityLocationOption3),
                                    MaxPrice = maxQuotationEntertaiment,
                                    FacilityRatingId = entertaimentRating!.Id,
                                    StartDate = null,
                                    EndDate = null,
                                    DistrictId = location.DistrictId
                                });
                            }


                        }


                    }
                    if(item.EventGalas != null && item.EventGalas.Count > 0)
                    {
                        List<OptionEvent> optionEvents = new List<OptionEvent>();
                        foreach (var location in item.EventGalas)
                        {
                            var eventDb = await eventRepository!.GetByExpression(e => e.Id == location.EventId, null);
                            if (eventDb != null)
                            {
                                var optionEvent1 = new OptionEvent
                                {
                                    Id = Guid.NewGuid(),
                                    EventId = eventDb.Id,
                                    OptionId = option1.Id,
                                    CustomEvent = location.CustomEvent,
                                    Date = location.Date
                                };
                                var optionEvent2 = new OptionEvent
                                {
                                    Id = Guid.NewGuid(),
                                    EventId = eventDb.Id,
                                    OptionId = option2.Id,
                                    CustomEvent = location.CustomEvent,
                                    Date = location.Date
                                };
                                var optionEvent3 = new OptionEvent
                                {
                                    Id = Guid.NewGuid(),
                                    EventId = eventDb.Id,
                                    OptionId = option3.Id,
                                    CustomEvent = location.CustomEvent,
                                    Date = location.Date
                                };
                                optionEvents.Add(optionEvent1);
                                optionEvents.Add(optionEvent2);
                                optionEvents.Add(optionEvent3);
                                var eventCostDetail = await eventDetailPriceRepository!.GetAllDataByExpression(e => e.EventDetail!.EventId == eventDb.Id, 0, 0, e => e.Date, false, e => e.EventDetail);
                                var latestCost = eventCostDetail.Items!.GroupBy(e => e.EventDetailId)
                                                                       .Select(g => g.OrderByDescending(e => e.Date).FirstOrDefault())
                                                                       .ToList();
                                foreach (var price in latestCost)
                                {
                                    eventTotal += price.Price * (price.EventDetail.PerPerson ? totalPeople : 1);
                                }
                            }
                        }
                        await eventOptionRepository!.InsertRange(optionEvents);
                    }
                }
                List<HumanResourceCost> driverCostOption1 = new List<HumanResourceCost>();
                List<HumanResourceCost> driverCostOption2 = new List<HumanResourceCost>();
                List<HumanResourceCost> driverCostOption3 = new List<HumanResourceCost>();

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
                           if(vehicle.OptionClass1 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option1.Id,
                                    MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                    MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                    StartPointId = (Guid)vehicle.StartPoint!,
                                    EndPointId = (Guid)vehicle.EndPoint!,
                                    StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                    EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                    VehicleType = vehicle.VehicleType,
                                });
                              
                            }

                            if (vehicle.OptionClass2 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option2.Id,
                                    MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                    MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                    StartPointId = (Guid)vehicle.StartPoint,
                                    EndPointId = (Guid)vehicle.EndPoint,
                                    StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                    EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                    VehicleType = vehicle.VehicleType,
                                });
                            }

                            if (vehicle.OptionClass3 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option3.Id,
                                    MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                    MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                    StartPointId = (Guid)vehicle.StartPoint!,
                                    EndPointId = (Guid)vehicle.EndPoint!,
                                    StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                    EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                    VehicleType = vehicle.VehicleType,
                                });
                            }

                        }
                    }
                    else
                    {
                        var price = await sellPriceRepository!.GetAllDataByExpression(s => s.TransportServiceDetail.FacilityService.Facility.Communce.District!.ProvinceId == vehicle.StartPoint
                        && s.TransportServiceDetail.VehicleTypeId == vehicle.VehicleType, 0, 0, null, false, s => s.TransportServiceDetail.FacilityService!);
                        if (price.Items!.Any())
                        {
                            int totalVehicle = (int)Math.Ceiling((decimal)((decimal)(privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren) / price.Items[0].TransportServiceDetail.FacilityService!.ServingQuantity));
                            int totalDays = (vehicle.EndDate - vehicle.StartDate).Days;
                            if(vehicle.OptionClass1 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option1.Id,
                                    MinPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Min(a => a.Price),
                                    MaxPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Max(a => a.Price),
                                    NumOfRentingDay = totalDays,
                                    NumOfVehicle = vehicle.NumOfVehicle,
                                    StartPointId = (Guid)vehicle.StartPoint!,
                                    EndPointId = vehicle.EndPoint,
                                    VehicleType = vehicle.VehicleType
                                });
                                driverCostOption1.Add(new HumanResourceCost
                                {
                                    Quantity = vehicle.NumOfVehicle,
                                    NumOfDay = totalDays
                                });
                            }

                            if (vehicle.OptionClass2 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option2.Id,
                                    MinPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Min(a => a.Price),
                                    MaxPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Max(a => a.Price),
                                    NumOfRentingDay = totalDays,
                                    NumOfVehicle = vehicle.NumOfVehicle,
                                    StartPointId = (Guid)vehicle.StartPoint!,
                                    EndPointId = vehicle.EndPoint,
                                    VehicleType = vehicle.VehicleType
                                });
                                driverCostOption2.Add(new HumanResourceCost
                                {
                                    Quantity = vehicle.NumOfVehicle,
                                    NumOfDay = totalDays
                                });
                            }

                            if (vehicle.OptionClass3 != null)
                            {
                                vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                {
                                    Id = Guid.NewGuid(),
                                    OptionQuotationId = option3.Id,
                                    MinPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Min(a => a.Price),
                                    MaxPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Max(a => a.Price),
                                    NumOfRentingDay = totalDays,
                                    NumOfVehicle = vehicle.NumOfVehicle,
                                    StartPointId = (Guid)vehicle.StartPoint!,
                                    EndPointId = vehicle.EndPoint,
                                    VehicleType = vehicle.VehicleType
                                });
                                driverCostOption3.Add(new HumanResourceCost
                                {
                                    Quantity = vehicle.NumOfVehicle,
                                    NumOfDay = totalDays
                                });
                            }
                        }
                    }


                }

                var totalDriverCostOption1 = await humanResourceFeeService.GetSalary(driverCostOption1, false);
                var totalDriverCostOption2 = await humanResourceFeeService.GetSalary(driverCostOption2, false);
                var totalDriverCostOption3 = await humanResourceFeeService.GetSalary(driverCostOption3, false);

                option1.DriverCost = (double)totalDriverCostOption1.Result;
                option2.DriverCost = (double)totalDriverCostOption2.Result;
                option3.DriverCost = (double)totalDriverCostOption3.Result;

                quotationDetails.Where(q => q.OptionQuotationId == option1.Id).ToList().ForEach(q =>
                    {
                        option1.MinTotal += q.MinPrice;
                        option1.MaxTotal += q.MaxPrice;
                    });

                quotationDetails.Where(q => q.OptionQuotationId == option2.Id).ToList().ForEach(q =>
                {
                    option2.MinTotal += q.MinPrice;
                    option2.MaxTotal += q.MaxPrice;
                });

                quotationDetails.Where(q => q.OptionQuotationId == option3.Id).ToList().ForEach(q =>
                {
                    option3.MinTotal += q.MinPrice;
                    option3.MaxTotal += q.MaxPrice;
                });

                vehicleQuotationDetails.Where(q => q.OptionQuotationId == option1.Id).ToList().ForEach(q =>
                {
                    option1.MinTotal += q.MinPrice;
                    option1.MaxTotal += q.MaxPrice;
                });

                vehicleQuotationDetails.Where(q => q.OptionQuotationId == option2.Id).ToList().ForEach(q =>
                {
                    option2.MinTotal += q.MinPrice;
                    option2.MaxTotal += q.MaxPrice;
                });

                vehicleQuotationDetails.Where(q => q.OptionQuotationId == option3.Id).ToList().ForEach(q =>
                {
                    option3.MinTotal += q.MinPrice;
                    option3.MaxTotal += q.MaxPrice;
                });

                option1.MinTotal = option1.EscortFee + option1.ContingencyFee * totalPeople + option1.OperatingFee + option1.OrganizationCost;
                option1.MaxTotal = option1.EscortFee + option1.ContingencyFee * totalPeople + option1.OperatingFee + option1.OrganizationCost;

                option2.MinTotal = option2.EscortFee + option2.ContingencyFee * totalPeople + option2.OperatingFee + option2.OrganizationCost;
                option2.MaxTotal = option2.EscortFee + option2.ContingencyFee * totalPeople + option2.OperatingFee + option2.OrganizationCost;

                option3.MinTotal = option3.EscortFee + option3.ContingencyFee * totalPeople + option3.OperatingFee + option3.OrganizationCost;
                option3.MaxTotal = option3.EscortFee + option3.ContingencyFee * totalPeople + option3.OperatingFee + option3.OrganizationCost;

                option1.MinTotal = dto.AssurancePricePerPerson * totalPeople;
                option1.MaxTotal = dto.AssurancePricePerPerson * totalPeople;

                option2.MinTotal = dto.AssurancePricePerPerson * totalPeople;
                option2.MaxTotal = dto.AssurancePricePerPerson * totalPeople; 
                
                option3.MinTotal = dto.AssurancePricePerPerson * totalPeople;
                option3.MaxTotal = dto.AssurancePricePerPerson * totalPeople;

                option1.MinTotal = Math.Ceiling(option1.MinTotal / (totalPeople * 1000)) * 1000;
                option1.MaxTotal = Math.Ceiling(option1.MaxTotal / (totalPeople * 1000)) * 1000;

                option2.MinTotal = Math.Ceiling(option2.MinTotal / (totalPeople * 1000)) * 1000;
                option2.MaxTotal = Math.Ceiling(option2.MaxTotal / (totalPeople * 1000)) * 1000;

                option3.MinTotal = Math.Ceiling(option3.MinTotal / (totalPeople * 1000)) * 1000;
                option3.MaxTotal = Math.Ceiling(option3.MaxTotal / (totalPeople * 1000)) * 1000;

                option1.MinTotal += materialTotal;
                option1.MaxTotal += materialTotal;
                option2.MinTotal += materialTotal;
                option2.MaxTotal += materialTotal;
                option3.MinTotal += materialTotal;
                option3.MaxTotal += materialTotal;

                option1.MinTotal += tourguideTotal;
                option1.MaxTotal += tourguideTotal;
                option2.MinTotal += tourguideTotal;
                option2.MaxTotal += tourguideTotal;
                option3.MinTotal += tourguideTotal;
                option3.MaxTotal += tourguideTotal;

                option1.MinTotal += (double)totalDriverCostOption1.Result!;
                option1.MaxTotal += (double)totalDriverCostOption1.Result!;
                option2.MinTotal += (double)totalDriverCostOption2.Result!;
                option2.MaxTotal += (double)totalDriverCostOption2.Result!;
                option3.MinTotal += (double)totalDriverCostOption3.Result!;
                option3.MaxTotal += (double)totalDriverCostOption3.Result!;

                option1.MinTotal += eventTotal;
                option1.MaxTotal += eventTotal;
                option2.MinTotal += eventTotal;
                option2.MaxTotal += eventTotal;
                option3.MinTotal += eventTotal;
                option3.MaxTotal += eventTotal;
                


                await optionQuotationRepository!.Insert(option1);
                await optionQuotationRepository!.Insert(option2);
                await optionQuotationRepository!.Insert(option3);
                await quotationDetailRepository!.InsertRange(quotationDetails);
                await vehicleQuotationDetailRepository!.InsertRange(vehicleQuotationDetails);
                await tourguideQuotationDetailRepository!.InsertRange(tourguideQuotationDetails);
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

    public async Task<AppActionResult> GetPrivateTourRequestByIdForCustomer(Guid id)
    {
        var result = new AppActionResult();
        OptionListResponseForCustomer data = new OptionListResponseForCustomer();
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
                OptionResponseForCustomer option = new OptionResponseForCustomer();
                option.OptionQuotation = item;
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a => a.District!.Province!, a => a.ServiceType!, a => a.FacilityRating!.FacilityType!, a => a.FacilityRating!.Rating!);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a => a.StartPoint!, a => a.EndPoint!);
                option.QuotationDetails = quotationDetailDb.Items!.ToList();
                option.VehicleQuotationDetails = vehicleQuotationDetailDb.Items!.Where(v => v.EndPointId != null).ToList();
                option.InnerProvinceVehicleQuotationDetails = vehicleQuotationDetailDb.Items!.Where(v => v.EndPointId == null).ToList();
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




    //public async Task<AppActionResult> CreateOptionsPrivateTour(CreateOptionsPrivateTourDto dto)
    //{
    //    var result = new AppActionResult();
    //    var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
    //    var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
    //    var districtRepository = Resolve<IRepository<District>>();
    //    var facilityRepository = Resolve<IRepository<Facility>>();
    //    var facilityRatingRepository = Resolve<IRepository<FacilityRating>>();
    //    var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
    //    var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
    //    var referenceTransportPriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
    //    var materialPriceHistoryRepository = Resolve<IRepository<MaterialPriceHistory>>();
    //    var eventRepository = Resolve<IRepository<Event>>();
    //    var eventDetailRepository = Resolve<IRepository<EventDetail>>();
    //    var eventDetailPriceRepository = Resolve<IRepository<EventDetailPriceHistory>>();
    //    var tourguideQuotationDetailRepository = Resolve<IRepository<TourguideQuotationDetail>>();
    //    var facilityService = Resolve<IFacilityServiceService>();
    //    try
    //    {
    //        var privateTourRequest = await _repository.GetById(dto.PrivateTourRequestId);
    //        if (privateTourRequest == null)
    //        {
    //            result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {dto.PrivateTourRequestId}");
    //            return result;
    //        }
    //        var totalPeople = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren;
    //        if (dto.ContingencyFee <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Phí dự phòng/người phải lớn hơn 0");
    //            return result;
    //        }
    //        if (dto.OrganizationCost <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Phí tổ chức phải lớn hơn 0");
    //            return result;
    //        }
    //        if (dto.EscortFee <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Phí dẫn đường phải lớn hơn 0");
    //            return result;
    //        }
    //        if (dto.OperatingFee <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Phí vận hành phải lớn hơn 0");
    //            return result;
    //        }

    //        if (dto.TourGuideCosts.Count <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Cần thêm báo giá về lương hướng dẫn viên");
    //            return result;
    //        }
    //        if (dto.MaterialCosts.Count <= 0)
    //        {
    //            result = BuildAppActionResultError(result, $"Cần thêm báo giá về chi phí khác");
    //            return result;
    //        }
    //        double materialTotal = 0;
    //        List<MaterialPriceHistory> materialPriceHistories = new List<MaterialPriceHistory>();
    //        foreach (var material in dto.MaterialCosts)
    //        {
    //            var materialPrice = await materialPriceHistoryRepository!.GetById(material.MaterialPriceHistoryId);
    //            if (materialPrice == null)
    //            {
    //                result = BuildAppActionResultError(result, $"Không tìm thấy báo giá về vật liệu có id {material.MaterialPriceHistoryId}");
    //                return result;
    //            }
    //            materialPriceHistories.Add(materialPrice);
    //            materialTotal += material.Quantity * materialPrice.Price;
    //        }
    //        var assuranceHistoryRepository = Resolve<IRepository<AssurancePriceHistory>>();
    //        var assuranceHistoryDb = await assuranceHistoryRepository!.GetById(dto.AssurancePriceHistoryId);
    //        if (assuranceHistoryDb == null)
    //        {
    //            result = BuildAppActionResultError(result, $"Không tìm thấy giá bảo hiểm với id {dto.AssurancePriceHistoryId}");
    //            return result;
    //        }
    //        int errorCount = 0;
    //        var menuRepository = Resolve<IRepository<Menu>>();
    //        foreach (var item in dto.provinceServices)
    //        {
    //            District district = null;
    //            foreach (var location in item.Hotels)
    //            {
    //                district = await districtRepository!.GetById(location.DistrictId);
    //                if (district == null)
    //                {
    //                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
    //                    return result;
    //                }
    //                if (location.HotelOptionRatingOption1 != null)
    //                {
    //                    var listHotel = await facilityRepository!.GetAllDataByExpression(
    //                       a => a.Communce!.DistrictId == location.DistrictId
    //                      && location.HotelOptionRatingOption1 == a.FacilityRating!.RatingId &&
    //                      a.FacilityRating.FacilityTypeId == FacilityType.HOTEL,
    //                       0,
    //                       0, null, false,
    //                       null);
    //                    var sellHotel = await sellPriceRepository!.GetAllDataByExpression(
    //                        a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
    //                        && a.FacilityService.ServiceTypeId == ServiceType.RESTING
    //                        && location.HotelOptionRatingOption1 == a.FacilityService.Facility.FacilityRating!.RatingId
    //                        ,
    //                        0, 0, null, false, null
    //                        );
    //                    if (!listHotel.Items!.Any())
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy khách sạn với phân loại {location.HotelOptionRatingOption1} tại huyện {district!.Name}");
    //                        errorCount++;
    //                        break;
    //                    }
    //                    if (!sellHotel.Items!.Any())
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy gía khách sạn với phân loại {location.HotelOptionRatingOption1} tại huyện {district!.Name}");
    //                        errorCount++;
    //                        break;
    //                    }
    //                }
    //                //same for other option                  
    //            }
    //            if (errorCount > 0)
    //            {
    //                break;
    //            }
    //            foreach (var location in item.Restaurants)
    //            {
    //                district = await districtRepository!.GetById(location.DistrictId);
    //                if (district == null)
    //                {
    //                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
    //                }
    //                foreach (var menuOption in location.MenuQuotations)
    //                {
    //                    if (menuOption.BreakfastMenuOption1 != null)
    //                    {
    //                        var menu = await menuRepository!.GetByExpression(m => m.Id == menuOption.BreakfastMenuOption1, null);
    //                        if (menu == null)
    //                        {
    //                            result = BuildAppActionResultError(result, $"Không tìm thấy menu với id {menuOption.BreakfastMenuOption1}");
    //                            errorCount++;
    //                            break;
    //                        }
    //                        var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
    //                           s => s.MenuId == menuOption.BreakfastMenuOption1,
    //                           0, 0, null, false, null
    //                           );

    //                        if (!sellRestaurent.Items!.Any())
    //                        {
    //                            result = BuildAppActionResultError(result, $"Không tìm thấy menu {menu.Name}");
    //                            errorCount++;
    //                            break;
    //                        }
    //                    }
    //                    //same for other option
    //                }
    //                if (errorCount > 0)
    //                {
    //                    break;
    //                }

    //            }
    //            if (errorCount > 0)
    //            {
    //                break;
    //            }
    //            foreach (var location in item.Entertainments)
    //            {
    //                district = await districtRepository!.GetById(location.DistrictId);
    //                if (district == null)
    //                {
    //                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {location.DistrictId}");
    //                }
    //                var entertaiment = await facilityRepository!.GetAllDataByExpression(
    //                          a => a.Communce!.DistrictId == location.DistrictId &&
    //                        a.FacilityRating!.FacilityTypeId == FacilityType.ENTERTAINMENT,
    //                          0,
    //                          0, null, false,
    //                          null);
    //                var sellEntertaiment = await sellPriceRepository!.GetAllDataByExpression(
    //                     a => a.FacilityService!.Facility!.Communce!.DistrictId == location.DistrictId
    //                       && a.FacilityService.ServiceTypeId == ServiceType.ENTERTAIMENT,
    //                     0, 0, null, false, null
    //                     );
    //                if (!entertaiment.Items!.Any())
    //                {
    //                    result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ vui chơi giải trí tại huyện {district.Name}");
    //                    errorCount++;
    //                    break;
    //                }
    //                if (!sellEntertaiment.Items!.Any())
    //                {
    //                    result = BuildAppActionResultError(result, $"Không tìm thấy giá dịch vụ giải trí tại huyện {district.Name}");
    //                    errorCount++;
    //                    break;
    //                }
    //            }
    //        }
    //        if (errorCount == 0)
    //        {
    //            var provinceRepository = Resolve<IRepository<Province>>();
    //            Province province = null;
    //            foreach (var item in dto.Vehicles)
    //            {
    //                if (item.StartPoint != null)
    //                {
    //                    if ((await provinceRepository!.GetById(item.StartPoint)) == null)
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy tỉnh với id {item.StartPoint}");
    //                        break;
    //                    }
    //                }
    //                if (item.EndPoint != null)
    //                {
    //                    if ((await provinceRepository!.GetById(item.EndPoint)) == null)
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy tỉnh với id {item.EndPoint}");
    //                        break;
    //                    }
    //                }
    //                if (item.StartPointDistrict != null)
    //                {
    //                    if ((await districtRepository!.GetById(item.StartPointDistrict!)) == null)
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {item.StartPointDistrict}");
    //                        break;
    //                    }
    //                }
    //                if (item.EndPointDistrict != null)
    //                {
    //                    if ((await districtRepository!.GetById(item.EndPointDistrict!)) == null)
    //                    {
    //                        result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {item.EndPointDistrict}");
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        if (!BuildAppActionResultIsError(result))
    //        {
    //            OptionQuotation option1 = new OptionQuotation
    //            {
    //                Id = Guid.NewGuid(),
    //                MinTotal = 0,
    //                MaxTotal = 0,
    //                StartDate = dto.StartDate,
    //                EndDate = dto.EndDate,
    //                EscortFee = dto.EscortFee,
    //                OrganizationCost = dto.OrganizationCost,
    //                OperatingFee = dto.OperatingFee,
    //                ContingencyFee = dto.ContingencyFee,
    //                AssurancePriceHistoryId = dto.AssurancePriceHistoryId,
    //                OptionClassId = OptionClass.ECONOMY,
    //                OptionQuotationStatusId = OptionQuotationStatus.NEW,
    //                PrivateTourRequestId = dto.PrivateTourRequestId
    //            };
    //            //same for other option
    //            List<TourguideQuotationDetail> tourguideQuotationDetails = new List<TourguideQuotationDetail>();
    //            List<QuotationDetail> quotationDetails = new List<QuotationDetail>();
    //            List<VehicleQuotationDetail> vehicleQuotationDetails = new List<VehicleQuotationDetail>();
    //            List<Event> events = new List<Event>();
    //            List<EventDetail> eventDetails = new List<EventDetail>();
    //            List<EventDetailPriceHistory> eventDetailPriceHistories = new List<EventDetailPriceHistory>();
    //            double tourguideTotal = 0;
    //            double eventTotal1 = 0;
    //            double eventTotal2 = 0;
    //            double eventTotal3 = 0;
    //            foreach (var item in dto.TourGuideCosts)
    //            {
    //                tourguideQuotationDetails.Add(new TourguideQuotationDetail
    //                {
    //                    Id = Guid.NewGuid(),
    //                    IsMainTourGuide = item.ProvinceId == null,
    //                    Quantity = item.Quantity,
    //                    ProvinceId = item.ProvinceId,
    //                    OptionId = option1.Id
    //                });
    //                tourguideTotal += item.Cost * item.NumOfDay * item.Quantity;
    //            }
    //            foreach (var item in dto.MaterialCosts)
    //            {
    //                var materialCostHostoryDb = await materialPriceHistoryRepository!.GetById(item.MaterialPriceHistoryId);
    //                quotationDetails.Add(new QuotationDetail
    //                {
    //                    Id = Guid.NewGuid(),
    //                    MaterialPriceHistoryId = materialCostHostoryDb.MaterialId,
    //                    OptionQuotationId = option1.Id,
    //                    MinPrice = materialCostHostoryDb.Price * item.Quantity,
    //                    MaxPrice = materialCostHostoryDb.Price * item.Quantity,
    //                    Quantity = item.Quantity
    //                });
    //            }
    //            foreach (var item in dto.provinceServices)
    //            {
    //                foreach (var location in item.Hotels)
    //                {
    //                    var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
    //                    var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
    //                    if (location.HotelOptionRatingOption1 != null)
    //                    {
    //                        var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
    //                            a => location.HotelOptionRatingOption1 == a.RatingId
    //                            && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == location.ServingQuantity
    //                            ).FirstOrDefault();
    //                        if (sellPriceHotel != null)
    //                        {
    //                            double min = sellPriceHotel!.MinPrice;

    //                            double max = sellPriceHotel!.MaxPrice;
    //                            int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
    //                            double minQuotation = numOfDay * location.NumOfRoom * min;
    //                            double maxQuotation = numOfDay * location.NumOfRoom * max;
    //                            var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption1 == a.RatingId);
    //                            quotationDetails.Add(new QuotationDetail
    //                            {
    //                                Id = Guid.NewGuid(),
    //                                OptionQuotationId = option1.Id,
    //                                QuantityOfAdult = privateTourRequest.NumOfAdult,
    //                                QuantityOfChild = privateTourRequest.NumOfChildren,
    //                                ServingQuantity = location.ServingQuantity,
    //                                Quantity = location.NumOfRoom,
    //                                MinPrice = minQuotation,
    //                                MaxPrice = maxQuotation,
    //                                FacilityRatingId = hotelRating!.Id,
    //                                StartDate = location.StartDate,
    //                                EndDate = location.EndDate,
    //                                DistrictId = location.DistrictId
    //                            });
    //                        }
    //                        else
    //                        {
    //                            result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng {location.ServingQuantity} người với phân loại {location.HotelOptionRatingOption1}");
    //                        }
    //                    }

    //                    //same for other option
    //                }
    //                foreach (var location in item.Restaurants)
    //                {
    //                    PagedResult<SellPriceHistory> menuPriceList = null;
    //                    SellPriceHistory menuPrice = null;
    //                    Menu menu = null;
    //                    foreach (var dayMenu in location.MenuQuotations)
    //                    {
    //                        menuPriceList = null;
    //                        menuPrice = null;
    //                        menu = null;
    //                        int quantity = 0;
    //                        int totalService = 0;
    //                        if (dayMenu.BreakfastMenuOption1 != null)
    //                        {
    //                            menu = await menuRepository!.GetByExpression(m => m.Id == dayMenu.BreakfastMenuOption1, m => m.FacilityService);
    //                            quantity = menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.BOTH ? privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren :
    //                                           menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.ADULT ? privateTourRequest.NumOfAdult : privateTourRequest.NumOfChildren;
    //                            totalService = (int)Math.Ceiling((double)(quantity / menu.FacilityService.ServingQuantity));
    //                            menuPriceList = await sellPriceRepository!.GetAllDataByExpression(s => s.MenuId == dayMenu.BreakfastMenuOption1 && totalService >= s.MOQ, 0, 0, null, false, m => m.FacilityService!.Facility!);
    //                            if (menuPriceList.Items != null && menuPriceList.Items.Count > 0)
    //                            {
    //                                menuPrice = menuPriceList.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault();
    //                                quotationDetails.Add(new QuotationDetail
    //                                {
    //                                    Id = Guid.NewGuid(),
    //                                    OptionQuotationId = option1.Id,
    //                                    QuantityOfAdult = privateTourRequest.NumOfAdult,
    //                                    QuantityOfChild = privateTourRequest.NumOfChildren,
    //                                    ServingQuantity = menu.FacilityService.ServingQuantity,
    //                                    Quantity = totalService,
    //                                    MinPrice = menuPrice.Price,
    //                                    MaxPrice = menuPrice.Price,
    //                                    FacilityRatingId = menuPrice.FacilityService.Facility.FacilityRatingId,
    //                                    StartDate = dayMenu.Date,
    //                                    EndDate = dayMenu.Date,
    //                                    DistrictId = location.DistrictId,
    //                                    MenuId = menu.Id
    //                                });
    //                            }
    //                        }                      
    //                        //same for other 8 options
    //                    }
    //                }
    //                foreach (var location in item.Entertainments)
    //                {
    //                    var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
    //                    var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
    //                    if (estimate.EntertainmentPrice.DetailedPriceReferences.Count > 0)
    //                    {
    //                        var entertaimentRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.ENTERTAINMENT);
    //                        var sellPriceAdultEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA
    //                    && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.ADULT).FirstOrDefault();
    //                        var sellPriceChildrenEntertainment = estimate.EntertainmentPrice.DetailedPriceReferences.Where(a => a.RatingId == Rating.TOURIST_AREA
    //                      && a.ServingQuantity == 1 && a.ServiceAvailability == ServiceAvailability.CHILD).FirstOrDefault();
    //                        double minQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MinPrice);
    //                        minQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MinPrice);
    //                        double maxQuotationEntertaiment = (privateTourRequest.NumOfAdult * sellPriceAdultEntertainment!.MaxPrice);
    //                        maxQuotationEntertaiment += privateTourRequest.NumOfChildren * (sellPriceChildrenEntertainment == null ? 0 : sellPriceChildrenEntertainment.MaxPrice);
    //                        if (location.QuantityLocationOption1 != null)
    //                        {
    //                            quotationDetails.Add(new QuotationDetail
    //                            {
    //                                Id = Guid.NewGuid(),
    //                                OptionQuotationId = option1.Id,
    //                                QuantityOfAdult = privateTourRequest.NumOfAdult,
    //                                QuantityOfChild = privateTourRequest.NumOfChildren,
    //                                ServingQuantity = 1,
    //                                Quantity = privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren,
    //                                MinPrice = (double)(minQuotationEntertaiment * location.QuantityLocationOption1),
    //                                MaxPrice = maxQuotationEntertaiment,
    //                                FacilityRatingId = entertaimentRating!.Id,
    //                                StartDate = null,
    //                                EndDate = null,
    //                                DistrictId = location.DistrictId
    //                            });
    //                        }
    //                       //same for option 2 and 3
    //                    }
    //                }
    //                if (item.EventGalas != null && item.EventGalas.Count > 0)
    //                {
    //                    foreach (var location in item.EventGalas)
    //                    {
    //                        if (location.Option1EventId != null)
    //                        {
    //                            var eventDb = await eventRepository!.GetByExpression(e => e.Id == location.Option1EventId && e.OptionId == null, null);
    //                            if (eventDb != null)
    //                            {
    //                                var newEvent = new Event
    //                                {
    //                                    Id = Guid.NewGuid(),
    //                                    Description = eventDb.Description,
    //                                    Date = location.Date,
    //                                    Name = eventDb.Name,
    //                                    OptionId = location.Option1EventId,
    //                                    Total = 0,
    //                                    MOQ = eventDb.MOQ
    //                                };
    //                                events.Add(newEvent);
    //                                var eventCostDetail = await eventDetailPriceRepository!.GetAllDataByExpression(e => e.EventDetail!.EventId == eventDb.Id, 0, 0, e => e.Date, false, e => e.EventDetail);
    //                                var latestCost = eventCostDetail.Items!.GroupBy(e => e.EventDetailId)
    //                                                                       .Select(g => g.OrderByDescending(e => e.Date).FirstOrDefault())
    //                                                                       .ToList();
    //                                foreach (var price in latestCost)
    //                                {
    //                                    var newDetail = new EventDetail
    //                                    {
    //                                        Id = Guid.NewGuid(),
    //                                        EventId = newEvent.Id,
    //                                        Name = price.EventDetail.Name,
    //                                        Quantity = (price.EventDetail!.PerPerson) ? totalPeople : 1,
    //                                    };
    //                                    eventDetails.Add(newDetail);
    //                                    var newDetailPrice = new EventDetailPriceHistory
    //                                    {
    //                                        Id = Guid.NewGuid(),
    //                                        Date = price.Date,
    //                                        EventDetailId = newDetail.Id,
    //                                        Price = price.Price
    //                                    };
    //                                    eventDetailPriceHistories.Add(newDetailPrice);
    //                                    eventTotal1 += newDetailPrice.Price * newDetail.Quantity;
    //                                }

    //                            }
    //                        } //same for other option                            
    //                    }  }               }
    //            foreach (var vehicle in dto.Vehicles)
    //            {
    //                if (vehicle.VehicleType == VehicleType.PLANE || vehicle.VehicleType == VehicleType.BOAT)
    //                {
    //                    var price = await referenceTransportPriceRepository!.GetAllDataByExpression(a => a.Departure!.Commune!.District!.ProvinceId == vehicle.StartPoint && a.Arrival!.Commune!.District!.ProvinceId == vehicle.EndPoint
    //                || a.Departure.Commune.District.ProvinceId == vehicle.EndPoint && a.Arrival!.Commune!.District!.ProvinceId == vehicle.StartPoint
    //                , 0, 0, null, false, null
    //                );
    //                    if (price.Items!.Any())
    //                    {
    //                        if (vehicle.OptionClass1 != null)
    //                        {
    //                            vehicleQuotationDetails.Add(new VehicleQuotationDetail
    //                            {
    //                                Id = Guid.NewGuid(),
    //                                OptionQuotationId = option1.Id,
    //                                MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
    //                                MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
    //                                StartPointId = (Guid)vehicle.StartPoint!,
    //                                EndPointId = (Guid)vehicle.EndPoint!,
    //                                StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
    //                                EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
    //                                VehicleType = vehicle.VehicleType,
    //                            });
    //                        }
    //                        if (vehicle.OptionClass2 != null)
    //                       //same for other option
    //                    }
    //                }
    //                else
    //                {
    //                    var price = await sellPriceRepository!.GetAllDataByExpression(s => s.TransportServiceDetail.FacilityService.Facility.Communce.District!.ProvinceId == vehicle.StartPoint
    //                    && s.TransportServiceDetail.VehicleTypeId == vehicle.VehicleType, 0, 0, null, false, s => s.TransportServiceDetail.FacilityService!);
    //                    if (price.Items!.Any())
    //                    {
    //                        int totalVehicle = (int)Math.Ceiling((decimal)((decimal)(privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren) / price.Items[0].TransportServiceDetail.FacilityService!.ServingQuantity));
    //                        int totalDays = (vehicle.EndDate - vehicle.StartDate).Days;
    //                        if (vehicle.OptionClass1 != null)
    //                        {
    //                            vehicleQuotationDetails.Add(new VehicleQuotationDetail
    //                            {
    //                                Id = Guid.NewGuid(),
    //                                OptionQuotationId = option1.Id,
    //                                MinPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Min(a => a.Price),
    //                                MaxPrice = vehicle.NumOfVehicle * totalDays * price.Items!.Max(a => a.Price),
    //                                NumOfRentingDay = totalDays,
    //                                NumOfVehicle = vehicle.NumOfVehicle,
    //                                StartPointId = (Guid)vehicle.StartPoint!,
    //                                EndPointId = vehicle.EndPoint,
    //                                VehicleType = vehicle.VehicleType
    //                            });
    //                        }
    //                        // same for other option
    //                    }
    //                }
    //            }
    //            quotationDetails.Where(q => q.OptionQuotationId == option1.Id).ToList().ForEach(q =>
    //            {
    //                option1.MinTotal += q.MinPrice;
    //                option1.MaxTotal += q.MaxPrice;
    //            });               
    //            vehicleQuotationDetails.Where(q => q.OptionQuotationId == option1.Id).ToList().ForEach(q =>
    //            {
    //                option1.MinTotal += q.MinPrice;
    //                option1.MaxTotal += q.MaxPrice;
    //            });           
    //            option1.MinTotal = option1.EscortFee + option1.ContingencyFee * totalPeople + option1.OperatingFee + option1.OrganizationCost;
    //            option1.MaxTotal = option1.EscortFee + option1.ContingencyFee * totalPeople + option1.OperatingFee + option1.OrganizationCost; 
    //            option1.MinTotal = Math.Ceiling(option1.MinTotal / (totalPeople * 1000)) * 1000;
    //            option1.MaxTotal = Math.Ceiling(option1.MaxTotal / (totalPeople * 1000)) * 1000;  
    //            option1.MinTotal += materialTotal;
    //            option1.MaxTotal += materialTotal;              
    //            option1.MinTotal += tourguideTotal;
    //            option1.MaxTotal += tourguideTotal;
    //            option1.MinTotal += eventTotal1;
    //            option1.MaxTotal += eventTotal1;
    //            await optionQuotationRepository!.Insert(option1);
    //            await quotationDetailRepository!.InsertRange(quotationDetails);
    //            await vehicleQuotationDetailRepository!.InsertRange(vehicleQuotationDetails);
    //            await tourguideQuotationDetailRepository!.InsertRange(tourguideQuotationDetails);
    //            await eventRepository!.InsertRange(events);
    //            await eventDetailRepository!.InsertRange(eventDetails);
    //            await eventDetailPriceRepository!.InsertRange(eventDetailPriceHistories);
    //            await _unitOfWork.SaveChangesAsync();
    //        }

    //    }
    //    catch (Exception e)
    //    {
    //        result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
    //    }

    //    return result;
    //}
}