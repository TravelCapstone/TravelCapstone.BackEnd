using AutoMapper;
using Hangfire.Logging.LogProviders;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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
                if (privateTourRequestDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với {dto.privateTourRequestId}");
                    return result;
                }

                var optionRepository = Resolve<IRepository<OptionQuotation>>();
                var optionDb = await optionRepository!.GetByExpression(o => o.PrivateTourRequestId == privateTourRequestDb.Id && o.OptionQuotationStatusId == Domain.Enum.OptionQuotationStatus.ACTIVE);
                if (optionDb == null)
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



                //Add tour guide
                var tourguideAssignmentRepository = Resolve<IRepository<TourguideAssignment>>();
                foreach (var item in dto.Tourguides)
                {
                    await tourguideAssignmentRepository!.Insert(new TourguideAssignment
                    {
                        Id = Guid.NewGuid(),
                        ProvinceId = item.ProvinceId,
                        AccountId = item.TourguideId,
                        TourId = tour.Id,
                        StartTime = item.StartDate,
                        EndTime = item.EndDate
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
                int numOfDay = 0;
                string activityType = "";
                foreach (var item in dto.Locations)
                {
                    numOfDay = (item.EndDate.HasValue) ? (item.EndDate.Value.Date - item.StartDate.Date).Days : 0;
                    numOfDay = (numOfDay > 0) ? numOfDay : 1;
                    //activityType = item.ServiceType == ServiceType.RESTING ? "Nghỉ ngơi" : item.ServiceType == ServiceType.FOODANDBEVARAGE ? "Ăn uống" : item.ServiceType == ServiceType.ENTERTAIMENT ? "Tham quan, vui chơi" : "Event";
                    planServiceCostDetails.Add(new PlanServiceCostDetail
                    {
                        Id = Guid.NewGuid(),
                        Name = $"{item.StartDate.Date} lúc {item.StartDate.Date.Hour} giờ",
                        Description = "",
                        Quantity = item.NumOfServiceUse * numOfDay,
                        StartDate = item.StartDate.Date,
                        EndDate = item.EndDate.Value.Date,
                        TourId = tour.Id,
                        SellPriceHistoryId = item.SellPriceHistoryId,
                    });
                    sellPriceIdCheckList.Add(item.SellPriceHistoryId);
                }

                foreach (var item in dto.Vehicles)
                {
                    numOfDay = (item.EndDate - item.StartDate).Value.Days;
                    if (item.VehicleType == Domain.Enum.VehicleType.PLANE || item.VehicleType == Domain.Enum.VehicleType.BOAT)
                    {
                        planServiceCostDetails.Add(new PlanServiceCostDetail
                        {
                            Id = Guid.NewGuid(),
                            Name = $"Dịch vụ di chuyển vào ngày {item.StartDate.Date} lúc {item.StartDate.Hour} giờ",
                            Description = "",
                            Quantity = privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate.HasValue ? item.EndDate.Value : item.StartDate.Date.AddHours(4),
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
                            Name = $"Dịch vụ di chuyển vào ngày {item.StartDate.Date} lúc {item.StartDate.Date.Hour} giờ",
                            Description = "",
                            Quantity = item.NumOfVehicle * numOfDay,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate.HasValue ? item.EndDate.Value : item.StartDate.Date.AddHours(4),
                            TourId = tour.Id,
                            SellPriceHistoryId = item.SellPriceHistoryId,
                        });
                        sellPriceIdCheckList.Add((Guid)item.SellPriceHistoryId);
                    }
                }

                var sellPriceDb = await sellPriceHistoyRepository!.GetAllDataByExpression(s => sellPriceIdCheckList.Contains(s.Id), 0, 0, null, false, s => s.FacilityService!, s => s.Menu.FacilityService, s => s.TransportServiceDetail.FacilityService);
                if (sellPriceDb.Items == null || sellPriceDb.Items.Count == 0)
                {
                    result = BuildAppActionResultError(result, $"Thông tin báo giá không hợp lệ");
                    return result;
                }
                List<Guid> facilityId = sellPriceDb.Items!.Where(s => s.FacilityServiceId != null).Select(s => s.FacilityService!.FacilityId).ToList();
                facilityId.AddRange(sellPriceDb.Items!.Where(s => s.MenuId != null).Select(s => s.Menu!.FacilityService!.FacilityId).ToList());
                facilityId.AddRange(sellPriceDb.Items!.Where(s => s.TransportServiceDetailId != null).Select(s => s.TransportServiceDetail!.FacilityService!.FacilityId).ToList());
                List<Guid> portIds = new List<Guid>();
                if (referencePriceIdCheckList.Count > 0)
                {
                    var referenceDb = await referencePriceRepository!.GetAllDataByExpression(r => referencePriceIdCheckList.Contains(r.Id), 0, 0, null, false, null);
                    if (referenceDb.Items != null && referenceDb.Items.Count > 0)
                    {
                        portIds.AddRange(referenceDb.Items!.Select(r => r.ArrivalId));
                        portIds.AddRange(referenceDb.Items!.Select(r => r.DepartureId));
                    }
                }

                //How to know which drive it or which reference price it is for route
                //Add DayPlan
                List<DayPlan> dayPlans = new List<DayPlan>();
                List<Route> routes = new List<Route>();
                List<VehicleRoute> vehicleRoutes = new List<VehicleRoute>();
                foreach (var item in dto.DetailPlanRoutes)
                {
                    if (item.Date > dto.EndDate || item.Date < dto.StartDate)
                    {
                        result = BuildAppActionResultError(result, $"Thời gian cho kế hoạch ngày {item.Date.Date.ToString()} không nằm trong thời gian của kế hoạch");
                        return result;
                    }

                    dayPlans.Add(new DayPlan
                    {
                        Id = Guid.NewGuid(),
                        Date = item.Date,
                        Description = item.Description,
                        TourId = tour.Id
                    });
                    Guid? parentRouteId = null;
                    Route parentRoute = null;
                    Guid? StartFacilityId = null;
                    Guid? EndFacilityId = null;
                    Guid? StartPortId = null;
                    Guid? EndPortId = null;

                    foreach (var detailRoute in item.DetailDayPlanRoutes)
                    {
                        StartFacilityId = null;
                        EndFacilityId = null;
                        StartPortId = null;
                        EndPortId = null;
                        if (detailRoute.StartId != null)
                        {
                            if (portIds.Contains((Guid)detailRoute.StartId))
                            {
                                StartPortId = detailRoute.StartId;
                            }
                            else if (facilityId.Contains((Guid)detailRoute.StartId))
                            {
                                StartFacilityId = detailRoute.StartId;
                            }
                            else
                            {
                                result = BuildAppActionResultError(result, $"Không tìm thấy cảng hoặc cơ sở với id {detailRoute.StartId} trong thông tin tour chi tiết");
                                return result;
                            }
                        }

                        if (detailRoute.EndId != null)
                        {
                            if (portIds.Contains((Guid)detailRoute.EndId))
                            {
                                StartPortId = detailRoute.EndId;
                            }
                            else if (facilityId.Contains((Guid)detailRoute.EndId))
                            {
                                StartFacilityId = detailRoute.EndId;
                            }
                            else
                            {
                                result = BuildAppActionResultError(result, $"Không tìm thấy cảng hoặc cơ sở với id {detailRoute.EndId} trong thông tin tour chi tiết");
                                return result;
                            }
                        }

                        routes.Add(new Route
                        {
                            Id = Guid.NewGuid(),
                            Note = detailRoute.Note,
                            StartTime = detailRoute.StartTime,
                            EndTime = detailRoute.EndTime,
                            StartPointId = StartFacilityId,
                            EndPointId = EndFacilityId,
                            PortStartPointId = StartPortId,
                            PortEndPointId = EndPortId,
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
                            var portDb = await portRepository!.GetAllDataByExpression(p => (StartPortId != null && StartPortId == p.Id) || (EndPortId != null && EndPortId == p.Id), 0, 0, null, false, null);
                            if (portDb.Items.Count > 1)
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
                            }
                            else if ((StartPortId is null) || (EndPortId is null))
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
                }
                await _repository.Insert(tour);
                await dayPlanRepository!.InsertRange(dayPlans);
                await routeRepository!.InsertRange(routes);
                await vehicleRouteRepository!.InsertRange(vehicleRoutes);
                int rowAffected = await _unitOfWork.SaveChangesAsync();
                if(rowAffected > 0)
                {
                    scope.Complete();
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