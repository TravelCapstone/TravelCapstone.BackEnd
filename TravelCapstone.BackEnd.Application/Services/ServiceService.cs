using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class ServiceService : GenericBackendService, IServiceService
    {
        private readonly IRepository<Service> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(
            IRepository<Service> repository,
            IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppActionResult> GetServiceByProvinceIdAndServiceType(Guid Id, ServiceType type)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var districtRepository = Resolve<IRepository<District>>();
                var districtListDb = await districtRepository!.GetAllDataByExpression(d => d.ProvinceId == Id, 1, Int32.MaxValue);
                if (districtListDb == null || districtListDb.Items!.Count == 0)
                {
                    return result;
                }
                var districtIds = districtListDb.Items.Select(d => d.Id);
                var communeRepository = Resolve<IRepository<Commune>>();
                var communeListDb = await communeRepository!.GetAllDataByExpression(c => districtIds.Contains(c.DistrictId), 1, Int32.MaxValue);
                if (communeListDb == null || communeListDb.Items!.Count == 0)
                {
                    return result;
                }
                var communeIds = communeListDb.Items.Select(d => d.Id);
                var serviceListDb = await _repository.GetAllDataByExpression(s => communeIds.Contains(s.CommunceId) && s.Type== type, 1, Int32.MaxValue);
                result.Result = serviceListDb;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
