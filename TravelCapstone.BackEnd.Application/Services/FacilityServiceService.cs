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
using static TravelCapstone.BackEnd.Common.DTO.Response.MapInfo;

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

        public async Task<AppActionResult> GetServicePriceRangeByDistrictIdAndRequestId(Guid Id, Guid requestId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var tourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var tourRequestDb = await tourRequestRepository!.GetById(requestId);
                if (tourRequestDb == null)
                {
                    result.Result = null;
                    result.Messages.Add($"Không tìm thấy yêu cầu tạo tour với id {requestId}");
                    return result;
                }
                ReferencedPriceRangeByProvince data = new ReferencedPriceRangeByProvince();
                var districtRepository = Resolve<IRepository<District>>();
                var districtDb = await districtRepository!.GetById(Id);
                if (districtDb == null)
                {
                    result.Result = null;
                    result.Messages.Add($"Không tìm thấy huyện với id {Id}");
                    return result;
                }

                var communeRepository = Resolve<IRepository<Commune>>();
                var communeDb = await communeRepository!.GetAllDataByExpression(d => d.DistrictId == districtDb.Id, 0, 0, null, false, null);
                if (communeDb.Items == null || communeDb.Items.Count <= 0)
                {
                    result.Result = null;
                    result.Messages.Add($"Không tìm thấy danh sách xã");
                    return result;
                }

                var communeIds = communeDb.Items.Select(s => s.Id);

                var serviceDb = await _repository.GetAllDataByExpression(g => communeIds.Contains(g.Facility!.CommunceId) && g.IsActive, 0, 0, null, false, g => g.Facility.FacilityRating!);



                if (serviceDb.Items != null && serviceDb.Items.Count > 0)
                {
                    var hotelPrice = await GetTypePriceReference(Domain.Enum.ServiceType.RESTING, tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                    var restaurantPrice = await GetMenuTypePriceReference(tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                    var entertainmentPrice = await GetTypePriceReference(Domain.Enum.ServiceType.ENTERTAIMENT, tourRequestDb.NumOfAdult, tourRequestDb.NumOfChildren, serviceDb.Items);
                    data.HotelPrice = hotelPrice;
                    data.RestaurantPrice = restaurantPrice;
                    data.EntertainmentPrice = entertainmentPrice;
                }

                result.Result = data;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public Task<List<Domain.Enum.Rating>> GetFacilityRating(Domain.Enum.ServiceType serviceType, IEnumerable<Domain.Models.FacilityService> services)
        {
            List<Rating> serviceRatings = null;
            try
            {
                serviceRatings = services.Where(s => s.ServiceTypeId == serviceType).Select(s => s.Facility.FacilityRating.RatingId).Distinct().ToList();
            }
            catch (Exception ex)
            {
                serviceRatings = new List<Rating>();
            }
            return Task.FromResult(serviceRatings);
        }


        public async Task<PriceReference> GetTypePriceReference(Domain.Enum.ServiceType type, int NumOfAdult, int NumOfChild, IEnumerable<Domain.Models.FacilityService> serviceDb)
        {
            PriceReference priceReference = null;
            try
            {
                var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                priceReference = new PriceReference(type);
                var hotelServiceRating = await GetFacilityRating(priceReference.ServiceType, serviceDb);
                var hotelService = serviceDb
                    .Where(s => hotelServiceRating.Contains(s.Facility!.FacilityRating!.RatingId) && s.ServiceTypeId == type)
                    .GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }) // Group by ServiceRating
                    .ToDictionary(g => g.Key, g => g.ToList());
                    double MinPrice = Double.MaxValue;
                    double MaxPrice = 0;
                    double MinSurchange = Double.MaxValue;
                    double MaxSurchange = 0;
                foreach (var kvp in hotelService)
                {
                    if(kvp.Value.Count == 0) continue;
                    MinPrice = Double.MaxValue;
                    MaxPrice = 0;
                    MinSurchange = Double.MaxValue;
                    MaxSurchange = 0;
                    var serviceRating = kvp.Key;
                    double currentPrice;
                    int total = 0;
                    DetailedPriceReference detailedPriceReference = new DetailedPriceReference();
                    int i = 0;
                    if(kvp.Value.Count > 0)
                    {
                        detailedPriceReference.ServiceTypeId = kvp.Value[0].ServiceTypeId;
                        detailedPriceReference.RatingId = kvp.Value[0].Facility!.FacilityRating!.RatingId;
                        detailedPriceReference.ServiceAvailability = kvp.Value[0].ServiceAvailabilityId;
                        detailedPriceReference.ServingQuantity = kvp.Value[0].ServingQuantity;
                        detailedPriceReference.Unit = kvp.Value[0].UnitId;
                        total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? NumOfAdult + NumOfChild :
                                detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? NumOfAdult : NumOfChild;
                        total = (int)Math.Ceiling((double)(total / detailedPriceReference.ServingQuantity));
                        if (total == 0) continue;
                        foreach (var item in kvp.Value)
                        {
                                var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(s => s.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                                if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                                {
                                    currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                                        .ThenByDescending(s => s.MOQ)
                                                                        .FirstOrDefault()!.Price;
                                    if (currentPrice > detailedPriceReference.MaxPrice)
                                    {
                                        detailedPriceReference.MaxPrice = currentPrice;
                                    }
                                    else if (currentPrice < detailedPriceReference.MinPrice)
                                    {
                                        detailedPriceReference.MinPrice = currentPrice;
                                    }

                                    if (detailedPriceReference.MaxPrice < currentPrice * item.SurchargePercent)
                                    {
                                        detailedPriceReference.MaxPrice = currentPrice * item.SurchargePercent;
                                    }
                                    else
                                    {
                                        detailedPriceReference.MinSurChange = currentPrice * item.SurchargePercent;
                                    }
                                }
                            }
                        detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                        detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                        priceReference.DetailedPriceReferences.Add(detailedPriceReference);
                    }
                    

                };
                var invalidPriceReferences = priceReference.DetailedPriceReferences.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                invalidPriceReferences.ForEach(item => priceReference.DetailedPriceReferences.Remove(item));
            }
            catch (Exception ex)
            {

            }
            return priceReference;
        }
        public async Task<PriceReference> GetMenuTypePriceReference(int NumOfAdult, int NumOfChild, IEnumerable<Domain.Models.FacilityService> serviceDb)
        {
            PriceReference priceReference = null;
            try
            {
                var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                priceReference = new PriceReference(ServiceType.FOODANDBEVARAGE);
                var hotelServiceRating = await GetFacilityRating(priceReference.ServiceType, serviceDb);
                var hotelService = serviceDb
                    .Where(s => hotelServiceRating.Contains(s.Facility!.FacilityRating!.RatingId) && s.ServiceTypeId == ServiceType.FOODANDBEVARAGE)
                    .GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }) // Group by ServiceRating
                    .ToDictionary(g => g.Key, g => g.ToList());
                double MinPrice = Double.MaxValue;
                double MaxPrice = 0;
                double MinSurchange = Double.MaxValue;
                double MaxSurchange = 0;
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
                    if(kvp.Value.Count > 0)
                    {
                        detailedPriceReference.ServiceTypeId = kvp.Value[0].ServiceTypeId;
                        detailedPriceReference.RatingId = kvp.Value[0].Facility!.FacilityRating!.RatingId;
                        detailedPriceReference.ServiceAvailability = kvp.Value[0].ServiceAvailabilityId;
                        detailedPriceReference.ServingQuantity = kvp.Value[0].ServingQuantity;
                        detailedPriceReference.Unit = kvp.Value[0].UnitId;
                        total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? NumOfAdult + NumOfChild :
                                    detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? NumOfAdult : NumOfChild;
                        if(total > 0)
                        {
                            total = (int)Math.Ceiling((double)((double)total /detailedPriceReference.ServingQuantity));
                            foreach (var item in kvp.Value)
                            {
                                var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(s => s.Menu.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                                if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                                {
                                    currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                                        .ThenByDescending(s => s.MOQ)
                                                                        .FirstOrDefault()!.Price;
                                    if (currentPrice > detailedPriceReference.MaxPrice)
                                    {
                                        detailedPriceReference.MaxPrice = currentPrice;
                                    }
                                    else if (currentPrice < detailedPriceReference.MinPrice)
                                    {
                                        detailedPriceReference.MinPrice = currentPrice;
                                    }

                                    if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                    {
                                        detailedPriceReference.MaxSurChange = currentPrice * item.SurchargePercent;
                                    }
                                    else if (detailedPriceReference.MinSurChange > currentPrice * item.SurchargePercent)
                                    {
                                        detailedPriceReference.MinSurChange = currentPrice * item.SurchargePercent;
                                    }
                                }
                            }
                            detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                            detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                            priceReference.DetailedPriceReferences.Add(detailedPriceReference);
                        }
                    }
                };
                var invalidPriceReferences = priceReference.DetailedPriceReferences.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                invalidPriceReferences.ForEach(item => priceReference.DetailedPriceReferences.Remove(item));
            }
            catch (Exception ex)
            {

            }
            return priceReference;
        }
        public async Task<PriceReference> GetTransportTypePriceReference(int NumOfAdult, int NumOfChild, IEnumerable<Domain.Models.FacilityService> serviceDb)
        {
            PriceReference priceReference = null;
            try
            {
                var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                priceReference = new PriceReference(ServiceType.VEHICLE);
                var hotelServiceRating = await GetFacilityRating(priceReference.ServiceType, serviceDb);
                var hotelService = serviceDb
                    .Where(s => hotelServiceRating.Contains(s.Facility!.FacilityRating!.RatingId) && s.ServiceTypeId == ServiceType.VEHICLE)
                    .GroupBy(s => new { s.ServiceAvailabilityId, s.Facility!.FacilityRating!.RatingId, s.ServingQuantity }) // Group by ServiceRating
                    .ToDictionary(g => g.Key, g => g.ToList());
                double MinPrice = Double.MaxValue;
                double MaxPrice = 0;
                double MinSurchange = Double.MaxValue;
                double MaxSurchange = 0;
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
                    if(kvp.Value.Count > 0)
                    {
                        detailedPriceReference.ServiceTypeId = kvp.Value[0].ServiceTypeId;
                        detailedPriceReference.RatingId = kvp.Value[0].Facility!.FacilityRating!.RatingId;
                        detailedPriceReference.ServiceAvailability = kvp.Value[0].ServiceAvailabilityId;
                        detailedPriceReference.ServingQuantity = kvp.Value[0].ServingQuantity;
                        detailedPriceReference.Unit = kvp.Value[0].UnitId;
                        total = detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.BOTH ? NumOfAdult + NumOfChild :
                                detailedPriceReference.ServiceAvailability == Domain.Enum.ServiceAvailability.ADULT ? NumOfAdult : NumOfChild;
                        if(total > 0)
                        {
                            total = (int)Math.Ceiling((double)(total / detailedPriceReference.ServingQuantity));
                            foreach (var item in kvp.Value)
                            {


                                var sellPriceHistory = await sellPriceRepository!.GetAllDataByExpression(s => s.TransportServiceDetail.FacilityServiceId == item.Id && s.MOQ <= total, 0, 0, null, false, null);
                                if (sellPriceHistory.Items != null && sellPriceHistory.Items.Count > 0)
                                {
                                    currentPrice = sellPriceHistory.Items.OrderByDescending(s => s.Date)
                                                                        .ThenByDescending(s => s.MOQ)
                                                                        .FirstOrDefault()!.Price;
                                    if (currentPrice > detailedPriceReference.MaxPrice)
                                    {
                                        detailedPriceReference.MaxPrice = currentPrice;
                                    }
                                    else if (currentPrice < detailedPriceReference.MinPrice)
                                    {
                                        detailedPriceReference.MinPrice = currentPrice;
                                    }

                                    if (detailedPriceReference.MaxSurChange < currentPrice * item.SurchargePercent)
                                    {
                                        detailedPriceReference.MaxSurChange = currentPrice * item.SurchargePercent;
                                    }
                                    else if (detailedPriceReference.MinSurChange > currentPrice * item.SurchargePercent)
                                    {
                                        detailedPriceReference.MinSurChange = currentPrice * item.SurchargePercent;
                                    }
                                }
                            }
                            detailedPriceReference.MinSurChange = Math.Min(detailedPriceReference.MinSurChange, detailedPriceReference.MaxSurChange);
                            detailedPriceReference.MinPrice = Math.Min(detailedPriceReference.MinPrice, detailedPriceReference.MaxPrice);
                            priceReference.DetailedPriceReferences.Add(detailedPriceReference);
                        }
                    }
                    

                };
                var invalidPriceReferences = priceReference.DetailedPriceReferences.Where(d => d.MinPrice == 0 && d.MaxPrice == 0).ToList();
                invalidPriceReferences.ForEach(item => priceReference.DetailedPriceReferences.Remove(item));
            }
            catch (Exception ex)
            {

            }
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

        public async Task<AppActionResult> GetListServiceForPlan(Guid privateTourRequestId, Guid quotationDetailId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
                var privateTourRequestDb = await privateTourRequestRepository!.GetById(privateTourRequestId);
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy tour yêu cầu với id {privateTourRequestId}");
                    return result;
                }
                var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
                var quotationDetailDb = await quotationDetailRepository!.GetById(quotationDetailId);
                if (quotationDetailDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy bản giá yêu cầu với id {quotationDetailId}");
                    return result;
                }
                if (privateTourRequestDb.NumOfAdult > quotationDetailDb.QuantityOfAdult && privateTourRequestDb.NumOfChildren > quotationDetailDb.QuantityOfChild)
                {
                    result.Result = await _repository.GetAllDataByExpression(p => p.Facility!.FacilityRatingId == quotationDetailDb.FacilityRatingId && p.ServiceTypeId == quotationDetailDb.ServiceTypeId, pageNumber , pageSize, null, false, p => p.Facility!); 
                }
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllFacilityRating()
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var facilityRatingRepository = Resolve<IRepository<FacilityRating>>();
                result.Result = await facilityRatingRepository!.GetAllDataByExpression(null,0 ,0, null, false, p => p.Rating!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetRestaurantRating(Guid Id)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var facilityServiceDb = await _repository.GetAllDataByExpression(p => p.Id == Id && p.ServiceTypeId == ServiceType.FOODANDBEVARAGE, 0 , 0, null, false,
                    p => p.Facility!.FacilityRating!,
                    p => p.Facility!.Communce!.District!.Province!,
                    p => p.Facility!.ServiceProvider!
                    , p => p.ServiceAvailability!, p =>  p.ServiceType!);
                if (facilityServiceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Nhà hàng với id {Id} không tìm thấy");
                    return result;
                }
                result.Result = facilityServiceDb;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
            }
            return result;
        }
    }
}
