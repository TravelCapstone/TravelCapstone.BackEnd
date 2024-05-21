using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class SellPriceHistoryService : GenericBackendService, ISellPriceHistoryService
    {
        private readonly IRepository<SellPriceHistory> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IFileService _fileService;

        public SellPriceHistoryService(
            IRepository<SellPriceHistory> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
        IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<AppActionResult> GetMinMaxPriceOfHotel(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfDay, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRepository = Resolve<IRepository<District>>();
                var districtDb = await districtRepository!.GetById(districtId);
                if (districtDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {districtId}");
                    return result;
                }
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourDb = await privateTourRequestRepository!.GetByExpression(p => p!.Id == privatetourRequestId);
                if (privateTourDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy tour với id {privatetourRequestId}");
                    return result;
                }
                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.DistrictId == districtId && f.Facility.FacilityRatingId == ratingId && f.ServiceTypeId == ServiceType.RESTING, 0, 0, null, false, f => f.Facility!.FacilityRating!);
                if (facilityServiceDb != null && facilityServiceDb!.Items!.Count > 0)
                {
                    var hotelService = facilityServiceDb.Items!.GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }).ToDictionary(g => g.Key, g => g.ToList());
                    double MinPrice = Double.MaxValue;
                    double MaxPrice = 0;
                    double MinSurchange = Double.MaxValue;
                    double MaxSurchange = 0;
                    List<DetailedPriceReference> priceReferences = new List<DetailedPriceReference>();
                    foreach (var kvp in hotelService)
                    {
                        if (kvp.Value.Count == 0)
                        {
                            continue;
                        }
                        MinPrice = Double.MaxValue;
                        MaxPrice = 0;
                        MinSurchange = Double.MaxValue;
                        MaxSurchange = 0;
                        var serviceRating = kvp.Key;
                        double currentPrice;
                        int total = 0;
                        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                        int i = 0;
                        double room = 0;
                        foreach (var item in kvp.Value)
                        {
                            if (i == 0)
                            {
                                detailedPriceReference.ServiceTypeId = item.ServiceTypeId;
                                detailedPriceReference.RatingId = item.Facility!.FacilityRating!.RatingId;
                                detailedPriceReference.ServingQuantity = item.ServingQuantity;
                                detailedPriceReference.ServiceAvailability = item.ServiceAvailabilityId;
                                detailedPriceReference.Unit = item.UnitId;
                                i++;
                            }
                            total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? privateTourDb.NumOfAdult + privateTourDb.NumOfChildren :
                                    detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? privateTourDb.NumOfAdult : privateTourDb.NumOfChildren;
                            var sellPriceHistoryDb = await _repository.GetAllDataByExpression(s => s.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                            room = Math.Ceiling((double)total / item.ServingQuantity);
                            if (sellPriceHistoryDb.Items != null && sellPriceHistoryDb.Items.Count > 0)
                            {
                                currentPrice = sellPriceHistoryDb.Items.OrderByDescending(s => s.Date).ThenByDescending(s => s.MOQ).FirstOrDefault()!.Price;
                                if (currentPrice > detailedPriceReference.MaxPrice)
                                {
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfDay * room) / total;
                                }
                                else if (currentPrice < detailedPriceReference.MinPrice)
                                {
                                    detailedPriceReference.MinPrice = (currentPrice * numOfDay * room) / total;
                                }
                                if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MaxPrice = ((currentPrice * numOfDay * room) + (currentPrice * numOfDay * room * item.SurchargePercent)) / total;
                                }
                                else if (currentPrice * item.SurchargePercent < detailedPriceReference.MinSurChange)
                                {
                                    detailedPriceReference.MinSurChange = ((currentPrice * numOfDay * room) + (currentPrice * numOfDay * room * item.SurchargePercent)) / total;
                                }
                            }
                        }
                        detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                        detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                        priceReferences.Add(detailedPriceReference);
                    }
                    var invalidPriceReferences = priceReferences.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                    invalidPriceReferences.ForEach(item => priceReferences.Remove(item));

                    result.Result = new PagedResult<DetailedPriceReference>
                    {
                        Items = priceReferences,
                    };

                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetPriceOfMeal(Guid districtId, Guid privatetourRequestId, Guid ratingId, int numOfMeal, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRepository = Resolve<IRepository<District>>();
                var districtDb = await districtRepository!.GetById(districtId);
                if (districtDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {districtId}");
                    return result;
                }
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();

                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privatetourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {privatetourRequestId}");
                    return result;
                }

                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(p => p.Facility!.Communce!.DistrictId == districtId && p.Facility.FacilityRatingId == ratingId
                && p.ServiceTypeId == ServiceType.FOODANDBEVARAGE, 0, 0, null, false, p => p.Facility!.FacilityRating!);
                if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                {
                    var restaurantService = facilityServiceDb.Items.GroupBy(s => new { s.ServiceAvailabilityId, s.ServingQuantity, s.Facility!.FacilityRatingId }).ToDictionary(g => g.Key, g => g.ToList());
                    double MinPrice = Double.MaxValue;
                    double MaxPrice = 0;
                    double MinSurchange = Double.MaxValue;
                    double MaxSurchange = 0;
                    double numOfTable = 0;
                    List<DetailedPriceReference> priceReference = new List<DetailedPriceReference>();
                    foreach (var kvp in restaurantService)
                    {
                        if (kvp.Value.Count == 0) continue;
                        MinPrice = Double.MaxValue;
                        MaxPrice = 0;
                        MinSurchange = Double.MaxValue;
                        MaxSurchange = 0;
                        var serviceRating = kvp.Key;
                        double currentPrice;
                        int total = 0;
                        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                        int i = 0;
                        foreach (var item in kvp.Value)
                        {
                            if (i == 0)
                            {
                                detailedPriceReference.ServiceTypeId = item.ServiceTypeId;
                                detailedPriceReference.RatingId = item.Facility!.FacilityRating!.RatingId;
                                detailedPriceReference.ServiceAvailability = item.ServiceAvailabilityId;
                                detailedPriceReference.ServingQuantity = item.ServingQuantity;
                                detailedPriceReference.Unit = item.UnitId;
                                i++;
                            }
                            total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren :
                            detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? privateTourRequestDb.NumOfAdult : privateTourRequestDb.NumOfChildren;

                            numOfTable = Math.Ceiling((double)total / item.ServingQuantity);

                            var sellPriceHistory = await _repository!.GetAllDataByExpression(s => s.Menu!.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                            if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                            {
                                currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                            .ThenByDescending(s => s.MOQ)
                                                            .FirstOrDefault()!.Price;
                                if (currentPrice > detailedPriceReference.MaxPrice)
                                {
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfMeal * numOfTable) / total;
                                }
                                else if (currentPrice < detailedPriceReference.MinPrice)
                                {
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfMeal * numOfTable) / total;
                                }
                                if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MaxPrice = ((currentPrice * numOfMeal * numOfTable) + (currentPrice * numOfMeal * numOfTable * item.SurchargePercent)) / total;
                                }
                                else if (detailedPriceReference.MinSurChange < currentPrice)
                                {
                                    detailedPriceReference.MinSurChange = ((currentPrice * numOfMeal * numOfTable) + (currentPrice * numOfMeal * numOfTable * item.SurchargePercent)) / total;
                                }
                            }
                        }
                        detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                        detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                        priceReference.Add(detailedPriceReference);
                    }
                    var invalidPriceReferences = priceReference.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                    invalidPriceReferences.ForEach(item => priceReference.Remove(item));

                    result.Result = new PagedResult<DetailedPriceReference>
                    {
                        Items = priceReference,
                    };
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetPriceOfVehicle(Guid provinceId, Guid privatetourRequestId, int numOfDay, VehicleType vehicleType, int pageNumber, int pageSize)
        {
            // (range Sức chứa bus * số lướng bus + range sc hơi * sl hơi) / Tổng người
            // (12-15 *2 + 5-6*1) / tổng = 
            AppActionResult result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var provinceDb = await provinceRepository!.GetById(provinceId);
                if (provinceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {provinceId}");
                    return result;
                }
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();

                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privatetourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {privatetourRequestId}");
                    return result;
                }
                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.District!.ProvinceId == provinceId && f.ServiceTypeId == ServiceType.VEHICLE, 0, 0, null, false, f => f.Facility!.FacilityRating!);
                if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                {
                    var vehicleService = facilityServiceDb.Items.GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }).ToDictionary(g => g.Key, g => g.ToList());
                    double MinPrice = Double.MaxValue;
                    double MaxPrice = 0;
                    double MinSurchange = Double.MaxValue;
                    double MaxSurchange = 0;
                    double numOfVehicle = 0;
                    List<DetailedPriceReference> priceReference = new List<DetailedPriceReference>();
                    foreach (var kvp in vehicleService)
                    {
                        if (kvp.Value.Count == 0) continue;
                        MinPrice = Double.MaxValue;
                        MaxPrice = 0;
                        MinSurchange = Double.MaxValue;
                        MaxSurchange = 0;
                        var serviceRating = kvp.Key;
                        double currentPrice;
                        int total = 0;
                        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                        int i = 0;
                        foreach (var item in kvp.Value)
                        {
                            if (i == 0)
                            {
                                detailedPriceReference.ServiceTypeId = item.ServiceTypeId;
                                detailedPriceReference.RatingId = item.Facility!.FacilityRating!.RatingId;
                                detailedPriceReference.ServiceAvailability = item.ServiceAvailabilityId;
                                detailedPriceReference.ServingQuantity = item.ServingQuantity;
                                detailedPriceReference.Unit = item.UnitId;
                                i++;
                            }
                            total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren :
                            detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? privateTourRequestDb.NumOfAdult : privateTourRequestDb.NumOfChildren;

                            numOfVehicle = Math.Ceiling((double)total / item.ServingQuantity);
                            var sellPriceHistory = await _repository!.GetAllDataByExpression(s => s.TransportServiceDetail!.FacilityServiceId == item.Id && s.TransportServiceDetail.VehicleTypeId == vehicleType && s.MOQ <= total, 0, 0, null, false, null);
                            if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                            {
                                currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                            .ThenByDescending(s => s.MOQ)
                                                            .FirstOrDefault()!.Price;
                                if (currentPrice > detailedPriceReference.MaxPrice)
                                {
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfDay * numOfVehicle) / total;
                                }
                                else if (currentPrice < detailedPriceReference.MinPrice)
                                {
                                    detailedPriceReference.MinPrice = (currentPrice * numOfDay * numOfVehicle) / total;
                                }
                                if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MaxPrice = ((currentPrice * numOfDay * numOfVehicle) + (currentPrice * numOfDay * numOfVehicle * item.SurchargePercent)) / total;
                                }
                                else if (detailedPriceReference.MinSurChange < currentPrice)
                                {
                                    detailedPriceReference.MinSurChange = ((currentPrice * numOfDay * numOfVehicle) + (currentPrice * numOfDay * numOfVehicle * item.SurchargePercent)) / total;
                                }
                            }
                        }
                        detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                        detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                        priceReference.Add(detailedPriceReference);
                    }
                    var invalidPriceReferences = priceReference.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                    invalidPriceReferences.ForEach(item => priceReference.Remove(item));

                    result.Result = new PagedResult<DetailedPriceReference>
                    {
                        Items = priceReference,
                    };
                }


            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetReferenceTransportByProvince(Guid startPoint, Guid endPoint, VehicleType vehicleType, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var startPointDb = await provinceRepository!.GetByExpression(p => p!.Id == startPoint);
                var endpointDb = await provinceRepository!.GetByExpression(p => p!.Id == endPoint);
                if (startPointDb == null || endpointDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy nơi bắt đầu {startPoint} hoặc nơi kết thúc {endpointDb}");
                    return result;
                }
                var referenceTransportRepository = Resolve<IRepository<ReferenceTransportPrice>>();
                if (vehicleType == VehicleType.PLANE || vehicleType == VehicleType.BOAT)
                {
                    var priceList = await referenceTransportRepository!.GetAllDataByExpression(a => a.Departure!.Commune!.District!.ProvinceId == startPoint && a.Arrival!.Commune!.District!.ProvinceId == endPoint
                  || a.Departure.Commune.District.ProvinceId == endPoint && a.Arrival!.Commune!.District!.ProvinceId == startPoint
                  , 0, 0, null, false, null
                  );
                    List<DetailedPriceReference> priceReference = new List<DetailedPriceReference>();
                    foreach (var item in priceList!.Items!)
                    {
                        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                        detailedPriceReference.MinPrice = priceList!.Items.Min(a => a.AdultPrice);
                        detailedPriceReference.MaxPrice = priceList!.Items.Max(a => a.AdultPrice);
                        detailedPriceReference.ServingQuantity = 1;
                        detailedPriceReference.ServiceAvailability = ServiceAvailability.BOTH;
                        detailedPriceReference.ServiceTypeId = ServiceType.VEHICLE;
                        priceReference.Add(detailedPriceReference);
                    }
                    var invalidPriceReferences = priceReference.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                    invalidPriceReferences.ForEach(item => priceReference.Remove(item));

                    result.Result = new PagedResult<DetailedPriceReference>
                    {
                        Items = priceReference,
                    };
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAttractionSellPriceRange(Guid districtId, Guid privateTourRequestId, int numOfPlace, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRepository = Resolve<IRepository<District>>();
                var districtDb = await districtRepository!.GetById(districtId);
                if (districtDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {districtId}");
                    return result;
                }
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();

                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privateTourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {privateTourRequestId}");
                    return result;
                }

                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.DistrictId == districtId && f.ServiceTypeId == ServiceType.ENTERTAIMENT, 0, 0, null, false, f => f.Facility.FacilityRating);
                if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                {
                    var hotelService = facilityServiceDb.Items
                    .GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }) // Group by ServiceRating
                    .ToDictionary(g => g.Key, g => g.ToList());
                    double MinPrice = Double.MaxValue;
                    double MaxPrice = 0;
                    double MinSurchange = Double.MaxValue;
                    double MaxSurchange = 0;
                    List<DetailedPriceReference> priceReference = new List<DetailedPriceReference>();
                    foreach (var kvp in hotelService)
                    {
                        if (kvp.Value.Count == 0) continue;
                        MinPrice = Double.MaxValue;
                        MaxPrice = 0;
                        MinSurchange = Double.MaxValue;
                        MaxSurchange = 0;
                        var serviceRating = kvp.Key;
                        double currentPrice;
                        int total = 0;
                        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                        int i = 0;
                        foreach (var item in kvp.Value)
                        {
                            if (i == 0)
                            {
                                detailedPriceReference.ServiceTypeId = item.ServiceTypeId;
                                detailedPriceReference.RatingId = item.Facility!.FacilityRating!.RatingId;
                                detailedPriceReference.ServiceAvailability = item.ServiceAvailabilityId;
                                detailedPriceReference.ServingQuantity = item.ServingQuantity;
                                detailedPriceReference.Unit = item.UnitId;
                                i++;
                            }
                            total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren :
                                    detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? privateTourRequestDb.NumOfAdult : privateTourRequestDb.NumOfChildren;
                            var sellPriceHistory = await _repository!.GetAllDataByExpression(s => s.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                            if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                            {
                                currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                                    .ThenByDescending(s => s.MOQ)
                                                                    .FirstOrDefault()!.Price;
                                if (currentPrice > detailedPriceReference.MaxPrice)
                                {
                                    detailedPriceReference.MaxPrice = currentPrice * numOfPlace;
                                }
                                else if (currentPrice < detailedPriceReference.MinPrice)
                                {
                                    detailedPriceReference.MinPrice = currentPrice * numOfPlace;
                                }

                                if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MaxSurChange = currentPrice * item.SurchargePercent * numOfPlace;
                                }
                                else if (detailedPriceReference.MinSurChange > currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MinSurChange = currentPrice * item.SurchargePercent * numOfPlace;
                                }
                            }
                        }
                        detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                        detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                        priceReference.Add(detailedPriceReference);

                    };
                    var invalidPriceReferences = priceReference.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                    invalidPriceReferences.ForEach(item => priceReference.Remove(item));

                    result.Result = new PagedResult<DetailedPriceReference>
                    {
                        Items = priceReference,
                    };
                }


            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetSellPriceByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var facilityRepository = Resolve<IRepository<Facility>>();
                var facilityDb = await facilityRepository!.GetByExpression(f => f!.Id == facilityId);
                if (facilityDb != null)
                {
                    var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                    var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(fs => fs.FacilityId == facilityDb.Id && fs.ServiceTypeId == serviceTypeId, 0, 0, null, false, null);
                    if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                    {
                        var facilityServiceIds = facilityServiceDb.Items.Select(f => f.Id).ToList();
                        var menuRepository = Resolve<IRepository<Menu>>();
                        var menuDb = await menuRepository!.GetAllDataByExpression(m => facilityServiceIds.Contains(m.FacilityServiceId), 0, 0, null, false, null);
                        var transportServiceDetailRepository = Resolve<IRepository<TransportServiceDetail>>();
                        var transportDb = await transportServiceDetailRepository!.GetAllDataByExpression(m => facilityServiceIds.Contains(m.FacilityServiceId), 0, 0, null, false, null);
                        var menuIds = menuDb.Items!.Select(m => m.Id).ToList();
                        var transportIds = transportDb.Items!.Select(m => m.Id).ToList();
                        var sellPriceHistoryRepository = Resolve<IRepository<SellPriceHistory>>();
                        var sellPriceHistoryDb = await sellPriceHistoryRepository!.GetAllDataByExpression(s => (s.FacilityServiceId != null && facilityServiceIds.Contains((Guid)s.FacilityServiceId))
                                                                                                                    || (s.MenuId != null && menuIds.Contains((Guid)s.MenuId))
                                                                                                                    || (s.TransportServiceDetailId != null && transportIds.Contains((Guid)s.TransportServiceDetailId)),
                                                                                                                    pageNumber, pageSize, s => s.Date, false, s => s.TransportServiceDetail.FacilityService, s => s.FacilityService, s => s.Menu.FacilityService);
                        result.Result = sellPriceHistoryDb;
                    }
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<IActionResult> GetTemplate()
        {
            IActionResult result = null;
            try
            {
                List<SellPriceHistoryRecord> sampleData = new List<SellPriceHistoryRecord>();
                sampleData.Add(new SellPriceHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, Price = 4 });
                result = _fileService.GenerateExcelContent<SellPriceHistoryRecord, Object>("GÍA BÁN",sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION, "ProviderName");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public async Task<IActionResult> GetMenuTemplate()
        {
            IActionResult result = null;
            try
            {
                List<MenuServiceCostHistoryRecord> sampleData = new List<MenuServiceCostHistoryRecord>();
                sampleData.Add(new MenuServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", MenuName = "Menu sáng",Unit = "Bar", MOQ = 1000, Price = 4 });
                result = _fileService.GenerateExcelContent<MenuServiceCostHistoryRecord, Object>("GÍA BÁN THỰC ĐƠN", sampleData, null, SD.ExcelHeaders.MENU_SERVICE_QUOTATION, "ProviderName");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public Task<AppActionResult> UploadQuotation(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> ValidateExcelFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<AppActionResult> GetServiceLatestPrice(Guid facilityServiceId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetById(facilityServiceId);
                if (facilityServiceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy dịch vụ với id {facilityServiceId}");
                    return result;
                }

                var sellPriceDb = await _repository.GetAllDataByExpression(s => s.FacilityServiceId == facilityServiceId, 0, 0, s => s.Date, false, null);
                var sellPriceGroup = sellPriceDb.Items!.GroupBy(s => s.MOQ).ToDictionary(g => g.Key, g => g.ToList());
                List<SellPriceHistory> data = new List<SellPriceHistory>();
                foreach (var kvp in sellPriceGroup)
                {
                    data.Add(kvp.Value.MaxBy(s => s.Date)!);
                }
                result.Result = new PagedResult<SellPriceHistory>
                {
                    Items = data,
                };
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetMenuServiceLatestPrice(Guid menuId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var menuRepository = Resolve<IRepository<Domain.Models.Menu>>();
                var menuDb = await menuRepository!.GetById(menuId);
                if (menuDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy menu với id {menuId}");
                    return result;
                }

                var sellPriceDb = await _repository.GetAllDataByExpression(s => s.MenuId == menuId, 0, 0, s => s.Date, false, null);
                var sellPriceGroup = sellPriceDb.Items!.GroupBy(s => s.MOQ).ToDictionary(g => g.Key, g => g.ToList());
                List<SellPriceHistory> data = new List<SellPriceHistory>();
                foreach (var kvp in sellPriceGroup)
                {
                    data.Add(kvp.Value.MaxBy(s => s.Date)!);
                }
                result.Result = new PagedResult<SellPriceHistory>
                {
                    Items = data,
                };
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetTransportServiceLatestPrice(Guid transportDetailId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var transportServiceDetailRepository = Resolve<IRepository<Domain.Models.TransportServiceDetail>>();
                var transportServiceDetailDb = await transportServiceDetailRepository!.GetById(transportDetailId);
                if (transportDetailId == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy chi tiết phương tiện với id {transportDetailId}");
                    return result;
                }

                var sellPriceDb = await _repository.GetAllDataByExpression(s => s.TransportServiceDetailId == transportDetailId, 0, 0, s => s.Date, false, null);
                var sellPriceGroup = sellPriceDb.Items!.GroupBy(s => s.MOQ).ToDictionary(g => g.Key, g => g.ToList());
                List<SellPriceHistory> data = new List<SellPriceHistory>();
                foreach (var kvp in sellPriceGroup)
                {
                    data.Add(kvp.Value.MaxBy(s => s.Date)!);
                }
                result.Result = new PagedResult<SellPriceHistory>
                {
                    Items = data,
                };
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
        public async Task<AppActionResult> GetAveragePriceOfService(Guid districId, Guid privatetourRequestId, Guid ratingId, ServiceType serviceType, int servingQuantity, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRespository = Resolve<IRepository<District>>();
                var districtDb = await districtRespository!.GetById(districId);
                if (districtDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {districId}");
                    return result;
                }

                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privatetourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {privatetourRequestId}");
                    return result;
                }

                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.DistrictId == districId && f.ServiceTypeId == serviceType &&
                f.ServingQuantity == servingQuantity && f.Facility!.FacilityRating!.Id == ratingId, 0, 0, null, false, f => f.Facility!.FacilityRating!, f => f.Facility!.Communce!.District!.Province!);
                if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                {
                    List<DetailedServicePriceReference> servicePriceReference = new List<DetailedServicePriceReference>();
                    foreach (var kvp in facilityServiceDb.Items)
                    {
                        var serviceRating = kvp.Facility!.FacilityRating;
                        SellPriceHistory currentPrice;
                        double total = 0;
                        double quantityOfService = 0;
                        DetailedServicePriceReference detailedServicePriceReference = new DetailedServicePriceReference();

                        total = kvp.ServiceAvailabilityId == Domain.Enum.ServiceAvailability.BOTH ? privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren :
                          kvp.ServiceAvailabilityId == Domain.Enum.ServiceAvailability.ADULT ? privateTourRequestDb.NumOfAdult : privateTourRequestDb.NumOfChildren;

                        quantityOfService = Math.Ceiling((double)total / kvp.ServingQuantity);
                        
                            var sellPriceHistory = await _repository!.GetAllDataByExpression(s => s.FacilityServiceId == kvp.Id && s.MOQ <= quantityOfService, 0, 0, null, false, p => p.FacilityService!.Facility!.Communce!.District!.Province!);
                            if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                            {
                                currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                               .ThenByDescending(s => s.MOQ)
                                                               .FirstOrDefault()!;
                                detailedServicePriceReference.SellPriceHistory = currentPrice;
                                detailedServicePriceReference.PriceOfPerson = (currentPrice.Price * quantityOfService) / total;
                                detailedServicePriceReference.FacilityServices = kvp;
                            }
                        servicePriceReference.Add(detailedServicePriceReference);

                    }
                        var invalidPriceReferences = servicePriceReference.Where(d => d.PriceOfPerson == 0).ToList();
                        invalidPriceReferences.ForEach(item => servicePriceReference.Remove(item));
                        result.Result = new PagedResult<DetailedServicePriceReference>
                        {
                            Items = servicePriceReference,
                        };
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAveragePriceOfMealService(Guid districId, Guid privatetourRequestId, Guid ratingId, MealType mealType, int servingQuantity, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRespository = Resolve<IRepository<District>>();
                var districtDb = await districtRespository!.GetById(districId);
                if (districtDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy huyện với id {districId}");
                    return result;
                }

                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privatetourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với id {privatetourRequestId}");
                    return result;
                }

                var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.DistrictId == districId && f.ServiceTypeId == ServiceType.FOODANDBEVARAGE &&
                f.ServingQuantity == servingQuantity && f.Facility!.FacilityRating!.Id == ratingId, 0, 0, null, false, f => f.Facility!.FacilityRating!, f => f.Facility!.Communce!.District!.Province!);
                if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                {
                    List<DetailedServiceMealPriceReference> servicePriceReference = new List<DetailedServiceMealPriceReference>();
                    foreach (var kvp in facilityServiceDb.Items)
                    {
                        var serviceRating = kvp.Facility!.FacilityRating;
                        SellPriceHistory currentPrice;
                        double total = 0;
                        double quantityOfService = 0;
                        DetailedServiceMealPriceReference detailedServicePriceReference = new DetailedServiceMealPriceReference();

                        total = kvp.ServiceAvailabilityId == Domain.Enum.ServiceAvailability.BOTH ? privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren :
                          kvp.ServiceAvailabilityId == Domain.Enum.ServiceAvailability.ADULT ? privateTourRequestDb.NumOfAdult : privateTourRequestDb.NumOfChildren;

                        quantityOfService = Math.Ceiling((double)total / kvp.ServingQuantity);

                        var sellPriceHistory = await _repository!.GetAllDataByExpression(s => s.Menu!.FacilityServiceId == kvp.Id && s.Menu.MealTypeId == mealType && s.MOQ <= quantityOfService, 0, 0, null, false, p => p.FacilityService!.Facility!.Communce!.District!.Province!, p => p.Menu!);
                        if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                        {
                            currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                           .ThenByDescending(s => s.MOQ)
                                                           .FirstOrDefault()!;
                            detailedServicePriceReference.SellPriceHistory = currentPrice;
                            detailedServicePriceReference.PriceOfPerson = (currentPrice.Price * quantityOfService) / total;
                            detailedServicePriceReference.FacilityServices = kvp;

                            var menuDishRepository = Resolve<IRepository<MenuDish>>();
                            var menuDishDb = await menuDishRepository!.GetAllDataByExpression(p => p.MenuId == currentPrice.MenuId, 0, 0, null, false, p => p.Dish!);
                            if (menuDishDb.Items != null && menuDishDb.Items.Count > 0)
                            {
                                detailedServicePriceReference.MenuDishes = menuDishDb.Items.ToList();
                            }
                        }

                        servicePriceReference.Add(detailedServicePriceReference);
                        var invalidPriceReferences = servicePriceReference.Where(d => d.PriceOfPerson == 0).ToList();
                        invalidPriceReferences.ForEach(item => servicePriceReference.Remove(item));

                        result.Result = new PagedResult<DetailedServiceMealPriceReference>
                        {
                            Items = servicePriceReference,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetVehiclePriceRange(VehiclePriceRangeRequest dto)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var startPointDb = await provinceRepository!.GetByExpression(p => p!.Id == dto.startPoint);
                var endpointDb = await provinceRepository!.GetByExpression(p => p!.Id == dto.endPoint);
                if (startPointDb == null || (endpointDb == null && (dto.vehicleType == VehicleType.PLANE || dto.vehicleType == VehicleType.BOAT)))
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy nơi bắt đầu {dto.startPoint} hoặc nơi kết thúc {endpointDb}");
                    return result;
                }

                var utility = Resolve<Utility>();

                if(dto.StartDate >= dto.EndDate || dto.StartDate <= utility!.GetCurrentDateTimeInTimeZone())
                {
                    result = BuildAppActionResultError(result, $"Thời gian bắt đầu và thời gian kết thúc không hợp lệ");
                    return result;
                }

                if(dto.Quantity == 0)
                {
                    result = BuildAppActionResultError(result, $"Số lượng hành khách ít nhất là 1 người");
                    return result;
                }
                SuggestedVehicleResponse data = new SuggestedVehicleResponse();
                if (dto.vehicleType == VehicleType.PLANE || dto.vehicleType == VehicleType.BOAT)
                {
                    var referenceTransportRepository = Resolve<IRepository<ReferenceTransportPrice>>();
                    var priceList = await referenceTransportRepository!.GetAllDataByExpression(a => a.Departure!.Commune!.District!.ProvinceId == dto.startPoint && a.Arrival!.Commune!.District!.ProvinceId == dto.endPoint
                  || a.Departure.Commune.District.ProvinceId == dto.endPoint && a.Arrival!.Commune!.District!.ProvinceId == dto.startPoint
                  , 0, 0, null, false, null
                  );
                   
                    if (priceList.Items.Count > 0)
                    {
                        data.suggestedVehicleItems.Add(new SuggestedVehicleItem
                        {
                            VehicleType = dto.vehicleType,
                            Quantity = 1
                        });
                        data.MinCostperPerson = priceList.Items!.OrderBy(p => p.AdultPrice).FirstOrDefault()!.AdultPrice;
                        data.MaxCostperPerson = priceList.Items!.OrderByDescending(p => p.AdultPrice).FirstOrDefault()!.AdultPrice;
                    }
                    else
                    {
                        data.suggestedVehicleItems = new List<SuggestedVehicleItem>();
                        data.MinCostperPerson = 0;
                        data.MaxCostperPerson = 0;
                    }
                }
                else
                {
                    var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                    Dictionary<VehicleType, int> suggestedVehicleList = await GetOptimalVehicleSuggestion(dto.Quantity);
                    int totalDays = (dto.EndDate - dto.StartDate).Days;
                    if (totalDays == 0) totalDays = 1;
                    foreach(var kvp in suggestedVehicleList)
                    {
                        var sellPrice = await sellPriceRepository!.GetAllDataByExpression(s => s.TransportServiceDetail!.VehicleTypeId == kvp.Key && s.TransportServiceDetail.FacilityService.Facility.Communce.District!.ProvinceId == dto.startPoint && s.MOQ <= kvp.Value, 0, 0, null, false, s => s.TransportServiceDetail);
                        var latestPrices = sellPrice.Items.GroupBy(p => p.TransportServiceDetail.FacilityServiceId)
                        .Select(g => g.OrderByDescending(p => p.Date).FirstOrDefault())
                        .ToList();
                        data.suggestedVehicleItems.Add( new SuggestedVehicleItem {
                            VehicleType = kvp.Key,
                            Quantity = kvp.Value
                        } );
                        data.MinCostperPerson += latestPrices.MinBy(p => p.Price).Price * kvp.Value * totalDays;
                        data.MaxCostperPerson += latestPrices.MaxBy(p => p.Price).Price* kvp.Value * totalDays;
                    }

                    data.MinCostperPerson = Math.Ceiling((data.MinCostperPerson / (1000 * dto.Quantity))) * 1000;
                    data.MaxCostperPerson = Math.Ceiling((data.MaxCostperPerson / (1000 * dto.Quantity))) * 1000;
                }

                result.Result = data;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        private async Task<Dictionary<VehicleType, int>> GetOptimalVehicleSuggestion(int quantity)
        {
            Dictionary<VehicleType, int> data = new Dictionary<VehicleType, int>();
            try
            {
                //6 -- 15 -- 29
                if(quantity > 44) {
                    int numOfBus = quantity / 44;
                    quantity -= numOfBus * 44;
                    data.Add(VehicleType.BUS, numOfBus);
                }

                if (quantity > 29)
                {
                    if (data.Count == 0)
                        data.Add(VehicleType.BUS, 1);
                    else data[VehicleType.BUS]++;
                    quantity = 0;
                }

                if (quantity > 15)
                {
                    int numOfBus = quantity / 29;
                    numOfBus = numOfBus == 0 ? 1 : numOfBus;
                    quantity -= numOfBus * 29;
                    data.Add(VehicleType.COACH, numOfBus);
                }

                if(quantity > 6)
                {
                    int numOfBus = quantity / 15;
                    numOfBus = numOfBus == 0 ? 1 : numOfBus;
                    quantity -= numOfBus * 15;
                    data.Add(VehicleType.LIMOUSINE, numOfBus);
                }

                if(quantity > 0)
                {
                    data.Add(VehicleType.CAR, 1);
                }


            } catch(Exception ex)
            {
                data = null;
            }
            return data;
        }
    }
}
