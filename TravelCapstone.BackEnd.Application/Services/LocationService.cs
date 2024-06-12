using NPOI.Util.ArrayExtensions;
using System.Text;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services;

public class LocationService : GenericBackendService, ILocationService
{
    private readonly IRepository<Commune> _communeRepository;
    private readonly IRepository<District> _districtRepository;
    private readonly IRepository<Province> _provinceRepository;
    private IUnitOfWork _unitOfWork;

    public LocationService(IServiceProvider serviceProvider, IRepository<Province> provinceRepository,
        IRepository<District> districtRepository, IRepository<Commune> communeRepository,
        IUnitOfWork unitOfWork) : base(serviceProvider)
    {
        _provinceRepository = provinceRepository;
        _districtRepository = districtRepository;
        _communeRepository = communeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppActionResult> GetAllProvince()
    {
        var result = new AppActionResult();
        try
        {
            result.Result = await _provinceRepository.GetAllDataByExpression(
                null,
                0,
                0,
                null,
                false,
                null
            );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetAllDistrictByProvinceId(Guid provinceId)
    {
        var result = new AppActionResult();
        try
        {
            result.Result = await _districtRepository.GetAllDataByExpression(
                a => a.ProvinceId == provinceId,
                0,
                0, null, false,
                null
            );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetAllCommuneByDistrictId(Guid districtId)
    {
        var result = new AppActionResult();
        try
        {
            result.Result = await _communeRepository.GetAllDataByExpression(
                a => a.DistrictId == districtId,
                0,
                0,
                 null, false,
                null
            );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetAllCommuneByDistrictNameAndCommuneName(string districtName,
        string communeName)
    {
        var result = new AppActionResult();
        try
        {
            result.Result = await _communeRepository.GetAllDataByExpression(
                a => a.Name.ToLower().Contains(communeName.ToLower()) &&
                     a.District!.Name.ToLower().Contains(districtName.ToLower()),
                0,
                0,
                 null, false,
                a => a.District!.Province!
            );
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }

        return result;
    }

    public async Task<AppActionResult> GetProvinceByName(string provinceName)
    {
        AppActionResult result = new();
        try
        {
            if(!provinceName.Contains("Bà"))
                result.Result = await _provinceRepository.GetByExpression(a => a!.Name.ToLower().Contains(provinceName.ToLower()));
            else
            {
                result.Result = await _provinceRepository.GetByExpression(a => a!.Name.ToLower().Contains("Bà"));
            }
        }
        catch (Exception e)
        {
            result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
        }
        return result;
    }
}