using AutoMapper;
using Hangfire.Logging.LogProviders;
using System.ComponentModel.DataAnnotations.Schema;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class TourService : GenericBackendService, ITourService
{
    private readonly IRepository<Tour> _repository;
    private IMapper _mapper;
    private IUnitOfWork _unitOfWork;
    public TourService(
        IServiceProvider serviceProvider,
        IRepository<Tour> repository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    ) : base(serviceProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppActionResult> GetById(Guid id)
    {
        var result = new AppActionResult();
        try
        {
            var dayPlanRepository = Resolve<IRepository<DayPlan>>();
            var routeRepository = Resolve<IRepository<Route>>();
            var materialRepository = Resolve<IRepository<MaterialAssignment>>();
            var detail = new TourDetail();
            detail.DayPlanDtos = new List<DayPlanDto>();
            var tour = await _repository.GetById(id);
            detail.Tour = tour;
                var materials =
                    await materialRepository!.GetAllDataByExpression(a => a.TourId == id, 0, 0, null, false, null);
            detail.Materials = materials.Items!;
            var listPlan = await dayPlanRepository!.GetAllDataByExpression(
                a => a.TourId == id,
                0,
                0, null, false,
                null);
            foreach (var item in listPlan.Items!)
            {
                var route = await routeRepository!.GetAllDataByExpression(
                    a => a.DayPlanId == item.Id,
                    0,
                    0,
                    null,
                    false,
                    a => a.StartPoint!, a => a.EndPoint!);
                detail.DayPlanDtos.Add(new DayPlanDto
                {
                    DayPlan = item,
                    Routes = route.Items!,
                });
            }

            result.Result = detail;
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Lỗi đã xảy ra: {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetAll(string? keyWord, int pageNumber, int pageSize)
    {
        var result = new AppActionResult();
        try
        {
            if (string.IsNullOrEmpty(keyWord))
                result.Result = await _repository.GetAllDataByExpression(
                    null,
                    pageNumber,
                    pageSize, null, false,
                    null
                );
            else
                result.Result = await _repository.GetAllDataByExpression(
                    a => a.Name.ToLower().Trim().Contains(keyWord.ToLower().Trim()),
                    pageNumber,
                    pageSize, null, false,
                    null
                );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Lỗi đã xảy ra: {e.Message}");
        }

        return result;
    }
    public async Task<AppActionResult> RegisterTour(TourRegistrationDto dto)
    {
        AppActionResult result = new AppActionResult();
        var smsService = Resolve<ISmsService>();
        var emailService = Resolve<IEmailService>();
        var customerRepository = Resolve<IRepository<Customer>>();
        try
        {
            var travelCompanionDb = await customerRepository!.GetByExpression
                (a => a!.Email == dto.Email || a.PhoneNumber == dto.PhoneNumber);
            if (travelCompanionDb != null && travelCompanionDb.IsVerfiedPhoneNumber && travelCompanionDb.IsVerifiedEmail)
            {
                result = BuildAppActionResultError(result, $"Đã tồn tại customer trong hệ thống!");
            }
            if (!BuildAppActionResultIsError(result))
            {
                //Validate phonumber
                if (dto.PhoneNumber != null && dto.Email == null)
                {
                    var id = Guid.NewGuid();
                    var code = SD.GenerateOTP();
                    var travelCompanion = _mapper.Map<Customer>(dto);
                    travelCompanion.Id = id;
                    await customerRepository.Insert(travelCompanion);
                    await smsService!.SendMessage(code);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    var code = SD.GenerateOTP();
                    emailService!.SendEmail(dto.Email!, "Mã xác thực OTP", code);
                }

                result.Messages.Add("Đã gửi OTP xác nhận");
            }

        }
        catch (Exception ex)
        {

            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {ex.Message}");
        }
        return result;
    }

    public async Task<AppActionResult> CreateTour(CreatePlanDetailDto dto)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            // create tour
            // add tour guide
            // add material
            // add plan detail
            // add day plan
            // add Route + Vehicle Route
            var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
            var privateTourRequestDb = await privateTourRequestRepository!.GetById(dto.privateTourRequestId);
            if(privateTourRequestDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với {dto.privateTourRequestId}");
                return result;
            }

            var optionRepository = Resolve<IRepository<OptionQuotation>>();
            var optionDb = await optionRepository!.GetByExpression(o => o.PrivateTourRequestId == privateTourRequestDb.Id && o.OptionQuotationStatusId == Domain.Enum.OptionQuotationStatus.ACTIVE);
            if(optionDb == null)
            {
                result = BuildAppActionResultError(result, $"Không tìm thấy lựa chọn đã được duyệt từ yêu cầu tạo tour");
                return result;
            }

            Tour tour = new Tour()
            {
                Id = Guid.NewGuid(),
                Name = privateTourRequestDb.Description!,//Check lại
                Description = privateTourRequestDb.Description!,
                BasedOnTourId = privateTourRequestDb.TourId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TourStatusId = Domain.Enum.TourStatus.NEW

            };

            await _repository.Insert(tour);
            await _unitOfWork.SaveChangesAsync();

            //Add tour guide
            var tourguideAssignmentRepository = Resolve<IRepository<TourguideAssignment>>();
            foreach(var item in dto.Tourguides)
            {
                await tourguideAssignmentRepository!.Insert(new TourguideAssignment
                {
                    Id = Guid.NewGuid(),
                    ProvinceId = item.ProvinceId,
                    AccountId = item.TourguideId,
                    TourId = tour.Id
                });
            }

            //Add material
            dto.Material.TourId = tour.Id;  
            var materialService = Resolve<IMaterialService>();
            await materialService!.AddMaterialtoTour(dto.Material);
            HashSet<Guid> sellPriceIdCheckList = new HashSet<Guid>();
            HashSet<Guid> referencePriceIdCheckList = new HashSet<Guid>();
            var portRepository = Resolve<IRepository<Port>>();
            var facilityRepository = Resolve<IRepository<Facility>>();
            var referencePriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
            var sellPriceHistoyRepository = Resolve<IRepository<SellPriceHistory>>();

            //Add plan detail

            var planDetailRepository = Resolve<IRepository<PlanServiceCostDetail>>();
            var dayPlanRepository = Resolve<IRepository<DayPlan>>();
            var routeRepository = Resolve<IRepository<Route>>();
            var vehicleRouteRepository = Resolve<IRepository<VehicleRoute>>();
            List<PlanServiceCostDetail> planServiceCostDetails = new List<PlanServiceCostDetail>();
            foreach(var item in dto.Locations)
            {
                int numOfDay = (item.EndDate.Date - item.StartDate.Date).Days;
                if(item is EatingPlanLocation)
                {
                    var eating = item as EatingPlanLocation;
                    planServiceCostDetails.Add(new PlanServiceCostDetail
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Dịch vụ ăn uống vào ngày {eating.StartDate.Date} lúc {eating.StartDate.Hour} giờ",
                        Description = "",
                        Quantity = eating.NumOfServiceUse * eating.MealPerDay * numOfDay,
                        StartDate = eating.StartDate,
                        EndDate = eating.EndDate,
                        TourId = tour.Id,
                        SellPriceHistoryId = eating.SellPriceHistoryId,
                    });
                } else
                {
                    planServiceCostDetails.Add(new PlanServiceCostDetail
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Dịch vụ ăn uống vào ngày {item.StartDate.Date} lúc {item.StartDate.Hour} giờ",
                        Description = "",
                        Quantity = item.NumOfServiceUse * numOfDay,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        TourId = tour.Id,
                        SellPriceHistoryId = item.SellPriceHistoryId,
                    });
                }
                sellPriceIdCheckList.Add(item.SellPriceHistoryId);
            }

            foreach (var item in dto.Vehicles)
            {
                int numOfDay = (item.EndDate - item.StartDate).Value.Days;
                if (item.VehicleType == Domain.Enum.VehicleType.PLANE || item.VehicleType == Domain.Enum.VehicleType.BOAT)
                {
                    planServiceCostDetails.Add(new PlanServiceCostDetail
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Dịch vụ di chuyển vào ngày {item.StartDate.Value.Date} lúc {item.StartDate.Value.Hour} giờ",
                        Description = "",
                        Quantity = privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren,
                        StartDate = (DateTime)item.StartDate,
                        EndDate = (DateTime)item.EndDate,
                        TourId = tour.Id,
                        ReferenceTransportPriceId = item.ReferencePriceId,
                    });
                    referencePriceIdCheckList.Add((Guid)item.ReferencePriceId);
                }
                else
                {
                    planServiceCostDetails.Add(new PlanServiceCostDetail
                    {
                        Id = Guid.NewGuid(),
                        Name = $"Dịch vụ di chuyển vào ngày {item.StartDate.Value.Date} lúc {item.StartDate.Value.Hour} giờ",
                        Description = "",
                        Quantity = item.NumOfVehicle * numOfDay,
                        StartDate = (DateTime)item.StartDate,
                        EndDate = (DateTime)item.EndDate,
                        TourId = tour.Id,
                        SellPriceHistoryId = item.SellPriceHistoryId,
                    });
                    sellPriceIdCheckList.Add((Guid)item.SellPriceHistoryId);
                }
            }

            var sellPriceDb = await sellPriceHistoyRepository!.GetAllDataByExpression(s => sellPriceIdCheckList.Contains(s.Id), 0, 0, null, false, s => s.FacilityService!);
            var referenceDb = await referencePriceRepository!.GetAllDataByExpression(r => referencePriceIdCheckList.Contains(r.Id), 0, 0, null, false, null);
            var facilityId = sellPriceDb.Items!.Select(s => s.FacilityService!.FacilityId).ToList();
            List<Guid> portIds = new List<Guid>();
            portIds.AddRange(referenceDb.Items!.Select(r => r.ArrivalId));
            portIds.AddRange(referenceDb.Items!.Select(r => r.DepartureId));

            //How to know which drive it or which reference price it is for route
            //Add DayPlan
            List<DayPlan> dayPlans = new List<DayPlan>();
            List<Route> routes = new List<Route>();
            List<VehicleRoute> vehicleRoutes = new List<VehicleRoute>();
            foreach (var item in dto.DetailPlanRoutes)
            {
                if(item.Date > dto.EndDate || item.Date < dto.StartDate)
                {
                    result = BuildAppActionResultError(result, $"Thời gian cho kế hoạch ngày {item.Date.Date.ToString()} không nằm trong thời gian của kế hoạch");
                    return result;
                }

                dayPlans.Add(new DayPlan
                {
                    Id = Guid.NewGuid(),
                    Date = item.Date,
                    Description = item.Description,
                    Name = item.Name,
                    TourId = tour.Id
                });
                Guid? parentRouteId = null;
                Route parentRoute = null;
                foreach(var detailRoute in item.DetailDayPlanRoutes) 
                {
                    if(detailRoute.StartPortId != null && !portIds.Contains((Guid)detailRoute.StartPortId))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy cảng với id {detailRoute.StartPortId} trong thông tin tour chi tiết");
                        return result;
                    }

                    if (detailRoute.EndPortId != null && !portIds.Contains((Guid)detailRoute.EndPortId))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy cảng với id {detailRoute.EndPortId} trong thông tin tour chi tiết");
                        return result;
                    }

                    if (detailRoute.StartFacilityId != null && !facilityId.Contains((Guid)detailRoute.StartFacilityId))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy cơ sở với id {detailRoute.StartFacilityId} trong thông tin tour chi tiết");
                        return result;
                    }

                    if (detailRoute.EndFacilityId != null && !facilityId.Contains((Guid)detailRoute.EndFacilityId))
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy cơ sở với id {detailRoute.EndFacilityId} trong thông tin tour chi tiết");
                        return result;
                    }

                    routes.Add(new Route
                    {
                        Id = Guid.NewGuid(),
                        Name = detailRoute.Name,
                        Note = detailRoute.Note,
                        StartTime = dto.StartDate,
                        EndTime = dto.EndDate,
                        StartPointId = detailRoute.StartFacilityId,
                        EndPointId = detailRoute.EndFacilityId,
                        PortStartPointId = detailRoute.StartPortId,
                        PortEndPointId = detailRoute.EndPortId,
                        DayPlanId = dayPlans[dayPlans.Count - 1].Id,
                        ParentRouteId = parentRouteId,

                    });
                    parentRoute = routes[routes.Count - 1];
                    parentRouteId = parentRoute.Id;
                    var vehicle = dto.Vehicles.Where(v => v.StartDate <= parentRoute.StartTime && parentRoute.EndTime <= v.EndDate)
                                              .OrderByDescending(v => (parentRoute.StartTime - v.StartDate) + (v.EndDate - parentRoute.EndTime))
                                              .FirstOrDefault();
                    if (vehicle != null)
                    {
                        var portDb = await portRepository!.GetAllDataByExpression(p => (detailRoute.StartPortId != null && detailRoute.StartPortId == p.Id) && (detailRoute.EndPortId != null && detailRoute.StartPortId == p.Id), 0, 0, null, false, null);
                        if (portDb.Items.Count > 0)
                        {
                            vehicleRoutes.Add(new VehicleRoute
                            {
                                Id = Guid.NewGuid(),
                                VehicleType = vehicle.VehicleType,
                                RouteId = (Guid)parentRouteId,
                                VehicleId = vehicle.VehicleId,
                                DriverId = vehicle.DriverId,
                                ReferenceBrandName = portDb.Items[0].Name
                            });
                        } else if ((detailRoute.StartPortId is null) ^ (detailRoute.EndPortId is null))
                        {
                            vehicleRoutes.Add(new VehicleRoute
                            {
                                Id = Guid.NewGuid(),
                                VehicleType = vehicle.VehicleType,
                                RouteId = (Guid)parentRouteId,
                                VehicleId = vehicle.VehicleId,
                                DriverId = vehicle.DriverId
                            });
                        }
                    }
                }
                await dayPlanRepository!.InsertRange(dayPlans);
                await routeRepository!.InsertRange(routes);
                await vehicleRouteRepository!.InsertRange(vehicleRoutes);
                await _unitOfWork.SaveChangesAsync();

            }

        } catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }
        return result;
    }
}