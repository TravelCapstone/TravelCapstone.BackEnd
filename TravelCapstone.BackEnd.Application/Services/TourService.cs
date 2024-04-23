using AutoMapper;
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
            var materialRepository = Resolve<IRepository<Material>>();
            var detail = new TourDetail();
            detail.DayPlanDtos = new List<DayPlanDto>();
            var tour = await _repository.GetById(id);
            detail.Tour = tour;

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
                var materials =
                    await materialRepository!.GetAllDataByExpression(a => a.DayPlanId == item.Id, 0, 0, null, false, null);
                detail.DayPlanDtos.Add(new DayPlanDto
                {
                    DayPlan = item,
                    Routes = route.Items!,
                    Materials = materials.Items!
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

}