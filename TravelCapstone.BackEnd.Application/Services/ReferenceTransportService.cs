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
                var data = await _repository.GetAllDataByExpression(null, pageIndex, pageSize, null, false, p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);
                result.Result = data;
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }

        public async Task<AppActionResult> GetAllReferenceTransportByProvinceId(FilterReferenceTransportPrice filter, int pageIndex, int pageSize)
        {
            var result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var communeRepository = Resolve<IRepository<Commune>>();
                var districtRepository = Resolve<IRepository<District>>();
                var provinceDb = await provinceRepository!.GetById(filter.FirstLocation.ProvinceId);
                var secondProvinceDb = await provinceRepository!.GetById(filter.SecondLocation.ProvinceId);
                if (provinceDb == null || secondProvinceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Tỉnh với {filter.FirstLocation.ProvinceId} và {filter.SecondLocation.ProvinceId} này không tồn tại");
                    if (filter.FirstLocation.DistrictId != null && filter.SecondLocation.DistrictId != null)
                    {
                        var firstDistrict = await districtRepository!.GetById(filter.FirstLocation.DistrictId!);
                        var secondDistrict = await districtRepository!.GetById(filter.SecondLocation.DistrictId!);
                        if (firstDistrict == null || secondDistrict == null)
                        {
                            result = BuildAppActionResultError(result, $"Huyện/xã với {filter.FirstLocation.DistrictId} và {filter.SecondLocation.DistrictId} này không tồn tại");
                        }
                        if (filter.FirstLocation.CommuneId != null && filter.SecondLocation.CommuneId != null)
                        {
                            var firstCommunce = await communeRepository!.GetById(filter.FirstLocation.CommuneId!);
                            var secondCommunce = await communeRepository!.GetById(filter.SecondLocation.CommuneId!);
                            if (firstCommunce == null || secondCommunce == null)
                            {
                                result = BuildAppActionResultError(result, $"Phường với {filter.FirstLocation.CommuneId} và {filter.SecondLocation.CommuneId} này không tồn tại");
                            }
                        }
                    }
                }
                if (!BuildAppActionResultIsError(result))
                {
                    if (filter.StartDate == null && filter.EndDate == null)
                    {
                        result.Result = await _repository.GetAllDataByExpression(p =>
                        p!.Departure!.Commune!.District!.ProvinceId == filter.FirstLocation.ProvinceId &&
                        p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                        p!.Arrival!.Commune!.District!.ProvinceId == filter.SecondLocation.ProvinceId
                       || p.Departure.Commune.District.ProvinceId == filter.SecondLocation.ProvinceId &&
                       p.Arrival!.Commune!.District!.ProvinceId == filter.FirstLocation.ProvinceId,
                       pageIndex, pageSize
                       , null, false,
                       p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);
                    }
                    else
                    {
                        result.Result = await _repository.GetAllDataByExpression(p =>
                           p!.Departure!.Commune!.District!.ProvinceId == filter.FirstLocation.ProvinceId &&
                           p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                           p!.Arrival!.Commune!.District!.ProvinceId == filter.SecondLocation.ProvinceId &&
                           p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate
                          ||
                          p.Departure.Commune.District.ProvinceId == filter.SecondLocation.ProvinceId &&
                          p.Arrival!.Commune!.District!.ProvinceId == filter.FirstLocation.ProvinceId &&
                          p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                           p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate
                          ,
                          pageIndex, pageSize
                          , null, false,
                          p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);
                    }
                    if (filter.FirstLocation.DistrictId != null && filter.SecondLocation.DistrictId != null && filter.StartDate == null && filter.EndDate == null)
                    {
                        result.Result = await _repository.GetAllDataByExpression(p => p!.Departure!.Commune!.DistrictId == filter.FirstLocation.DistrictId &&
                       p!.Arrival!.Commune!.DistrictId == filter.SecondLocation.DistrictId &&
                       p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType
                      || p.Departure.Commune.DistrictId == filter.SecondLocation.DistrictId &&
                       p.Arrival!.Commune!.DistrictId == filter.FirstLocation.DistrictId &&
                       p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType,
                       pageIndex, pageSize
                      , null, false,
                       p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);
                    }
                    else
                    {
                        result.Result = await _repository.GetAllDataByExpression(p => p!.Departure!.Commune!.DistrictId == filter.FirstLocation.DistrictId &&
                         p!.Arrival!.Commune!.DistrictId == filter.SecondLocation.DistrictId &&
                        p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                        p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate

                        || p.Departure.Commune.DistrictId == filter.SecondLocation.DistrictId &&
                        p.Arrival!.Commune!.DistrictId == filter.FirstLocation.DistrictId &&
                        p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                        p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate,
                        pageIndex, pageSize, null, false,
                        p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);
                    }
                    if (filter.FirstLocation.CommuneId != null && filter.SecondLocation.CommuneId != null && filter.StartDate == null && filter.EndDate == null)
                    {
                        result.Result = await _repository.GetAllDataByExpression(p =>
                        p!.Departure!.CommuneId == filter.FirstLocation.CommuneId &&
                        p!.Arrival!.CommuneId == filter.SecondLocation.CommuneId &&
                        p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType

                        || p.Departure.CommuneId == filter.SecondLocation.CommuneId &&
                        p.Arrival!.CommuneId == filter.FirstLocation.CommuneId &&
                        p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType,
                        pageIndex, pageSize
                        , null, false,
                        p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);

                    }
                    else
                    {
                        result.Result = await _repository.GetAllDataByExpression(p =>
                       p!.Departure!.CommuneId == filter.FirstLocation.CommuneId &&
                       p!.Arrival!.CommuneId == filter.SecondLocation.CommuneId &&
                       p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                       p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate

                       || p.Departure.CommuneId == filter.SecondLocation.CommuneId &&
                       p.Arrival!.CommuneId == filter.FirstLocation.CommuneId &&
                       p!.Departure.PortType == filter.PortType && p!.Arrival!.PortType == filter.PortType &&
                       p!.DepartureDate == filter.StartDate && p!.ArrivalDate == filter.EndDate,
                       pageIndex, pageSize
                       , null, false,
                       p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!);

                    }

                }
            }
            catch (Exception e)
            {
                result = BuildAppActionResultError(result, $"Có lỗi xảy ra {e.Message}");
            }
            return result;
        }
    }
}
