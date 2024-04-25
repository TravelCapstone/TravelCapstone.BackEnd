using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class ReferenceTransportService : GenericBackendService, IReferenceTransportService
    {
        private readonly IRepository<ReferenceTransportPrice> _repository;
        private readonly IUnitOfWork _unitOfWork;
        
        public ReferenceTransportService(
            IServiceProvider serviceProvider,
            IUnitOfWork unitOfWork,
            IRepository<ReferenceTransportPrice> repository
            ) : base(serviceProvider)
        {
            _repository = repository;   
            _unitOfWork = unitOfWork;       
        }

        public async Task<AppActionResult> GetAllReferenceTransport(int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var data = await _repository.GetAllDataByExpression(null, pageIndex, pageSize,null,false, p => p.Departure!.Commune!.District!.Province!,p => p.Arrival!.Commune!.District!.Province!);
                result.Result = data;   
            } catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllReferenceTransportByProvinceId(int pageIndex, int pageSize, Guid firstProvince, Guid secondProvince)
        {
            var result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var provinceDb = await provinceRepository!.GetById(firstProvince);
                var secondProvinceDb = await provinceRepository!.GetById(secondProvince);
                if (provinceDb == null || secondProvinceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Tỉnh với {firstProvince} và {secondProvince} này không tồn tại");
                }
                if (!BuildAppActionResultIsError(result))
                {
                    result.Result = await _repository.GetAllDataByExpression(p => p!.Departure!.Commune!.District!.ProvinceId == firstProvince && p!.Arrival!.Commune!.District!.ProvinceId == secondProvince 
                    || p.Departure.Commune.District.ProvinceId == secondProvince && p.Arrival!.Commune!.District!.ProvinceId == firstProvince, pageIndex, pageSize, null, false, p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!); 
                }
            } catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }
    }
}
