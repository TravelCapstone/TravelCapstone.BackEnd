using AutoMapper;
using Hangfire.Logging.LogProviders;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using static System.Formats.Asn1.AsnWriter;

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
        var staticFileRepository = Resolve<IRepository<StaticFile>>();
        try
        {
            var tourResponseList = new List<TourResponse>();

            if (string.IsNullOrEmpty(keyWord))
            {
                var tourListDb = await _repository.GetAllDataByExpression(
                    a => a.TourTypeId != TourType.ENTERPRISE,
                    pageNumber,
                    pageSize, null, false,
                    null
                );
                foreach (var tour in tourListDb.Items!)
                {
                    var tourResponse = new TourResponse();
                    var staticFileListDb = await staticFileRepository!.GetAllDataByExpression(p => p.TourId == tour.Id, 0, 0, null, false, null);
                    if (staticFileListDb.Items == null && staticFileListDb.Items!.Count() <= 0)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy ảnh cho tour với id {tour.Id}");
                    }
                    tourResponse.Tour = tour;
                    tourResponse.StaticFiles = staticFileListDb.Items!;
                    tourResponseList.Add(tourResponse);
                }
                result.Result = new PagedResult<TourResponse>
                {
                    Items = tourResponseList,
                    TotalPages = tourListDb.TotalPages,
                };
            }
            else
            {
                var tourListDb = await _repository.GetAllDataByExpression(
                    a => a.Name.ToLower().Trim().Contains(keyWord.ToLower().Trim()) && a.TourTypeId != TourType.ENTERPRISE,
                    pageNumber,
                    pageSize, null, false,
                    null
                );
                foreach (var tour in tourListDb.Items!)
                {
                    var tourResponse = new TourResponse();
                    var staticFileListDb = await staticFileRepository!.GetAllDataByExpression(p => p.TourId == tour.Id, 0, 0, null, false, null);
                    if (staticFileListDb.Items == null && staticFileListDb.Items!.Count() <= 0)
                    {
                        result = BuildAppActionResultError(result, $"Không tìm thấy ảnh cho tour với id {tour.Id}");
                    }
                    tourResponse.Tour = tour;
                    tourResponse.StaticFiles = staticFileListDb.Items!;
                    tourResponseList.Add(tourResponse);
                }
                result.Result = new PagedResult<TourResponse>
                {
                    Items = tourResponseList,
                    TotalPages = tourListDb.TotalPages,
                };
            }

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
                    await smsService!.SendMessage(code, dto.PhoneNumber);
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
                    Name = privateTourRequestDb.Description != null ? privateTourRequestDb.Description : "",//Check lại
                    Description = privateTourRequestDb.Description != null ? privateTourRequestDb.Description : "",
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

                var _assignmentRepository = Resolve<IRepository<MaterialAssignment>>();
                List<MaterialAssignment> materialAssignments = new List<MaterialAssignment>();
                foreach (var item in dto.Material.MaterialRequests)
                {
                    materialAssignments.Add(new MaterialAssignment()
                    {
                        Id = Guid.NewGuid(),
                        MaterialPriceHistoryId = item.MaterialSellPriceId,
                        Quantity = item.Quantity,
                        TourId = tour.Id
                    });
                }
                await _assignmentRepository!.InsertRange(materialAssignments);

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
                    if (item.Date > dto.EndDate.Date || item.Date < dto.StartDate.Date)
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
                            if ((await portRepository!.GetById(detailRoute.StartId)) != null)
                            {
                                StartPortId = detailRoute.StartId;
                            }
                            else if ((await facilityRepository!.GetById(detailRoute.StartId)) != null)
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
                            if ((await portRepository!.GetById(detailRoute.EndId)) != null)
                            {
                                StartPortId = detailRoute.EndId;
                            }
                            else if ((await facilityRepository!.GetById(detailRoute.EndId)) != null)
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
                privateTourRequestDb.PrivateTourStatusId = PrivateTourStatus.PLANCREATED;
                privateTourRequestDb.GeneratedTourId = tour.Id;
                tour.ContingencyFee = optionDb.ContingencyFee;
                tour.OperatingFee = optionDb.OperatingFee;
                tour.OrganizationCost = optionDb.OrganizationCost;
                tour.EscortFee = optionDb.EscortFee;
                tour.TotalPrice = !dto.Total.HasValue ? 0 : (double)dto.Total;
                tour.PricePerAdult = !dto.PricePerAdult.HasValue ? 0 : (double)dto.PricePerAdult;
                tour.PricePerChild = !dto.PricePerChildren.HasValue ? 0 : (double)dto.PricePerChildren;
                await privateTourRequestRepository.Update(privateTourRequestDb);
                await _repository.Insert(tour);
                await planDetailRepository!.InsertRange(planServiceCostDetails);
                await dayPlanRepository!.InsertRange(dayPlans);
                await routeRepository!.InsertRange(routes);
                await vehicleRouteRepository!.InsertRange(vehicleRoutes);
                int rowAffected = await _unitOfWork.SaveChangesAsync();
                if (rowAffected > 0)
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

    public async Task<AppActionResult> GetPlanByTour(Guid tourId)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var tourRepository = Resolve<IRepository<Tour>>();
            var dayPlanRepository = Resolve<IRepository<DayPlan>>();
            var planCostDetailRepository = Resolve<IRepository<PlanServiceCostDetail>>();
            var tourguideAssignmentRepository = Resolve<IRepository<TourguideAssignment>>();
            var materialAssignmentRepository = Resolve<IRepository<MaterialAssignment>>();
            var routeRepository = Resolve<IRepository<Route>>();
            TourPlanResponse tourPlanResponse = new TourPlanResponse();
            var vehicleRouteRepository = Resolve<IRepository<VehicleRoute>>();
            var listDayPlan = new List<DayPlansDto>();
            var tourDb = await tourRepository!.GetByExpression(p => p!.Id == tourId);
            if (tourDb == null)
            {
                result = BuildAppActionResultError(result, $"Tour với id {tourId} không tồn tại");
            }
            var dayPlanDb = await dayPlanRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, null);
            if (dayPlanDb.Items == null || dayPlanDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Kế hoạch cho tour với id {tourId} không tồn tại");
            }

            var dayPlanSorted = dayPlanDb.Items.OrderBy(p => p.Date);

            var planCostDetailsDb = await planCostDetailRepository!.GetAllDataByExpression(
                p => p.TourId == tourId, 0, 0, null, false,
                p => p.ReferenceTransportPrice!.Arrival!.Commune!.District!.Province!,
                p => p.ReferenceTransportPrice!.Departure!.Commune!.District!.Province!,
                p => p.ReferenceTransportPrice!.ReferencePriceRating!,
                p => p.SellPriceHistory!.FacilityService!.Facility!.FacilityRating!,
                p => p.SellPriceHistory!.Menu!.FacilityService!.Facility!.FacilityRating!,
                p => p.SellPriceHistory!.TransportServiceDetail!.FacilityService!.Facility!.FacilityRating!
            );
            if (planCostDetailsDb.Items == null || planCostDetailsDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Giá của kế hoạch chi tiết cho tour với id {tourId} không tồn tại");
            }
            var tourguideAssignmentDb = await tourguideAssignmentRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, p => p.Account!, p => p.Province!);
            if (tourguideAssignmentDb.Items == null || tourguideAssignmentDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Hướng dẫn viên cho tour với ${tourId} không tìm thấy");
            }

            var materialDb = await materialAssignmentRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, p => p.Tour!, p => p.MaterialPriceHistory!.Material!);
            if (materialDb.Items == null || materialDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Các vật phẩm đi cùng tour với id {tourId} không tìm thấy");
            }

            foreach (var item in dayPlanSorted)
            {
                var routeDb = await routeRepository!.GetAllDataByExpression(
                    p => p.DayPlanId == item.Id, 0, 0, null, false,
                    p => p.StartPoint!.FacilityRating!.Rating!, p => p.EndPoint!.FacilityRating!.Rating!,
                    p => p.StartPoint!.Communce!.District!.Province!,
                    p => p.EndPoint!.Communce!.District!.Province!,
                    p => p.PortStartPoint!.Commune!.District!.Province!, p => p.PortEndPoint!.Commune!.District!.Province!, p => p.ParentRoute!
                );
                if (routeDb.Items == null || routeDb.Items!.Count <= 0)
                {
                    result = BuildAppActionResultError(result, $"Lộ trình cho tour với id {tourId} không tìm thấy");
                }

                var routeIds = routeDb.Items.Select(r => r.Id).ToList();

                var vehicleRouteDb = await vehicleRouteRepository!.GetAllDataByExpression(
                    p => p.Route!.DayPlanId == item.Id, 0, 0, p => p.Route!.StartTime, true,
                    p => p.Route!.StartPoint!.Communce!.District!.Province!, p => p.Vehicle!.VehicleType!,
                    p => p.Driver!,
                    p => p.Route!.EndPoint!.Communce!.District!.Province!,
                    p => p.Route!.PortStartPoint!.Commune!.District!.Province!,
                    p => p.Route!.PortEndPoint!.Commune!.District!.Province!,
                    p => p.Route!.ParentRoute!
                );


                DayPlansDto dayPlansDto = new DayPlansDto();
                dayPlansDto.DayPlan = item;
                dayPlansDto.VehicleRoutes = vehicleRouteDb.Items!;

                listDayPlan.Add(dayPlansDto);

                tourPlanResponse.Tour = tourDb;
                tourPlanResponse.PlanServiceCostDetails = planCostDetailsDb.Items;
                tourPlanResponse.TourguideAssignments = tourguideAssignmentDb.Items;
                tourPlanResponse.MaterialAssignments = materialDb.Items;
                tourPlanResponse.DayPlans = listDayPlan;
            }

            result.Result = tourPlanResponse;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
        }
        return result;
    }

    public async Task<AppActionResult> UpdateOptionQuotationStatus(Guid optionId)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var optionQuotationRepository = Resolve<IRepository<OptionQuotation>>();
            var optionQuotationDb = await optionQuotationRepository!.GetByExpression(p => p.Id == optionId);
            if (optionQuotationDb == null)
            {
                result = BuildAppActionResultError(result, $"Option với Id {optionId} không tồn tại");
            }
            else
            {
                if (optionQuotationDb.OptionQuotationStatusId == OptionQuotationStatus.NEW)
                {
                    optionQuotationDb.OptionQuotationStatusId = OptionQuotationStatus.ACTIVE;
                }
                else
                {
                    optionQuotationDb.OptionQuotationStatusId = OptionQuotationStatus.IN_ACTIVE;
                }
                await _unitOfWork.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
        }
        return result;
    }

    public async Task<AppActionResult> CalculatePlanCost(CreatePlanDetailDto dto)
    {
        var result = new AppActionResult();

        try
        {
            var data = new PlanCostResponse();

            // Resolve repositories
            var privateTourRequestRepository = Resolve<IRepository<PrivateTourRequest>>();
            var tourGuideSalaryHistoryRepository = Resolve<IRepository<TourGuideSalaryHistory>>();
            var optionRepository = Resolve<IRepository<OptionQuotation>>();
            var assurancePriceRepository = Resolve<IRepository<AssurancePriceHistory>>();
            var optionEventRepository = Resolve<IRepository<OptionEvent>>();
            var sellPriceHistoyRepository = Resolve<IRepository<SellPriceHistory>>();
            var referencePriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
            var driverSalaryHistoryRepository = Resolve<IRepository<DriverSalaryHistory>>();
            var eventDetailPriceHistoryRepository = Resolve<IRepository<EventDetailPriceHistory>>();
            var materialService = Resolve<IMaterialService>();

            // Get private tour request
            var privateTourRequestDb = await privateTourRequestRepository.GetById(dto.privateTourRequestId);
            if (privateTourRequestDb == null)
            {
                return BuildAppActionResultError(result, $"Không tìm thấy yêu cầu tạo tour với {dto.privateTourRequestId}");
            }

            // Get active option quotation
            var optionDb = await optionRepository.GetByExpression(o => o.PrivateTourRequestId == dto.privateTourRequestId && o.OptionQuotationStatusId == Domain.Enum.OptionQuotationStatus.ACTIVE);
            if (optionDb == null)
            {
                return BuildAppActionResultError(result, "Không tìm thấy lựa chọn đã được duyệt từ yêu cầu tạo tour");
            }

            // Populate data from option quotation
            data.EscortFee = optionDb.EscortFee;
            data.OperatingFee = optionDb.OperatingFee;
            data.OrganizationCost = optionDb.OrganizationCost;
            data.ContingencyFee = optionDb.ContingencyFee;

            // Get assurance price history
            var assurancePriceDb = await assurancePriceRepository.GetById(optionDb.AssurancePriceHistoryId);
            if (assurancePriceDb != null)
            {
                data.AssuranceCost = assurancePriceDb.Price * (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren);
            }

            // Get events for the option
            var events = await optionEventRepository.GetAllDataByExpression(o => o.OptionId == optionDb.Id, 0, 0, null, false, null);
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var item in events.Items)
                {
                    if (!item.CustomEvent.Equals("string") && !string.IsNullOrEmpty(item.CustomEvent))
                    {
                        var latestCost = JsonConvert.DeserializeObject<CustomEventStringResponse>(item.CustomEvent);
                        if (latestCost != null)
                        {
                            data.EventCost += latestCost.Total;
                        }
                    } else
                    {
                        var eventSellPriceDb = await eventDetailPriceHistoryRepository.GetAllDataByExpression(e => e.EventDetail.EventId == item.EventId, 0, 0, null, false, e => e.EventDetail);
                        if(eventSellPriceDb.Items != null && eventSellPriceDb.Items.Count() > 0)
                        {
                            var latestPrice = eventSellPriceDb.Items.GroupBy(e => e.EventDetailId).Select(e => e.OrderByDescending(s => s.Date).FirstOrDefault()).ToList();
                            latestPrice.ForEach(e => data.EventCost += e.Price * ((e.EventDetail.PerPerson) ? (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren) : 1));
                        }
                    }
                }
            }

            // Calculate tour guide costs
            foreach (var item in dto.Tourguides)
            {
                var tourguideDb = await tourGuideSalaryHistoryRepository.GetByExpression(t => t.AccountId == item.TourguideId, null);
                if (tourguideDb != null)
                {
                    int numOfDays = (int)Math.Ceiling((item.EndDate - item.StartDate).TotalDays);
                    numOfDays = numOfDays == 0 ? 1 : numOfDays;
                    data.TourguideCost += tourguideDb.Salary * numOfDays;
                }
            }

            // Calculate material cost
            var materialCost = await materialService!.GetMaterialCost(dto.Material);
            data.MaterialCost = (double)materialCost.Result!;

            // Calculate facility costs
            foreach (var item in dto.Locations)
            {
                int numOfDay = (item.EndDate.HasValue) ? (item.EndDate.Value.Date - item.StartDate.Date).Days : 0;
                numOfDay = (numOfDay > 0) ? numOfDay : 1;
                var sellpriceDb = await sellPriceHistoyRepository!.GetById(item.SellPriceHistoryId);
                data.FacilityCost += sellpriceDb.Price * item.NumOfServiceUse * numOfDay;
            }

            // Calculate vehicle costs
            foreach (var item in dto.Vehicles)
            {
                int numOfDay = (item.EndDate - item.StartDate)!.Value.Days;
                numOfDay = numOfDay == 0 ? 1 : numOfDay;
                if (item.VehicleType == Domain.Enum.VehicleType.PLANE || item.VehicleType == Domain.Enum.VehicleType.BOAT)
                {
                    var referencePriceDb = await referencePriceRepository!.GetById(item.ReferencePriceId!);
                    data.VehicleCost += referencePriceDb.AdultPrice * privateTourRequestDb.NumOfAdult + referencePriceDb.ChildPrice * privateTourRequestDb.NumOfChildren;
                }
                else
                {
                    var sellpriceDb = await sellPriceHistoyRepository!.GetByExpression(s => s.Id == item.SellPriceHistoryId, s => s.TransportServiceDetail.FacilityService);
                    int numOfVehicle = (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren) / sellpriceDb.TransportServiceDetail.FacilityService.ServingQuantity
                        + (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren) % sellpriceDb.TransportServiceDetail.FacilityService.ServingQuantity == 0 ? 0 : 1;
                    data.VehicleCost += sellpriceDb.Price * numOfVehicle * numOfDay;
                    var driverSalaryDb = await driverSalaryHistoryRepository.GetAllDataByExpression(d => d.DriverId == item.DriverId, 0, 0, d => d.Date, false, null);
                    if (driverSalaryDb.Items.Count > 0)
                    {
                        data.DriverCost += driverSalaryDb.Items.First().Salary * numOfDay;
                    }
                }
            }
            data.Total = data.VehicleCost + data.DriverCost + data.FacilityCost +
                         data.TourguideCost + data.AssuranceCost + data.MaterialCost +
                         data.EventCost + data.EscortFee + data.OperatingFee +
                         data.OrganizationCost + data.ContingencyFee;
            data.PricePerAdult = data.Total / (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren);
            data.PricePerChildren = data.Total / (privateTourRequestDb.NumOfAdult + privateTourRequestDb.NumOfChildren);
            // Set result data
            result.Result = data;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }

        return result;
    }

    public async Task<AppActionResult> GetGroupPlanByTour(Guid tourId)
    {
        AppActionResult result = new AppActionResult();
        try
        {
            var tourRepository = Resolve<IRepository<Tour>>();
            var dayPlanRepository = Resolve<IRepository<DayPlan>>();
            var planCostDetailRepository = Resolve<IRepository<PlanServiceCostDetail>>();
            var tourguideAssignmentRepository = Resolve<IRepository<TourguideAssignment>>();
            var materialAssignmentRepository = Resolve<IRepository<MaterialAssignment>>();
            var routeRepository = Resolve<IRepository<Route>>();
            GroupedTourPlanResponse tourPlanResponse = new GroupedTourPlanResponse();
            var vehicleRouteRepository = Resolve<IRepository<VehicleRoute>>();
            var listDayPlan = new List<DayPlansDto>();
            var tourDb = await tourRepository!.GetByExpression(p => p!.Id == tourId);
            if (tourDb == null)
            {
                result = BuildAppActionResultError(result, $"Tour với id {tourId} không tồn tại");
                return result;
            }
            var dayPlanDb = await dayPlanRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, null);
            if (dayPlanDb.Items == null || dayPlanDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Kế hoạch cho tour với id {tourId} không tồn tại");
                return result;
            }

            var dayPlanSorted = dayPlanDb.Items.OrderBy(p => p.Date);

            var planCostDetailsDb = await planCostDetailRepository!.GetAllDataByExpression(
                p => p.TourId == tourId, 0, 0, null, false,
                p => p.ReferenceTransportPrice!.Arrival!.Commune!.District!.Province!,
                p => p.ReferenceTransportPrice!.Departure!.Commune!.District!.Province!,
                p => p.ReferenceTransportPrice!.ReferencePriceRating!,
                p => p.SellPriceHistory!.FacilityService!.Facility!.FacilityRating!,
                p => p.SellPriceHistory!.Menu!.FacilityService!.Facility!.FacilityRating!,
                p => p.SellPriceHistory!.FacilityService!.Facility!.Communce!.District!.Province!,
                p => p.SellPriceHistory!.Menu!.FacilityService!.Facility!.Communce!.District!.Province!,
                p => p.SellPriceHistory!.TransportServiceDetail!.FacilityService!.Facility!.FacilityRating!
            );
            if (planCostDetailsDb.Items == null || planCostDetailsDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Giá của kế hoạch chi tiết cho tour với id {tourId} không tồn tại");
                return result;
            }
            var groupedPlanCostDetail = planCostDetailsDb.Items.Where(p => p.SellPriceHistory.FacilityServiceId != null).GroupBy(s => s.SellPriceHistory.FacilityService.ServiceTypeId).ToList();
            var menuGroupedPlanCostDetail = planCostDetailsDb.Items.Where(p => p.SellPriceHistory.MenuId != null).GroupBy(s => s.SellPriceHistory.Menu.FacilityService.ServiceTypeId).ToList();
            List<GroupedPlanServiceDetail> groupedService = new List<GroupedPlanServiceDetail>();
            foreach(var group in groupedPlanCostDetail)
            {
                var grouped = new GroupedPlanServiceDetail();
                grouped.serviceType = group.Key;
                var provinceGrouped = group.GroupBy(p => p.SellPriceHistory.FacilityService.Facility.Communce.District.Province).ToList();
                provinceGrouped.ForEach(p => grouped.ProvincePlanserviceDetails.Add(
                    new ProvincePlanserviceDetail
                    {
                        Province = p.Key,
                        PlanServiceCostDetails = p.ToList()
                    }
                    ));
                //grouped.PlanServiceCostDetails.AddRange(group.ToList());
                groupedService.Add(grouped);
            }

            foreach (var group in menuGroupedPlanCostDetail)
            {
                var grouped = new GroupedPlanServiceDetail();
                grouped.serviceType = group.Key;
                var provinceGrouped = group.GroupBy(p => p.SellPriceHistory.Menu.FacilityService.Facility.Communce.District.Province).ToList();
                provinceGrouped.ForEach(p => grouped.ProvincePlanserviceDetails.Add(
                    new ProvincePlanserviceDetail
                    {
                        Province = p.Key,
                        PlanServiceCostDetails = p.ToList()
                    }
                    ));
                //grouped.PlanServiceCostDetails.AddRange(group.ToList());
                groupedService.Add(grouped);
            }

            var tourguideAssignmentDb = await tourguideAssignmentRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, p => p.Account!, p => p.Province!);
            if (tourguideAssignmentDb.Items == null || tourguideAssignmentDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Hướng dẫn viên cho tour với ${tourId} không tìm thấy");
                return result;
            }

            var materialDb = await materialAssignmentRepository!.GetAllDataByExpression(p => p.TourId == tourId, 0, 0, null, false, p => p.Tour!, p => p.MaterialPriceHistory!.Material!);
            if (materialDb.Items == null || materialDb.Items!.Count <= 0)
            {
                result = BuildAppActionResultError(result, $"Các vật phẩm đi cùng tour với id {tourId} không tìm thấy");
                return result;
            }

            foreach (var item in dayPlanSorted)
            {
                var routeDb = await routeRepository!.GetAllDataByExpression(
                    p => p.DayPlanId == item.Id, 0, 0, null, false,
                    p => p.StartPoint!.FacilityRating!.Rating!, p => p.EndPoint!.FacilityRating!.Rating!,
                    p => p.StartPoint!.Communce!.District!.Province!,
                    p => p.EndPoint!.Communce!.District!.Province!,
                    p => p.PortStartPoint!.Commune!.District!.Province!, p => p.PortEndPoint!.Commune!.District!.Province!, p => p.ParentRoute!
                );
                if (routeDb.Items == null || routeDb.Items!.Count <= 0)
                {
                    result = BuildAppActionResultError(result, $"Lộ trình cho tour với id {tourId} không tìm thấy");
                    return result;
                }

                var routeIds = routeDb.Items.Select(r => r.Id).ToList();

                var vehicleRouteDb = await vehicleRouteRepository!.GetAllDataByExpression(
                    p => p.Route!.DayPlanId == item.Id, 0, 0, p => p.Route!.StartTime, true,
                    p => p.Route!.StartPoint!.Communce!.District!.Province!, p => p.Vehicle!.VehicleType!,
                    p => p.Driver!,
                    p => p.Route!.EndPoint!.Communce!.District!.Province!,
                    p => p.Route!.PortStartPoint!.Commune!.District!.Province!,
                    p => p.Route!.PortEndPoint!.Commune!.District!.Province!,
                    p => p.Route!.ParentRoute!
                );


                DayPlansDto dayPlansDto = new DayPlansDto();
                dayPlansDto.DayPlan = item;
                dayPlansDto.VehicleRoutes = vehicleRouteDb.Items!;

                listDayPlan.Add(dayPlansDto);

                tourPlanResponse.Tour = tourDb;
                tourPlanResponse.PlanServiceCostDetails = groupedService;
                tourPlanResponse.TourguideAssignments = tourguideAssignmentDb.Items;
                tourPlanResponse.MaterialAssignments = materialDb.Items;
                tourPlanResponse.DayPlans = listDayPlan;
            }

            result.Result = tourPlanResponse;
        }
        catch (Exception ex)
        {
            result = BuildAppActionResultError(result, $"Co loi xay ra {ex.Message}");
        }
        return result;
    }
}