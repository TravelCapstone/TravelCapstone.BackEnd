﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
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
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfDay * room * item.SurchargePercent) / total;
                                }
                                else if (currentPrice * item.SurchargePercent < detailedPriceReference.MinSurChange)
                                {
                                    detailedPriceReference.MinSurChange = (currentPrice * room * numOfDay * item.SurchargePercent) / total;
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
                        Items = priceReferences.Skip(pageNumber - 1).Take(pageSize).ToList()
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
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfMeal * numOfTable * item.SurchargePercent) / total;
                                }
                                else if (detailedPriceReference.MinSurChange < currentPrice)
                                {
                                    detailedPriceReference.MinSurChange = (currentPrice * numOfMeal * numOfTable * item.SurchargePercent) / total;
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
                        Items = priceReference.Skip(pageNumber - 1).Take(pageSize).ToList()
                    };
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetPriceOfVehicle(Guid districtId, Guid privatetourRequestId, int numOfDay, VehicleType vehicleType, int pageNumber,int pageSize)
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
                var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(f => f.Facility!.Communce!.DistrictId == districtId && f.ServiceTypeId == ServiceType.VEHICLE, 0, 0, null, false, f => f.Facility!.FacilityRating!);
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
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfDay * numOfVehicle) / total;
                                }
                                if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                {
                                    detailedPriceReference.MaxPrice = (currentPrice * numOfDay * numOfVehicle * item.SurchargePercent) / total;
                                }
                                else if (detailedPriceReference.MinSurChange < currentPrice)
                                {
                                    detailedPriceReference.MinSurChange = (currentPrice * numOfDay * numOfVehicle * item.SurchargePercent) / total;
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
                        Items = priceReference.Skip(pageNumber - 1).Take(pageSize).ToList()
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
                var facilityDb = await facilityRepository!.GetByExpression(f => f.Id == facilityId);
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
                List<ServiceCostHistoryRecord> sampleData = new List<ServiceCostHistoryRecord>();
                sampleData.Add(new ServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, Price = 4 });
                result = _fileService.GenerateExcelContent<ServiceCostHistoryRecord, Object>(sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION, "ProviderName_ddMMyyyy");

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
    }
}
