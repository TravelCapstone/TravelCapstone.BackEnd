using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class FacilityService : GenericBackendService, IFacilityService
    {
        private readonly IMapper _mapper;
        private IRepository<Facility> _repository;
        private IUnitOfWork _unitOfWork;

        public FacilityService(
            IRepository<Facility> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppActionResult> GetAllFacility(int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
               result.Result = await _repository.GetAllDataByExpression(null, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetAllFacilityByRatingId(FilterLocation filter, Rating ratingId, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.District!.ProvinceId == filter.ProvinceId &&  a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                if (filter.DistrictId != null && filter.CommuneId == null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.DistrictId == filter.DistrictId && a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);

                }
                else if (filter.DistrictId != null && filter.CommuneId != null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.CommunceId == filter.CommuneId && a.FacilityRating!.RatingId == ratingId, pageNumber, pageSize, null, false, a => a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetFacilityAndPortInformation(FacilityAndPortRequest dto)
        {
            AppActionResult result = new();
            try
            {
                var optionRepository = Resolve<IRepository<OptionQuotation>>();
                var optiondb = await optionRepository.GetById(dto.optionId);
                if (optiondb == null) {
                    result = BuildAppActionResultError(result, $"Không tìm thấy lựa chọn với id {dto.optionId}");
                    return result;  
                }

                var vehicleQuotationDetailRepository = Resolve<IRepository<VehicleQuotationDetail>>();
                List<PortFacilityInformation> portFacilityInformations = new List<PortFacilityInformation>();
                var vehicleQuotationDetailDb = await vehicleQuotationDetailRepository!.GetAllDataByExpression(v => v.OptionQuotationId == dto.optionId, 0, 0, null, false, v => v.StartPort, v=> v.EndPort);
                if (vehicleQuotationDetailDb.Items != null && vehicleQuotationDetailDb.Items.Count > 0)
                {
                    foreach(var vehicle in vehicleQuotationDetailDb.Items)
                    {
                        if(vehicle.StartPortId != null)
                        {
                            portFacilityInformations.Add(new PortFacilityInformation
                            {
                                Id = (Guid)vehicle.StartPortId,
                                Name = vehicle.StartPort.Name,
                                Address = vehicle.StartPort.Address,
                                IsFacility = false
                            });
                        }

                        if (vehicle.EndPortId != null)
                        {
                            portFacilityInformations.Add(new PortFacilityInformation
                            {
                                Id = (Guid)vehicle.EndPortId,
                                Name = vehicle.EndPort.Name,
                                Address = vehicle.EndPort.Address,
                                IsFacility = false
                            });
                        }
                    }
                }

                var quotationDetailRepository = Resolve<IRepository<QuotationDetail>>();
                var quotationDetailDb = await quotationDetailRepository!.GetAllDataByExpression(q => q.OptionQuotationId == dto.optionId && q.MenuId != null, 0, 0, null, false, q => q.Menu.FacilityService.Facility);
                if(quotationDetailDb.Items != null && quotationDetailDb.Items.Count > 0)
                {
                    foreach (var quotation in quotationDetailDb.Items)
                    {
                        portFacilityInformations.Add(new PortFacilityInformation
                        {
                            Id = quotation.Menu.FacilityService.FacilityId,
                            Name = quotation.Menu.FacilityService.Facility.Name,
                            Address = quotation.Menu.FacilityService.Facility.Address,
                            IsFacility = true
                        });
                    }
                }

                if(dto.planLocations != null && dto.planLocations.Count > 0)
                {
                    var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                    foreach (var planLocation in dto.planLocations)
                    {
                        var sellPriceDb = await sellPriceRepository!.GetByExpression(p => p.Id == planLocation.SellPriceHistoryId && p.FacilityServiceId != null, p => p.FacilityService.Facility);
                        if(sellPriceDb != null)
                        {
                            portFacilityInformations.Add(new PortFacilityInformation
                            {
                                Id = sellPriceDb.FacilityService.FacilityId,
                                Name = sellPriceDb.FacilityService.Facility.Name,
                                Address = sellPriceDb.FacilityService.Facility.Address,
                                IsFacility = true
                            });
                        }
                    }
                }

                result.Result = portFacilityInformations.DistinctBy(p => p.Id).ToList();    
            } catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetFacilityByProvinceId(FilterLocation filter, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                //var communeRepository = Resolve<IRepository<Commune>>();
                //var communeDb = await communeRepository!.GetAllDataByExpression(c => c.District!.ProvinceId == provinceId, 0, 0, null, false, null);
                //if(communeDb.Items != null & communeDb.Items!.Count > 0)
                //{
                //    var communeIds = communeDb.Items!.Select(c => c.Id);
                //    var facilityDb = await _repository.GetAllDataByExpression(f => communeIds.Contains(f.CommunceId), pageNumber, pageSize, null, false, a=> a.FacilityRating!.Rating!, a => a.Communce!.District!.Province!);
                //    result.Result = facilityDb;
                //}
                if (filter.DistrictId != null && filter.CommuneId == null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.DistrictId == filter.DistrictId, pageNumber, pageSize, null, false, a => a.Communce!.District!.Province!, a => a.FacilityRating!.Rating!, a => a.FacilityRating!.FacilityType!);
                }
                else if (filter.DistrictId != null && filter.CommuneId != null)
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.CommunceId == filter.CommuneId, pageNumber, pageSize, null, false, a => a.Communce!.District!.Province!, a => a.FacilityRating!.Rating!, a => a.FacilityRating!.FacilityType!);
                }
                else
                {
                    result.Result = await _repository.GetAllDataByExpression(a => a.Communce!.District!.ProvinceId == filter.ProvinceId, pageNumber, pageSize, null, false, a => a.Communce!.District!.Province!, a => a.FacilityRating!.Rating!, a => a.FacilityRating!.FacilityType!);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
