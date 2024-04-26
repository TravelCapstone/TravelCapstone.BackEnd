using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class SellPriceHistoryService: GenericBackendService, ISellPriceHistoryService
    {
        private readonly IRepository<SellPriceHistory> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IFileService _fileService;

        public SellPriceHistoryService(
            IRepository<SellPriceHistory> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
        IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<AppActionResult> GetSellPriceByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var facilityRepository = Resolve<IRepository<Facility>>();
                var facilityDb = await facilityRepository!.GetByExpression(f => f.Id == facilityId);
                if (facilityDb != null)
                {
                    var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                    var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(fs => fs.FacilityId == facilityDb.Id && fs.ServiceTypeId == serviceTypeId, 0, 0, null, false, null);
                    if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                    {
                        var facilityServiceIds = facilityServiceDb.Items.Select(f => f.Id).ToList();
                        var menuRepository = Resolve<IRepository<Menu>>();
                        var menuDb = await menuRepository!.GetAllDataByExpression(m => facilityServiceIds.Contains(m.FacilityServiceId), 0, 0, null, false, null);
                        var transportServiceDetailRepository = Resolve<IRepository<TransportServiceDetail>>();
                        var transportDb = await transportServiceDetailRepository!.GetAllDataByExpression(m => facilityServiceIds.Contains(m.FacilityServiceId), 0, 0, null, false, null);
                        var menuIds = menuDb.Items!.Select(m => m.Id).ToList();
                        var transportIds = transportDb.Items!.Select(m => m.Id).ToList();
                        var sellPriceHistoryRepository = Resolve<IRepository<SellPriceHistory>>();
                        var sellPriceHistoryDb = await sellPriceHistoryRepository!.GetAllDataByExpression(s => (s.FacilityServiceId != null && facilityServiceIds.Contains((Guid)s.FacilityServiceId))
                                                                                                                    || (s.MenuId != null && menuIds.Contains((Guid)s.MenuId))
                                                                                                                    || (s.TransportServiceDetailId != null && transportIds.Contains((Guid)s.TransportServiceDetailId)), 
                                                                                                                    0, 0, s => s.Date, false, s => s.TransportServiceDetail.FacilityService, s => s.FacilityService, s => s.Menu.FacilityService);
                        result.Result = sellPriceHistoryDb;
                    }
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<IActionResult> GetTemplate()
        {
            IActionResult result = null;
            try
            {
                List<ServiceCostHistoryRecord> sampleData = new List<ServiceCostHistoryRecord>();
                sampleData.Add(new ServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, PricePerAdult = 9, PricePerChild = 4 });
                result = _fileService.GenerateExcelContent<ServiceCostHistoryRecord, Object>(sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION, "ProviderName_ddMMyyyy");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public Task<AppActionResult> UploadQuotation(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<AppActionResult> ValidateExcelFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
