using AutoMapper;
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


            //Add plan detail
            var planDetailRepository = Resolve<IRepository<PlanServiceCostDetail>>();
            foreach(var item in dto.Locations)
            {
                int numOfDay = (item.EndDate.Date - item.StartDate.Date).Days;
                if(item is EatingPlanLocation)
                {
                    var eating = item as EatingPlanLocation;
                    await planDetailRepository!.Insert(new PlanServiceCostDetail
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
                    await planDetailRepository!.Insert(new PlanServiceCostDetail
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
            }

            foreach (var item in dto.Vehicles)
            {
                int numOfDay = (item.EndDate - item.StartDate).Value.Days;
                if (item.VehicleType == Domain.Enum.VehicleType.PLANE || item.VehicleType == Domain.Enum.VehicleType.BOAT)
                {
                    await planDetailRepository!.Insert(new PlanServiceCostDetail
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
                }
                else
                {
                    await planDetailRepository!.Insert(new PlanServiceCostDetail
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
                }
            }


        } catch (Exception ex)
        {
            result = BuildAppActionResultError(result, ex.Message);
        }
        return result;
    }
}