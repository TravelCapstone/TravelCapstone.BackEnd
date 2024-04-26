using AutoMapper;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class FacilityServiceService : GenericBackendService, IFacilityServiceService
    {
        private readonly IRepository<Domain.Models.FacilityService> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public FacilityServiceService(
            IRepository<Domain.Models.FacilityService> repository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetServiceByProvinceIdAndRequestId(Guid Id, Guid requestId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                //var tourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                //var tourRequestDb = await tourRequestRepository!.GetById(requestId);
                //if (tourRequestDb == null)
                //{
                //    result.Result = null;
                //    result.Messages.Add($"Không tìm thấy yêu cầu tạo tour với id {requestId}");
                //    return result;
                //}
                //ReferencedPriceRangeByProvince data = new ReferencedPriceRangeByProvince();
                //var districtRepository = Resolve<IRepository<District>>();
                //var districtDb = await districtRepository!.GetAllDataByExpression(d => d.ProvinceId == Id, 0, 0);
                //if (districtDb.Items == null || districtDb.Items.Count <= 0)
                //{
                //    result.Result = null;
                //    result.Messages.Add($"Không tìm thấy danh sách huyện");
                //    return result;
                //}

                //var communeRepository = Resolve<IRepository<Commune>>();
                //var districtIds = districtDb.Items.Select(s => s.Id);
                //var communeDb = await communeRepository!.GetAllDataByExpression(d => districtIds.Contains(d.DistrictId), 0, 0);
                //if (communeDb.Items == null || communeDb.Items.Count <= 0)
                //{
                //    result.Result = null;
                //    result.Messages.Add($"Không tìm thấy danh sách xã");
                //    return result;
                //}

                //var communeIds = communeDb.Items.Select(s => s.Id);

                //var serviceDb = await _repository.GetAllDataByExpression(g => communeIds.Contains(g.Facility!.CommunceId) && g.IsActive, 0, 0, null, false, g => g.Facility.FacilityRating!);



                //if (serviceDb.Items != null && serviceDb.Items.Count > 0)
                //{
                //    var hotelPrice = await GetTypePriceReference(Domain.Enum.ServiceType.RESTING, tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                //    var restaurantPrice = await GetTypePriceReference(Domain.Enum.ServiceType.FOODANDBEVARAGE, tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                //    var entertainmentPrice = await GetTypePriceReference(Domain.Enum.ServiceType.ENTERTAIMENT, tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                //    //var vehicleSupplyPrice = await GetTypePriceReference(Domain.Enum.ServiceType., tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                //    data.HotelPrice = hotelPrice;
                //    data.RestaurantPrice = restaurantPrice;
                //    data.EntertainmentPrice = entertainmentPrice;
                //    //data.VehicleSupplyPrice = vehicleSupplyPrice;
                //}

                //result.Result = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public Task<List<FaicilityServiceCategory>> GetServiceRatingByServiceTypeAndCommuneId(Domain.Enum.ServiceType serviceType, IEnumerable<Domain.Models.FacilityService> services)
        {
            List<FaicilityServiceCategory> serviceRatings = null;
            try
            {
                //    serviceRatings = services.Where(s => s.ServiceTypeId == serviceType).Select(s => new FaicilityServiceCategory
                //    {
                //        RatingId = s.Facility.FacilityRating.RatingId,
                //        ServiceTypeId = s.ServiceTypeId
                //    }).ToList();
            }
            catch (Exception ex)
            {
                serviceRatings = new List<FaicilityServiceCategory>();
            }
            return Task.FromResult(serviceRatings);
        }


        public async Task<PriceReference> GetTypePriceReference(Domain.Enum.ServiceType type, int NumOfAdult, int NumOfChild, IEnumerable<Domain.Models.FacilityService> serviceDb)
        {
            PriceReference priceReference = null;
            //try
            //{
            //    var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
            //    priceReference = new PriceReference(type);
            //    var hotelServiceRating = await GetServiceRatingByServiceTypeAndCommuneId(priceReference.ServiceType, serviceDb);
            //    var hotelService = serviceDb
            //        .Where(s => hotelServiceRating.Contains(new FaicilityServiceCategory
            //        {
            //            ServiceTypeId = s.ServiceTypeId,
            //            RatingId = s.Facility!.FacilityRating!.RatingId
            //        }))
            //        .GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }) // Group by ServiceRating
            //        .ToDictionary(g => g.Key, g => g.ToList());

            //    foreach (var kvp in hotelService)
            //    {
            //        var serviceRating = kvp.Key;
            //        double MinPrice = Double.MaxValue;
            //        double MaxPrice = 0;
            //        double MinSurchange = Double.MaxValue;
            //        double MaxSurchange = 0;
            //        double currentPrice;
            //        int total = 0;
            //        DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
            //        int i = 0;
            //        foreach (var item in kvp.Value)
            //        {
            //            if (i == 0)
            //            {
            //                detailedPriceReference.ServiceTypeId = item.ServiceTypeId;
            //                detailedPriceReference.RatingId = item.Facility!.FacilityRating!.RatingId;
            //                detailedPriceReference.ServiceAvailability = item.ServiceAvailabilityId;
            //                detailedPriceReference.ServingQuantity = item.ServingQuantity;
            //                detailedPriceReference.Unit = item.UnitId;
            //                i++;
            //            }
            //            total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? NumOfAdult + NumOfChild :
            //                    detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? NumOfAdult : NumOfChild;
            //            var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(s => s.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0);
            //            if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
            //            {
            //                currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
            //                                                    .ThenByDescending(s => s.MOQ)
            //                                                    .FirstOrDefault()!.Price;
            //                if (currentPrice > MaxPrice)
            //                {
            //                    MaxPrice = currentPrice;
            //                }
            //                else if (currentPrice < MinPrice)
            //                {
            //                    MinPrice = currentPrice;
            //                }

            //                if (MinSurchange < currentPrice * item.SurchargePercent)
            //                {
            //                    MinSurchange = currentPrice * item.SurchargePercent;
            //                }
            //                else
            //                {
            //                    MaxSurchange = currentPrice * item.SurchargePercent;
            //                }
            //            }
            //        }

            //        detailedPriceReference.MinPrice = MinPrice;
            //        detailedPriceReference.MaxPrice = MaxPrice;
            //        detailedPriceReference.MinSurChange = MinSurchange;
            //        detailedPriceReference.MaxSurChange = MaxSurchange;

            //        priceReference.DetailedPriceReferences.Add(detailedPriceReference);

            //    };
            //}
            //catch (Exception ex)
            //{

            //}
            return priceReference;
        }

        public async Task<AppActionResult> GetServiceByProvinceIdAndServiceType(Guid Id, Domain.Enum.ServiceType type, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRepository = Resolve<IRepository<District>>();
                var districtListDb = await districtRepository!.GetAllDataByExpression(d => d.ProvinceId == Id, 0,0,null,false,null);
                if (districtListDb == null || districtListDb.Items!.Count == 0)
                {
                    return result;
                }
                var districtIds = districtListDb.Items.Select(d => d.Id);
                var communeRepository = Resolve<IRepository<Commune>>();
                var communeListDb = await communeRepository!.GetAllDataByExpression(c => districtIds.Contains(c.DistrictId), pageNumber, pageSize, null, false, null);
                if (communeListDb == null || communeListDb.Items!.Count == 0)
                {
                    return result;
                }
                var communeIds = communeListDb.Items.Select(d => d.Id);
                //var serviceListDb = await _repository.GetAllDataByExpression(s => communeIds.Contains(s.CommunceId) && s.Type== type, 1, Int32.MaxValue);
                //  result.Result = serviceListDb;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetServiceByFacilityId(Guid Id, int pageNumber, int pageSize)
        {
            AppActionResult result = new();
            try
            {
                result.Result = await _repository.GetAllDataByExpression(a => a.FacilityId == Id, pageNumber, pageSize, null, false, a => a.Facility!.Communce!.District!.Province!, a => a.Facility!.FacilityRating!.Rating!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);

            }
            return result;
        }
    }
}
