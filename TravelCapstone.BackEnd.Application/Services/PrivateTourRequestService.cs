using AutoMapper;
using Bogus;
using EnumsNET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.SS.Formula;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel.Design;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Transactions;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;
using static TravelCapstone.BackEnd.Common.DTO.Response.MapInfo;
using FacilityType = TravelCapstone.BackEnd.Domain.Enum.FacilityType;
using OptionClass = TravelCapstone.BackEnd.Domain.Enum.OptionClass;
using OptionQuotationStatus = TravelCapstone.BackEnd.Domain.Enum.OptionQuotationStatus;
using OrderStatus = TravelCapstone.BackEnd.Domain.Enum.OrderStatus;
using PrivateTourStatus = TravelCapstone.BackEnd.Domain.Enum.PrivateTourStatus;
using Rating = TravelCapstone.BackEnd.Domain.Enum.Rating;
using ServiceAvailability = TravelCapstone.BackEnd.Domain.Enum.ServiceAvailability;
using ServiceType = TravelCapstone.BackEnd.Domain.Enum.ServiceType;
using VehicleType = TravelCapstone.BackEnd.Domain.Enum.VehicleType;

namespace TravelCapstone.BackEnd.Application.Services;

public class PrivateTourRequestService : GenericBackendService, IPrivateTourRequestService
{
    private readonly IMapper _mapper;
    private readonly IExcelService _excelService;
    private IRepository<PrivateTourRequest> _repository;
    private IUnitOfWork _unitOfWork;

    public PrivateTourRequestService(
        IExcelService excelService,
        IRepository<PrivateTourRequest> repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IServiceProvider serviceProvider
    ) : base(serviceProvider)
    {
        _excelService = excelService;
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

            int totalChildren = 0;
            int totalAdult = 0;
            foreach(var item in privateTourequestDTO.FamilyDetails)
            {
                totalAdult += item.NumOfAdult * item.TotalFamily;
                totalChildren += item.NumOfChildren * item.TotalFamily;
            }

            if(totalAdult + privateTourequestDTO.NumOfSingleFemale + privateTourequestDTO.NumOfSingleMale != privateTourequestDTO.NumOfAdult)
            {
                result = BuildAppActionResultError(result, "Tổng số người lớn không hợp lệ");
                return result;
            }

            if (totalChildren != privateTourequestDTO.NumOfChildren)
            {
                result = BuildAppActionResultError(result, "Tổng số trẻ em không hợp lệ");
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
                NumOfFamily = privateTourequestDTO.NumOfFamily,
                NumOfSingleMale = privateTourequestDTO.NumOfSingleMale,
                NumOfSingleFemale = privateTourequestDTO.NumOfSingleFemale,
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

            if (privateTourequestDTO.FamilyDetails != null && privateTourequestDTO.FamilyDetails.Count > 0)
            {
                var roomQuantityDetailRepository = Resolve<IRepository<FamilyDetailRequest>>();
                foreach (var detail in privateTourequestDTO.FamilyDetails)
                {
                    await roomQuantityDetailRepository!.Insert(new FamilyDetailRequest
                    {
                        Id = Guid.NewGuid(),
                        PrivateTourRequestId = request.Id,
                        NumOfAdult = detail.NumOfAdult,
                        NumOfChildren = detail.NumOfChildren,
                        TotalFamily = detail.TotalFamily
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
        var roomQuantityDetailRepository = Resolve<IRepository<FamilyDetailRequest>>();
        try
        {
            var data = await _repository.GetAllDataByExpression
            (null, pageNumber,
                pageSize, a => a.CreateDate, false,
                p => p.Tour, p => p.CreateByAccount!, p => p.Province!, p => p.HotelFacilityRating, p => p.RestaurantFacilityRating, p=> p.Tour, p=> p.Commune.District.Province);
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
            var roomQuantityDetailRepository = Resolve<IRepository<FamilyDetailRequest>>();
            var requestedLocationDb = await requestedLocationRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == privateTourRequestDb.Id, 0, 0, null, false, r => r.Province!);
            data.PrivateTourResponse.OtherLocation = requestedLocationDb.Items;
            var roomQuantityDetailDb = await roomQuantityDetailRepository!.GetAllDataByExpression(r => r.PrivateTourRequestId == privateTourRequestDb.Id, 0, 0, null, false, null);
            data.PrivateTourResponse.RoomDetails = roomQuantityDetailDb.Items;
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
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false,a => a.District!.Province!, a => a.ServiceType!, a=> a.FacilityRating!.FacilityType!, a => a.FacilityRating!.Rating!, a => a.Menu, a => a.MaterialPriceHistory.Material);
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a=> a.StartPoint!, a => a.EndPoint!);
                var tourGuideQuotationDetailDb = await tourguideQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionId == item.Id, 0, 0, null, false, q => q.Province!);
                var eventOptionDb = await optionEventRepository!.GetAllDataByExpression(o => o.OptionId == item.Id, 0, 0, null, false, o => o.Event);
                option.OptionEvent = eventOptionDb.Items;
                option.TourguideQuotationDetails = tourGuideQuotationDetailDb.Items;
                option.QuotationDetails = quotationDetailDb.Items!.OrderBy(o => o.StartDate).ToList();
                //option.VehicleQuotationDetails = vehicleQuotationDetailDb.Items!.ToList();
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
                            else if(current.Count == 1)
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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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

                if (dto.TourGuideCosts.Count <= 0)
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
                foreach (var material in dto.MaterialCosts)
                {
                    var materialPrice = await materialPriceHistoryRepository!.GetById(material.MaterialPriceHistoryId);
                    if (materialPrice == null)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá về vật liệu có id {material.MaterialPriceHistoryId}");
                        return result;
                    }
                    materialPriceHistories.Add(materialPrice);
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
                        if (location.HotelOptionRatingOption1 != null)
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
                                && a.FacilityService.ServiceTypeId == Domain.Enum.ServiceType.RESTING
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

                        if (location.HotelOptionRatingOption2 != null)
                        {
                            var listHotel = await facilityRepository!.GetAllDataByExpression(
                               a => a.Communce!.DistrictId == location.DistrictId
                              && location.HotelOptionRatingOption2 == a.FacilityRating!.RatingId &&
                              a.FacilityRating.FacilityTypeId == Domain.Enum.FacilityType.HOTEL,
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
                        foreach (var menuOption in location.MenuQuotations)
                        {
                            foreach (var MenuId in menuOption.MenuIds)
                            {
                                var menu = await menuRepository!.GetByExpression(m => m.Id == MenuId, null);
                                if (menu == null)
                                {
                                    result = BuildAppActionResultError(result, $"Không tìm thấy menu với id {MenuId}");
                                    errorCount++;
                                    break;
                                }
                                var sellRestaurent = await sellPriceRepository!.GetAllDataByExpression(
                                   s => s.MenuId == MenuId,
                                   0, 0, null, false, null
                                   );

                                if (!sellRestaurent.Items!.Any())
                                {
                                    result = BuildAppActionResultError(result, $"Không tìm thấy menu {menu.Name}");
                                    errorCount++;
                                    break;
                                }
                            }

                        }
                        if (errorCount > 0)
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
                        var tourguideCost = await humanResourceFeeService!.GetSalary(dto.TourGuideCosts, true);
                        tourguideTotal = (double)tourguideCost.Result!;
                        tourguideQuotationDetails.Add(new TourguideQuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            IsMainTourGuide = item.ProvinceId == null,
                            Quantity = item.Quantity,
                            NumOfDay = item.NumOfDay,
                            Total = tourguideTotal,
                            ProvinceId = item.ProvinceId,
                            OptionId = option1.Id
                        });

                        tourguideQuotationDetails.Add(new TourguideQuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            IsMainTourGuide = item.ProvinceId == null,
                            Quantity = item.Quantity,
                            NumOfDay = item.NumOfDay,
                            Total = tourguideTotal,
                            ProvinceId = item.ProvinceId,
                            OptionId = option2.Id
                        });
                        tourguideQuotationDetails.Add(new TourguideQuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            IsMainTourGuide = item.ProvinceId == null,
                            Quantity = item.Quantity,
                            NumOfDay = item.NumOfDay,
                            Total = tourguideTotal,
                            ProvinceId = item.ProvinceId,
                            OptionId = option3.Id
                        });
                    }


                    foreach (var item in dto.MaterialCosts)
                    {
                        var materialCostHistoryDb = await materialPriceHistoryRepository!.GetById(item.MaterialPriceHistoryId);
                        quotationDetails.Add(new QuotationDetail
                        {
                            Id = Guid.NewGuid(),
                            MaterialPriceHistoryId = materialCostHistoryDb.Id,
                            OptionQuotationId = option1.Id,
                            MinPrice = materialCostHistoryDb.Price * item.Quantity,
                            MaxPrice = materialCostHistoryDb.Price * item.Quantity,
                            Quantity = item.Quantity
                        });
                    }

                    var eventService = Resolve<IEventService>();
                    if (dto.EventGalas != null && dto.EventGalas.Count > 0)
                    {
                        List<OptionEvent> optionEvents = new List<OptionEvent>();
                        foreach (var location in dto.EventGalas)
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

                                var latestCost = JsonConvert.DeserializeObject<CustomEventStringResponse>(location.CustomEvent);
                                if (latestCost != null)
                                {
                                    eventTotal += (latestCost as CustomEventStringResponse).Total;
                                }
                            }
                        }
                        await eventOptionRepository!.InsertRange(optionEvents);
                    }
                    foreach (var item in dto.provinceServices)
                    {
                        foreach (var location in item.Hotels)
                        {
                            var estimateService = await facilityService!.GetServicePriceRangeByDistrictIdAndRequestId(location.DistrictId, dto.PrivateTourRequestId, 0, 0);
                            var estimate = (ReferencedPriceRangeByProvince)estimateService.Result!;
                            if (location.HotelOptionRatingOption1 != null)
                            {
                                if (location.NumOfSingleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption1 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 2
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfSingleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfSingleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption1 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option1.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 2,
                                            Quantity = location.NumOfSingleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 2 người với phân loại {location.HotelOptionRatingOption1}");
                                    }
                                }

                                if (location.NumOfDoubleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption1 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 4
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfDoubleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfDoubleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption1 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option1.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 4,
                                            Quantity = location.NumOfDoubleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 4 người với phân loại {location.HotelOptionRatingOption1}");
                                    }
                                }

                            }

                            if (location.HotelOptionRatingOption2 != null)
                            {
                                if (location.NumOfSingleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption2 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 2
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfSingleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfSingleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption2 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option2.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 2,
                                            Quantity = location.NumOfSingleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 2 người với phân loại {location.HotelOptionRatingOption2}");
                                    }
                                }

                                if (location.NumOfDoubleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption2 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 4
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfDoubleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfDoubleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption2 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option2.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 4,
                                            Quantity = location.NumOfDoubleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 4 người với phân loại {location.HotelOptionRatingOption2}");
                                    }
                                }

                            }
                            if (location.HotelOptionRatingOption3 != null)
                            {
                                if (location.NumOfSingleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption3 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 2
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfSingleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfSingleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption3 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option3.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 2,
                                            Quantity = location.NumOfSingleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 2 người với phân loại {location.HotelOptionRatingOption3}");
                                    }
                                }

                                if (location.NumOfDoubleRoom > 0)
                                {
                                    var sellPriceHotel = estimate.HotelPrice.DetailedPriceReferences.Where(
                                    a => location.HotelOptionRatingOption3 == a.RatingId
                                    && a.ServiceAvailability == ServiceAvailability.BOTH && a.ServingQuantity == 4
                                    ).FirstOrDefault();
                                    if (sellPriceHotel != null)
                                    {
                                        double min = sellPriceHotel!.MinPrice;

                                        double max = sellPriceHotel!.MaxPrice;
                                        int numOfDay = (int)(location.EndDate - location.StartDate).TotalDays;
                                        double minQuotation = numOfDay * location.NumOfDoubleRoom * min;
                                        double maxQuotation = numOfDay * location.NumOfDoubleRoom * max;
                                        var hotelRating = await facilityRatingRepository!.GetByExpression(a => a.FacilityTypeId == FacilityType.HOTEL && location.HotelOptionRatingOption3 == a.RatingId);
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = option3.Id,
                                            QuantityOfAdult = privateTourRequest.NumOfAdult,
                                            QuantityOfChild = privateTourRequest.NumOfChildren,
                                            ServingQuantity = 4,
                                            Quantity = location.NumOfDoubleRoom,
                                            MinPrice = minQuotation,
                                            MaxPrice = maxQuotation,
                                            FacilityRatingId = hotelRating!.Id,
                                            StartDate = location.StartDate,
                                            EndDate = location.EndDate,
                                            DistrictId = location.DistrictId,
                                            ServiceTypeId = ServiceType.RESTING
                                        });
                                    }
                                    else
                                    {
                                        result = BuildAppActionResultError(result, $"Không tìm thấy báo giá khách sạn cho phòng 4 người với phân loại {location.HotelOptionRatingOption3}");
                                    }
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
                                foreach (var MenuId in dayMenu.MenuIds)
                                {
                                    menu = await menuRepository!.GetByExpression(m => m.Id == MenuId, m => m.FacilityService);
                                    quantity = menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.BOTH ? privateTourRequest.NumOfAdult + privateTourRequest.NumOfChildren :
                                                   menu.FacilityService.ServiceAvailabilityId == ServiceAvailability.ADULT ? privateTourRequest.NumOfAdult : privateTourRequest.NumOfChildren;
                                    totalService = (int)Math.Ceiling((double)(quantity / menu.FacilityService.ServingQuantity));
                                    menuPriceList = await sellPriceRepository!.GetAllDataByExpression(s => s.MenuId == MenuId && totalService >= s.MOQ, 0, 0, null, false, m => m.Menu.FacilityService!.Facility!);
                                    if (menuPriceList.Items != null && menuPriceList.Items.Count > 0)
                                    {
                                        menuPrice = menuPriceList.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault();
                                        quotationDetails.Add(new QuotationDetail
                                        {
                                            Id = Guid.NewGuid(),
                                            OptionQuotationId = dayMenu.option == OptionClass.ECONOMY ? option1.Id : dayMenu.option == OptionClass.MIDDLE ? option2.Id : option3.Id,
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
                                            MenuId = menu.Id,
                                            ServiceTypeId = ServiceType.FOODANDBEVARAGE
                                        });
                                    }
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


                                if (location.QuantityLocationOption1 != null)
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
                                        DistrictId = location.DistrictId,
                                        ServiceTypeId = ServiceType.ATTRACTION
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
                                        DistrictId = location.DistrictId,
                                        ServiceTypeId = ServiceType.ATTRACTION
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
                                        DistrictId = location.DistrictId,
                                        ServiceTypeId = ServiceType.ATTRACTION
                                    });
                                }


                            }


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
                                if (vehicle.OptionClass1 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option1.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
                                        MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                        MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                        StartPointId = (Guid)vehicle.StartPoint!,
                                        EndPointId = (Guid)vehicle.EndPoint!,
                                        StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                        EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                        StartPortId = price.Items.FirstOrDefault().DepartureId,
                                        EndPortId = price.Items.FirstOrDefault().ArrivalId,
                                        VehicleType = vehicle.VehicleType
                                    }); ;

                                }

                                if (vehicle.OptionClass2 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option2.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
                                        MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                        MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                        StartPointId = (Guid)vehicle.StartPoint,
                                        EndPointId = (Guid)vehicle.EndPoint,
                                        StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                        EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                        StartPortId = price.Items.FirstOrDefault().DepartureId,
                                        EndPortId = price.Items.FirstOrDefault().ArrivalId,
                                        VehicleType = vehicle.VehicleType,
                                    });
                                }

                                if (vehicle.OptionClass3 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option3.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
                                        MinPrice = totalPeople * price.Items!.Min(a => a.AdultPrice),
                                        MaxPrice = totalPeople * price.Items!.Max(a => a.AdultPrice),
                                        StartPointId = (Guid)vehicle.StartPoint!,
                                        EndPointId = (Guid)vehicle.EndPoint!,
                                        StartPointDistrictId = vehicle.StartPointDistrict != null ? (Guid)vehicle.StartPointDistrict : null,
                                        EndPointDistrictId = vehicle.EndPointDistrict != null ? (Guid)vehicle.EndPointDistrict : null,
                                        StartPortId = price.Items.FirstOrDefault().DepartureId,
                                        EndPortId = price.Items.FirstOrDefault().ArrivalId,
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
                                if (vehicle.OptionClass1 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option1.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
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

                                if (vehicle.OptionClass2 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option2.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
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

                                if (vehicle.OptionClass3 != null || vehicle.OptionClass1 == null && vehicle.OptionClass2 == null && vehicle.OptionClass3 == null)
                                {
                                    vehicleQuotationDetails.Add(new VehicleQuotationDetail
                                    {
                                        Id = Guid.NewGuid(),
                                        OptionQuotationId = option3.Id,
                                        StartDate = vehicle.StartDate,
                                        EndDate = vehicle.EndDate,
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


                    privateTourRequest.PrivateTourStatusId = PrivateTourStatus.WAITINGFORCUSTOMER;
                    await _repository.Update(privateTourRequest);
                    await optionQuotationRepository!.Insert(option1);
                    await optionQuotationRepository!.Insert(option2);
                    await optionQuotationRepository!.Insert(option3);
                    await quotationDetailRepository!.InsertRange(quotationDetails);
                    await vehicleQuotationDetailRepository!.InsertRange(vehicleQuotationDetails);
                    await tourguideQuotationDetailRepository!.InsertRange(tourguideQuotationDetails);
                    int rowAffected = await _unitOfWork.SaveChangesAsync();
                    if(rowAffected > 0)
                    {
                        scope.Complete();
                    }
                }

            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }

            return result;
        }
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
                await _repository.Update(option!.PrivateTourRequest);
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

    public async Task<AppActionResult> GetPrivateTourRequestByIdForCustomer(string id, int pageNumber, int pageSize)
    {
        var result = new AppActionResult();
        //OptionListResponseForCustomer data = new OptionListResponseForCustomer();
        //var requestLocationRepository = Resolve<IRepository<RequestedLocation>>();
        try
        {
            var accountRepository = Resolve<IRepository<Account>>();
            var accounttDb = await accountRepository!.GetByExpression(a => a.Id == id, null);
            if (accounttDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy khách hàng với id: {id}");
                return result;
            }

            var privateTourRequestDb = await _repository.GetAllDataByExpression(p => p.CreateBy == id, 0, 0, null, false, p => p.CreateByAccount);
            //data.PrivateTourResponse = _mapper.Map<PrivateTourResponseDto>(privateTourRequestDb);
            //var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            //var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
            //var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            //var optionsDb = await optionQuotationRepository!.GetAllDataByExpression(q => q.PrivateTourRequestId == id, 0, 0, null, false, null);
            //if (optionsDb.Items!.Count != 3)
            //{
            //    result.Messages.Add($"Số lượng lựa chọn hiện tại: {optionsDb.Items.Count} không phù hợp");
            //}
            //int fullOption = 0;
            //optionsDb.Items.ForEach(o => fullOption ^= (int)(o.OptionClassId));
            //if (fullOption != 3)
            //{
            //    result.Messages.Add("Danh sách lựa chọn không đủ các hạng mục");
            //}

            //foreach (var item in optionsDb.Items)
            //{
            //    OptionResponseForCustomer option = new OptionResponseForCustomer();
            //    option.OptionQuotation = item;
            //    var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a => a.District!.Province!, a => a.ServiceType!, a => a.FacilityRating!.FacilityType!, a => a.FacilityRating!.Rating!);
            //    var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == item.Id, 0, 0, null, false, a => a.StartPoint!, a => a.EndPoint!);
            //    option.QuotationDetails = quotationDetailDb.Items!.ToList();
            //    option.VehicleQuotationDetails = vehicleQuotationDetailDb.Items!.Where(v => v.EndPointId != null).ToList();
            //    option.InnerProvinceVehicleQuotationDetails = vehicleQuotationDetailDb.Items!.Where(v => v.EndPointId == null).ToList();
            //    if (item.OptionClassId == OptionClass.ECONOMY)
            //        data.Option1 = option;
            //    else if (item.OptionClassId == OptionClass.MIDDLE)
            //        data.Option2 = option;
            //    else data.Option3 = option;
            //}
            //List<Province> provinces = new List<Province>();
            //var list = await requestLocationRepository!.GetAllDataByExpression(a => a.PrivateTourRequestId == id, 0, 0, null, false, a => a.Province!);
            //foreach (var item in list.Items!)
            //{
            //    data.PrivateTourResponse.OtherLocation = list.Items;
            //}
            result.Result = privateTourRequestDb;
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }
        return result;
    }

    public async Task<IActionResult> GetExcelQuotation(Guid privateTourRequestId)
    {
        IActionResult result = new ObjectResult(null) { StatusCode = 200 };
        try
        {
            OptionListResponseDto data = null;
            var privateTourRequest = await GetPrivateTourRequestById(privateTourRequestId);
            if(privateTourRequest.Result != null)
            {
                data = (OptionListResponseDto)privateTourRequest.Result;
                if(data.Option1 == null || data.Option2 == null || data.Option3 == null)
                {
                    result = new ObjectResult($"Số lượng lựa chọn không hợp lệ");
                    return result;
                }
            } else
            {
                result = new ObjectResult($"Không tìm thấy yêu cầu tạo tour với id {privateTourRequestId}");
                return result;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // Create excel file for Option 1
                    var option1ExcelBytes = await CreateExcelFileForOption(data, data.Option1);
                    var option1Entry = zip.CreateEntry("Option1.xlsx");
                    using (var stream = option1Entry.Open())
                    {
                        stream.Write(option1ExcelBytes, 0, option1ExcelBytes.Length);
                    }

                    // Create excel file for Option 2
                    var option2ExcelBytes = await CreateExcelFileForOption(data, data.Option2);
                    var option2Entry = zip.CreateEntry("Option2.xlsx");
                    using (var stream = option2Entry.Open())
                    {
                        stream.Write(option2ExcelBytes, 0, option2ExcelBytes.Length);
                    }

                    // Create excel file for Option 3
                    var option3ExcelBytes = await CreateExcelFileForOption(data, data.Option3);
                    var option3Entry = zip.CreateEntry("Option3.xlsx");
                    using (var stream = option3Entry.Open())
                    {
                        stream.Write(option3ExcelBytes, 0, option3ExcelBytes.Length);
                    }
                }

                memoryStream.Position = 0;
                return new FileContentResult(memoryStream.ToArray(), "application/zip")
                {
                    FileDownloadName = $"Quotation.zip"
                };
            }


        } catch(Exception ex)
        {
            result = null;
        }
        return result;
    }

    private async Task<byte[]> CreateExcelFileForOption(OptionListResponseDto data, OptionResponseDto option)
    {
        using (var package = new ExcelPackage())
        {
            //Sheet 1: Quotation
            //TourInformation
            //header for quotation
            //Quotation of each category
            //total, average, tax, final average price
            // kế toán, Điều hành tour lập biểu
            var worksheetQuotation = package.Workbook.Worksheets.Add("Báo giá");

            // Starting line for the first section
            int currentLine = 1;

            // Insert sections
            currentLine = AddTourInformation(worksheetQuotation, data.PrivateTourResponse, currentLine);
            currentLine = await AddTourInfo(worksheetQuotation, option, currentLine);


            //Sheet 2: Menu
            //Menu  Sáng   Trưa   Chiều
            //Day1  Tự túc Menu1  Menu2
            //Day2   ..   ..     ..
            var worksheetMenu = package.Workbook.Worksheets.Add("Menu");
            await AddMenuInfo(worksheetMenu, option.QuotationDetails.Where(q => q.MenuId != null));

            // Save the new workbook
            return package.GetAsByteArray();
        }
    }
    

    private async Task AddMenuInfo(ExcelWorksheet worksheet, IEnumerable<QuotationDetail> menuQuotations)
    {
        try
        {
            if (worksheet == null) return;
            if (menuQuotations == null || menuQuotations.Count() == 0) return;
            worksheet.Cells["A1:D1"].Merge = true;
            worksheet.Cells["A1"].Value = "MENU DỰ KIẾN CHO ĐOÀN";
            worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["A1"].Style.Font.Bold = true;
            worksheet.Cells["A1"].Style.Font.Size = 14;

            // Tạo các tiêu đề dưới header
            worksheet.Cells["A2"].Value = "Ngày";
            worksheet.Cells["B2"].Value = "Sáng";
            worksheet.Cells["C2"].Value = "Trưa";
            worksheet.Cells["D2"].Value = "Chiều/Tối";
            
            int i = 3;
            DateTime startDate = (DateTime)menuQuotations.Where(m => m.StartDate != null).OrderBy(m => m.StartDate).FirstOrDefault().StartDate;
            var menuByDay = menuQuotations.GroupBy(m => m.StartDate.Value.Date).ToDictionary(key => key, value => value.Select(v => new { v.StartDate, v.Menu }));

                StringBuilder morning = new StringBuilder();
                StringBuilder lunch = new StringBuilder();
                StringBuilder dinner = new StringBuilder();
            List<string> dishNames = new List<string>();
            var menuDishRepository = Resolve<IRepository<MenuDish>>();
            var dishRepository = Resolve<IRepository<Dish>>();
            int option = 1;
            foreach (var kvp in menuByDay)
            {
                var breakfastMenu = kvp.Value.Where(m => m.StartDate.Value.Hour < 10).ToList();
                var lunchMenu = kvp.Value.Where(m => m.StartDate.Value.Hour >= 11 && m.StartDate.Value.Hour <= 14).ToList();
                var dinnerMenu = kvp.Value.Where(m => m.StartDate.Value.Hour > 16).ToList();
                if(breakfastMenu != null && breakfastMenu.Count() > 0)
                {                  
                        option = 1;
                        morning = new StringBuilder();
                        foreach(var menu in breakfastMenu)
                        {
                            morning.AppendLine($"Menu {option++}: {menu.Menu.Name}");
                            dishNames = await GetMenuDish(menu.Menu.Id, menuDishRepository!, dishRepository!);
                            dishNames.ForEach(d => morning.AppendLine($"{d}"));
                        }
                }

                if (lunchMenu != null && lunchMenu.Count() > 0)
                {
                    option = 1;
                    lunch = new StringBuilder();
                    foreach (var menu in lunchMenu)
                    {
                        lunch.AppendLine($"Menu {option++}: {menu.Menu.Name}");
                        dishNames = await GetMenuDish(menu.Menu.Id, menuDishRepository!, dishRepository!);
                        dishNames.ForEach(d => lunch.AppendLine($"{d}"));
                    }
                }

                if (dinnerMenu != null && dinnerMenu.Count() > 0)
                {
                    option = 1;
                    dinner = new StringBuilder();
                    foreach (var menu in dinnerMenu)
                    {
                        dinner.AppendLine($"Menu {option++}: {menu.Menu.Name}");
                        dishNames = await GetMenuDish(menu.Menu.Id, menuDishRepository!, dishRepository!);
                        dishNames.ForEach(d => dinner.AppendLine($"{d}"));
                    }
                }

                worksheet.Cells[i, 1].Value = $"Ngày {Math.Ceiling((kvp.Key.Key - startDate).TotalDays)}";
                worksheet.Cells[i, 2].Value = morning.ToString();
                worksheet.Cells[i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells[i, 3].Value = lunch.ToString();
                worksheet.Cells[i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                worksheet.Cells[i, 4].Value = dinner.ToString();
                worksheet.Cells[i, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                i++;
            }
        } catch(Exception ex)
        {

        }
    }

    private async Task<List<string>> GetMenuDish(Guid id, IRepository<MenuDish> menuDishRepository, IRepository<Dish> dishRepository)
    {
        List<string> dishName = new List<string>();
        try
        {
            var menuDishDb = await menuDishRepository.GetAllDataByExpression(m => m.MenuId == id, 0, 0, null, false, null);
            if(menuDishDb.Items != null && menuDishDb.Items.Count() > 0) 
            {
                List<Guid> dishIds = menuDishDb.Items.DistinctBy(d => d.DishId).Select(d => d.DishId).ToList();
                var dishes = await dishRepository.GetAllDataByExpression(d => dishIds.Contains(d.Id), 0, 0, null, false, null);
                dishName = dishes.Items.Select(d => d.Name).ToList();
            }
        } catch(Exception ex)
        {

        }
        return dishName;
    }

    private async Task<int> AddTourInfo(ExcelWorksheet worksheetQuotation, OptionResponseDto option1, int currentLine)
    {
        int i = 4;
        try
        {
            worksheetQuotation.Cells[currentLine, 1].Value = "NỘI DUNG";
            worksheetQuotation.Cells[currentLine, 1, currentLine + 1, 1].Merge = true;
            worksheetQuotation.Cells[currentLine, 2].Value = "HƯỚNG DẪN CHI";
            worksheetQuotation.Cells[currentLine, 2, currentLine, 4].Merge = true;
            worksheetQuotation.Cells[currentLine, 5].Value = "NỘI DUNG";
            worksheetQuotation.Cells[currentLine, 5, currentLine + 1, 5].Merge = true;
            worksheetQuotation.Cells[currentLine + 1, 2].Value = "Đơn giá";
            worksheetQuotation.Cells[currentLine + 1, 3].Value = "Số lượng";
            worksheetQuotation.Cells[currentLine + 1, 4].Value = "Thành tiền";
            worksheetQuotation.Cells[currentLine + 2, 1].Value = "A";
            worksheetQuotation.Cells[currentLine + 2, 2].Value = "1";
            worksheetQuotation.Cells[currentLine + 2, 3].Value = "2";
            worksheetQuotation.Cells[currentLine + 2, 4].Value = "3=1*2";
            worksheetQuotation.Cells[currentLine + 2, 5].Value = "HOÀN THUẾ";

            worksheetQuotation.Cells[currentLine + 3, 1].Value = "1/KHÁCH SẠN/RESORT";
            worksheetQuotation.Cells[currentLine + 3, 1].Style.Font.Bold = true;

            DateTime startDate = option1.QuotationDetails.Where(q => q.StartDate != null).OrderBy(q => q.StartDate).FirstOrDefault()!.StartDate!.Value;

            foreach(var resting in option1.QuotationDetails.OrderBy(q => q.StartDate).Where(q => q.ServiceTypeId == ServiceType.RESTING))
            {
                worksheetQuotation.Cells[currentLine + i, 1].Value = $"Khách sạn ngày {Math.Ceiling((resting.StartDate.Value.Date - startDate).TotalDays) + 1}";
                worksheetQuotation.Cells[currentLine + i, 2].Value = Math.Ceiling(resting.MaxPrice * 0.001 / (resting.QuantityOfAdult + resting.QuantityOfChild)) * 1000;
                worksheetQuotation.Cells[currentLine + i, 3].Value = resting.QuantityOfAdult + resting.QuantityOfChild;
                worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
                worksheetQuotation.Cells[currentLine + i, 5].Value = getRatingName(resting.FacilityRating.Rating.Name);
                i++;
            }
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "2/ĂN UỐNG";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;

            string meal = null;

            foreach (var resting in option1.QuotationDetails.OrderBy(q => q.StartDate).Where(q => q.ServiceTypeId == ServiceType.FOODANDBEVARAGE))
            {
                if(resting.StartDate.Value.Hour <= 10)
                {
                    meal = "Bữa sáng";
                } else if(resting.StartDate.Value.Hour < 14)
                {
                    meal = "Bữa trưa";
                } else
                {
                    meal = "Bữa tối";
                }
                worksheetQuotation.Cells[currentLine + i, 1].Value = $"{meal} ngày {Math.Ceiling((resting.StartDate.Value.Date - startDate).TotalDays) + 1}";
                worksheetQuotation.Cells[currentLine + i, 2].Value = Math.Ceiling(resting.MaxPrice * 0.001 / (resting.QuantityOfAdult + resting.QuantityOfChild)) * 1000;
                worksheetQuotation.Cells[currentLine + i, 3].Value = resting.QuantityOfAdult + resting.QuantityOfChild;
                worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
                worksheetQuotation.Cells[currentLine + i, 5].Value = getRatingName(resting.FacilityRating.Rating.Name);
                i++;
            }
            int totalPeople = option1.QuotationDetails.Select(q => q.QuantityOfChild + q.QuantityOfAdult).FirstOrDefault();
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "3/THAM QUAN";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;

            foreach (var resting in option1.QuotationDetails.Where(q => q.ServiceTypeId == Domain.Enum.ServiceType.ENTERTAIMENT))
            {
                worksheetQuotation.Cells[currentLine + i, 1].Value = $"Tham quan {resting.Quantity} địa điểm";
                worksheetQuotation.Cells[currentLine + i, 2].Value = resting.MaxPrice / (resting.QuantityOfAdult + resting.QuantityOfChild);
                worksheetQuotation.Cells[currentLine + i, 3].Value = resting.QuantityOfAdult + resting.QuantityOfChild;
                worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
                worksheetQuotation.Cells[currentLine + i, 5].Value = getRatingName(resting.FacilityRating.Rating.Name);
                i++;
            }

            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "4/GÓI GALA + TEAM BUILDING";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;

            if (option1.OptionEvent != null) {
                foreach (var item in option1.OptionEvent)
                {
                    if (!item.CustomEvent.Equals("string"))
                    {
                        CustomEventStringResponse customEvent = JsonConvert.DeserializeObject<CustomEventStringResponse>(item.CustomEvent)!;
                        if (customEvent != null)
                        {
                            worksheetQuotation.Cells[currentLine + i++, 1].Value = customEvent.Name;
                            customEvent.eventDetailPriceHistoryResponses.ForEach(e =>
                            {
                                worksheetQuotation.Cells[currentLine + i, 1].Value = e.Name;
                                worksheetQuotation.Cells[currentLine + i, 2].Value = e.Price;
                                worksheetQuotation.Cells[currentLine + i, 3].Value = e.Quantity;
                                worksheetQuotation.Cells[currentLine + i, 4].Value = e.Quantity * e.Price;
                                i++;
                            });
                        }
                    }

                }
            }

            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "5/DI CHUYỂN";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;

            if (option1.VehicleQuotationDetails != null && option1.VehicleQuotationDetails.Count > 0)
            {
                foreach (var resting in option1.VehicleQuotationDetails)
                {
                    string vehicleName = GetVehicleName(resting.VehicleType);
                    if(resting.VehicleType == VehicleType.PLANE || resting.VehicleType == VehicleType.BOAT)
                    {
                        worksheetQuotation.Cells[currentLine + i, 1].Value = $"{vehicleName} trong {resting.NumOfRentingDay} ngày, đi từ {resting.StartPoint.Name}";
                        worksheetQuotation.Cells[currentLine + i, 2].Value = Math.Ceiling(resting.MaxPrice * 0.001 / (totalPeople)) * 1000;
                        worksheetQuotation.Cells[currentLine + i, 3].Value = resting.NumOfVehicle;
                        worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
                    } else
                    {
                        worksheetQuotation.Cells[currentLine + i, 1].Value = $"{vehicleName} trong {resting.NumOfRentingDay} ngày, đi từ {resting.StartPoint.Name}";
                        worksheetQuotation.Cells[currentLine + i, 2].Value = resting.MaxPrice / (resting.NumOfVehicle == 0 ? 1 : resting.NumOfVehicle);
                        worksheetQuotation.Cells[currentLine + i, 3].Value = resting.NumOfVehicle;
                        worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
                    }
                    i++;
                }
            }

            worksheetQuotation.Cells[currentLine + i, 1].Value = $"Tồng phí tài xế";
            worksheetQuotation.Cells[currentLine + i, 4].Value = option1.OptionQuotation.DriverCost;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "6/CÔNG TÁC PHÍ";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;
            string forLocalTourGuide = "";
            if (option1.TourguideQuotationDetails != null && option1.TourguideQuotationDetails.Count > 0)
            {
                foreach (var tourguide in option1.TourguideQuotationDetails)
                {
                    if (tourguide.ProvinceId != null)
                    {
                        forLocalTourGuide = $"tại {tourguide.Province.Name}";
                    }
                    else
                    {
                        forLocalTourGuide = "";
                    }
                    worksheetQuotation.Cells[currentLine + i, 1].Value = $"{tourguide.Quantity} HDV / Ngày {forLocalTourGuide}";
                    worksheetQuotation.Cells[currentLine + i, 2].Value = tourguide.Total / tourguide.Quantity;
                    worksheetQuotation.Cells[currentLine + i, 3].Value = tourguide.Quantity;
                    worksheetQuotation.Cells[currentLine + i, 4].Value = tourguide.Total;
                    i++;
                }
            }

            worksheetQuotation.Cells[currentLine + i, 1].Value = "Escort";
            worksheetQuotation.Cells[currentLine + i, 2].Value = option1.OptionQuotation.EscortFee;
            worksheetQuotation.Cells[currentLine + i, 3].Value = 1;
            worksheetQuotation.Cells[currentLine + i, 4].Value = option1.OptionQuotation.EscortFee;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "CTP Điều hành";
            worksheetQuotation.Cells[currentLine + i, 2].Value = option1.OptionQuotation.OperatingFee;
            worksheetQuotation.Cells[currentLine + i, 3].Value = 1;
            worksheetQuotation.Cells[currentLine + i, 4].Value = option1.OptionQuotation.OperatingFee;
            i++;

            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "7/CHI PHÍ KHÁC";
            worksheetQuotation.Cells[currentLine + i++, 1].Style.Font.Bold = true;
            MaterialPriceHistory material = null;
            var materialHistoryPriceRepository = Resolve<IRepository<MaterialPriceHistory>>();
            foreach(var item in option1.QuotationDetails.Where(q => q.MaterialPriceHistoryId != null))
            {
                material = await materialHistoryPriceRepository!.GetByExpression(i => i.Id == item.MaterialPriceHistoryId, m => m.Material);
                if(material != null)
                {
                    worksheetQuotation.Cells[currentLine + i, 1].Value = material.Material.Name;
                    worksheetQuotation.Cells[currentLine + i, 2].Value = material.Price;
                    worksheetQuotation.Cells[currentLine + i, 3].Value = item.QuantityOfAdult + item.QuantityOfChild;
                    worksheetQuotation.Cells[currentLine + i, 4].Value = (material.Price) * (item.QuantityOfAdult + item.QuantityOfChild);
                    i++;
                }
            }
            int quantity = option1.OptionQuotation.PrivateTourRequest.NumOfAdult + option1.OptionQuotation.PrivateTourRequest.NumOfChildren;

            var assurancePriceHistoryRepository = Resolve<IRepository<AssurancePriceHistory>>();
            var assurance = await assurancePriceHistoryRepository!.GetByExpression(a => a.Id == option1.OptionQuotation.AssurancePriceHistoryId, a => a.Assurance);
            if(assurance != null)
            {
                worksheetQuotation.Cells[currentLine + i, 1].Value = assurance.Assurance.Name;
                worksheetQuotation.Cells[currentLine + i, 2].Value = assurance.Price;
                worksheetQuotation.Cells[currentLine + i, 3].Value = quantity;
                worksheetQuotation.Cells[currentLine + i, 4].Value = quantity * assurance.Price;
            }
            i++;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "Phí dịch vụ tổ chức";
            worksheetQuotation.Cells[currentLine + i, 2].Value = option1.OptionQuotation.OrganizationCost;
            worksheetQuotation.Cells[currentLine + i, 3].Value = 1;
            worksheetQuotation.Cells[currentLine + i, 4].Value = option1.OptionQuotation.OrganizationCost;
            i++;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "8/ TỔNG CỘNG CHI";
            worksheetQuotation.Cells[currentLine + i, 1].Style.Font.Bold = true;
            worksheetQuotation.Cells[currentLine + i, 4].Value = option1.OptionQuotation.MaxTotal;
            i++;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "GIÁ NET 1 PAX";
            worksheetQuotation.Cells[currentLine + i, 3].Value = quantity; 
            worksheetQuotation.Cells[currentLine + i, 4].Value = Math.Ceiling(option1.OptionQuotation.MaxTotal * 0.001 / quantity) * 1000;
            i++;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "VAT 8%";
            worksheetQuotation.Cells[currentLine + i, 3].Value = 0.08;
            worksheetQuotation.Cells[currentLine + i, 4].Value = Math.Ceiling(option1.OptionQuotation.MaxTotal * 0.001 * 0.08 / quantity) * 1000;
            i++;
            i++;

            worksheetQuotation.Cells[currentLine + i, 1].Value = "GIÁ BÁN";
            worksheetQuotation.Cells[currentLine + i, 3].Value = 1.08;
            worksheetQuotation.Cells[currentLine + i, 4].Value = Math.Ceiling(option1.OptionQuotation.MaxTotal * 0.001 * 1.08 / quantity) * 1000;
            i++;
            //worksheetQuotation.Cells[currentLine + i, 2].Value = resting.MaxPrice / resting.NumOfVehicle;
            //worksheetQuotation.Cells[currentLine + i, 3].Value = resting.NumOfVehicle;
            //worksheetQuotation.Cells[currentLine + i, 4].Value = resting.MaxPrice;
            i++;

        }
        catch (Exception ex)
        {
            return -1;
        }
        return currentLine + i;
    }

    private string getRatingName(string name)
    {
        switch (name)
        {
            case "LODGING":
                return "Nhà nghỉ";
            case "HOTEL_TWOSTAR":
                return "Khách sạn 2 sao";
            case "HOTEL_THREESTAR":
                return "Khách sạn 3 sao";
            case "HOTEL_FOURSTAR":
                return "Khách sạn 4 sao";
            case "HOTEL_FIVESTAR":
                return "Khách sạn 5 sao";
            case "RESTAURENT_CASUAL":
                return "Nhà hàng thông thường";
            case "RESTAURENT_TWOSTAR":
                return "Nhà hàng 2 sao";
            case "RESTAURENT_THREESTAR":
                return "Nhà hàng 3 sao";
            case "RESTAURENT_FOURSTAR":
                return "Nhà hàng 4 sao";
            case "RESTAURENT_FIVESTAR":
                return "Nhà hàng 5 sao";
            case "RESORT":
                return "Khu nghỉ dưỡng";
            case "TOURIST_AREA":
                return "Khu du lịch";
            default:
                return "Unknown rating";
        }
    }

    private string GetVehicleName(VehicleType vehicleType)
    {
        if (vehicleType == VehicleType.BUS)
            return "Xe 45 chỗ";
        else if (vehicleType == VehicleType.COACH)
            return "Xe 29 chỗ";
        else if (vehicleType == VehicleType.LIMOUSINE)
            return "Xe 16 chỗ";
        else if (vehicleType == VehicleType.PLANE)
            return "Vé máy bay";
        else if (vehicleType == VehicleType.BOAT)
            return "Vé phà";
        else if (vehicleType == VehicleType.BICYCLE)
            return "Thuê xe đạp";
        else return "Thuê trực thăng";
    }

    private int AddTourInformation(ExcelWorksheet worksheetQuotation, PrivateTourResponseDto? privateTourResponse, int currentLine)
    {
        try
        {
            worksheetQuotation.Cells[currentLine, 1].Value = "CHIẾT TÍNH TOUR";
            worksheetQuotation.Cells[currentLine, 1, 1, 4].Merge = true;
            worksheetQuotation.Cells[currentLine, 1, 1, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            worksheetQuotation.Cells[currentLine, 1, 1, 4].Style.Font.Bold = true;
            worksheetQuotation.Cells[currentLine, 1, 1, 4].Style.Font.Size = 30;

            worksheetQuotation.Cells[currentLine + 1, 1].Value = "I.THÔNG TIN KHÁCH";
            worksheetQuotation.Cells[currentLine + 1, 1].Style.Font.UnderLine = true;

            worksheetQuotation.Cells[currentLine + 2, 1].Value = "- Tên đoàn khách: ";
            worksheetQuotation.Cells[currentLine + 2, 3].Value = "Quốc tịch: ";
            worksheetQuotation.Cells[currentLine + 2, 4].Value = "VIỆT NAM";
            worksheetQuotation.Cells[currentLine + 3, 1].Value = "- Số lượng khách: ";
            worksheetQuotation.Cells[currentLine + 3, 2].Value = privateTourResponse.NumOfAdult + privateTourResponse.NumOfChildren;
            worksheetQuotation.Cells[currentLine + 3, 3].Value = "Trong đó số trẻ em: ";
            worksheetQuotation.Cells[currentLine + 3, 4].Value = privateTourResponse.NumOfChildren;

            worksheetQuotation.Cells[currentLine + 4, 1].Value = "II. LỘ TRÌNH TOUR";
            worksheetQuotation.Cells[currentLine + 4, 1].Style.Font.UnderLine = true;
            worksheetQuotation.Cells[currentLine + 4, 1].Style.Font.Bold = true;

            worksheetQuotation.Cells[currentLine + 5, 1].Value = "- Tuyến tham quan: ";
            StringBuilder places = new StringBuilder();
            privateTourResponse.OtherLocation!.ForEach(p => places.Append($" - {p.Province.Name}"));
            worksheetQuotation.Cells[currentLine + 5, 2].Value = places.ToString();
            worksheetQuotation.Cells[currentLine + 6, 1].Value = "- Thời gian thực hiện: ";
            worksheetQuotation.Cells[currentLine + 6, 2].Value = $"{privateTourResponse.StartDate.Date} - {privateTourResponse.EndDate.Date}";

            worksheetQuotation.Cells[currentLine + 7, 1].Value = "III. THÔNG TIN TOUR";
            worksheetQuotation.Cells[currentLine + 7, 1].Style.Font.UnderLine = true;
            worksheetQuotation.Cells[currentLine + 7, 1].Style.Font.Bold = true;
            _excelService.SetBorders(worksheetQuotation, worksheetQuotation.Cells[currentLine, 1, currentLine + 7, 4], OfficeOpenXml.Style.ExcelBorderStyle.None, OfficeOpenXml.Style.ExcelBorderStyle.None);


        }
        catch (Exception ex )
        {
            return -1;
        }
        return currentLine + 8;
    }

    public async Task<AppActionResult> GetRoomSuggestion(RoomSuggestionRequest dto)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            int roomForFour = 0;
            int roomForTwo = 0;
            if(dto.FamilyDetails.Count > 0)
            {
                int total = 0;
                foreach (var family in dto.FamilyDetails)
                {
                    total = family.NumOfChildren + family.NumOfAdult;
                    if (total <= 3)
                    {
                        if(family.NumOfChildren > 0)
                        {
                            roomForTwo += family.TotalFamily;
                        }
                        else
                        {
                            roomForFour += family.TotalFamily;
                        }
                    }
                    if (total <= 5) roomForFour += family.TotalFamily;
                    else if(total <= 7)
                    {
                        roomForFour += family.TotalFamily;
                        roomForTwo += family.TotalFamily;
                    }
                    else
                    {
                        int forFour = 0;
                        int forTwo = 0;
                        if (family.NumOfChildren > 0) 
                        { 
                            forFour = total / 5;
                            if(total % 5 > 0)
                            {
                                if (total % 5 > 3) forFour++;
                                else forTwo = total % 5;
                            }
                        }
                        else
                        {
                            forFour = total / 4;
                            if (total % 4 > 0)
                            {
                                if (total % 4 > 2) forFour++;
                                else forTwo = total % 4;
                            }
                        }
                        roomForFour += forFour * family.TotalFamily;
                        roomForTwo += forTwo * family.TotalFamily;
                    }

                }
            }
            roomForFour += dto.NumOfSingleMale / 4;
            if(dto.NumOfSingleMale % 4 != 0)
            {
                if (dto.NumOfSingleMale % 4 > 2)
                {
                    roomForFour++;
                }
                else
                {
                    roomForTwo++;
                }
            }
            

            roomForFour += dto.NumOfSingleFemale / 4;
            if(dto.NumOfSingleFemale % 4 != 0)
            {
                if (dto.NumOfSingleFemale % 4 > 2)
                {
                    roomForFour++;
                }
                else
                {
                    roomForTwo++;
                }
            }

            result.Result = new List<RoomSuggestionResponse>
            {
                new RoomSuggestionResponse
                {
                    RoomSize = 4,
                    NumOfRoom = roomForFour
                },
                new RoomSuggestionResponse
                {
                    RoomSize = 2,
                    NumOfRoom = roomForTwo
                }
            };
        } catch(Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }
        return result;
    }

    public async Task<AppActionResult> GetProvinceOfOption(Guid optionId)
    {
        AppActionResult result = new();
        try
        {
            var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
            var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(v => v.OptionQuotationId == optionId, 0, 0, null, false, v => v.StartPoint, v => v.EndPoint);
            var provinces = vehicleQuotationDetailDb.Items!.OrderBy(v =>v.StartDate)
                .SelectMany(route => new[] { route.StartPoint, route.EndPoint })
                                           .Where(p => p != null)
                                           .DistinctBy(p => p.Id)
                                           .ToList();
            result.Result = provinces;

        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
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