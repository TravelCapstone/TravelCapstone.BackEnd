using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ServiceCostHistoryService : GenericBackendService, IServiceCostHistoryService
    {
        private readonly IRepository<ServiceCostHistory> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IFileService _fileService;
        private readonly IExcelService _excelService;

        public ServiceCostHistoryService(
            IRepository<ServiceCostHistory> repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileService fileService,
            IExcelService excelService,
        IServiceProvider serviceProvider
        ) : base(serviceProvider)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _excelService = excelService;
        }
        public async Task<IActionResult> GetPriceQuotationTemplate()
        {
            IActionResult result = null;
            try
            {
                List<ServiceCostHistoryRecord> sampleData = new List<ServiceCostHistoryRecord>();
                sampleData.Add(new ServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, Price = 4 });
                result = _fileService.GenerateExcelContent<ServiceCostHistoryRecord, Object>(sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION,"ProviderName");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public async Task<AppActionResult> UploadQuotation(IFormFile file)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var validation = await ValidateExcelFile(file);
                ExcelValidatingResponse validationResponse = validation.Result as ExcelValidatingResponse;
                if (validationResponse == null)
                {
                    result = BuildAppActionResultError(result, "Kiểm tra file Excel xảy ra lỗi.\n Vui lòng thử lại.");
                    return result;
                }
                if (!validationResponse.IsValidated)
                {
                    result.Result = validationResponse;
                    return result;
                }
                string[] serviceInfos = file.FileName.Split('_');
                var serviceProviderRepository = Resolve<IRepository<ServiceProvider>>();
                var serviceRepository = Resolve<IRepository<Facility>>();
                var serviceProviderDb = await serviceProviderRepository.GetByExpression(s => s.Name == serviceInfos[0]);
                DateTime.TryParseExact(serviceInfos[1].Substring(0, 8), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                List<ServiceCostHistoryRecord> records = await GetListFromExcel(file);
                List<ServiceCostHistory> data = new List<ServiceCostHistory>();
                var serviceId = Guid.Empty;
                Dictionary<string, Guid> serviceIds = new Dictionary<string, Guid>();
                string key;

                foreach (var record in records)
                {
                    key = record.ServiceName + '-' + record.Unit;
                    if (serviceIds.ContainsKey(key)) serviceId = serviceIds[key];
                    else
                    {
                        bool containsUnit = SD.EnumType.ServiceCostUnit.TryGetValue(record.Unit, out int index);
                        if (containsUnit)
                        {
                            var service = await serviceRepository.GetByExpression(m => m.Name.Equals(record.ServiceName) && m.ServiceProviderId == serviceProviderDb.Id);
                            serviceId = service.Id;
                            serviceIds.Add(key, serviceId);
                        }
                        else
                        {
                            result = BuildAppActionResultError(result, "Gặp lỗi trong quá trình tải. Vui lòng kiểm tra thông tin và thủ lại");
                        }
                    }
                    data.Add(new ServiceCostHistory
                    {
                        Id = Guid.NewGuid(),
                        Price = record.Price,
                        MOQ = record.MOQ,
                        Date = date,
                        FacilityServiceId = serviceId
                    });
                }
                await _repository.InsertRange(data);
                if (!BuildAppActionResultIsError(result))
                {
                    result.Result = data;
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> ValidateExcelFile(IFormFile file)
        {
            AppActionResult result = new AppActionResult();
            ExcelValidatingResponse data = new ExcelValidatingResponse();
            if (file == null || file.Length == 0)
            {
                data.IsValidated = false;
                data.HeaderError = "Kiểm tra file Excel xảy ra lỗi.\n Vui lòng thử lại.";
                result.Result = data;
                return result;
            }
            try
            {
                //Format: Name_ddmmyyy
                string nameDateString = file.FileName;
                if (file.FileName.Contains("(ErrorColor)"))
                    nameDateString = nameDateString.Substring("(ErrorColor)".Length);
                string[] serviceInfo = nameDateString.Split('_');
                if (serviceInfo.Length != 2)
                {
                    data.IsValidated = false;
                    data.HeaderError = "Tên file không đúng định dạng.\nVui lòng làm theo định dạng: tênNhàCungCấp_ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string serviceProviderName = serviceInfo[0];
                if (serviceInfo[1].Length < 8)
                {
                    data.IsValidated = false;
                    data.HeaderError = "Sai định dạng ngày. Vui lòng làm theo định dạng: ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string dateString = serviceInfo[1].Substring(0, 8);
                if (!DateTime.TryParseExact(dateString, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    data.IsValidated = false;
                    data.HeaderError = $"{dateString} không theo định dạng: ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string errorHeader = await _excelService.CheckHeader(file, SD.ExcelHeaders.MENU_SERVICE_QUOTATION);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                string menuErrorHeader = await _excelService.CheckHeader(file, SD.ExcelHeaders.MENU_DISH, 1);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                var serviceRepository = Resolve<IRepository<Facility>>();
                var serviceProviderRepository = Resolve<IRepository<ServiceProvider>>();
                var serviceProdviderDb = await serviceProviderRepository!.GetByExpression(s => s.Name.ToLower().Equals(serviceProviderName.ToLower()));
                if (serviceProdviderDb == null)
                {
                    data.IsValidated = false;
                    data.HeaderError = $"Nhà cung cấp dịch vụ tên: {serviceProviderName} không tồn tại!";
                    result.Result = data;
                    return result;
                }

                List<ServiceCostHistoryRecord> records = await GetListFromExcel(file);
                if (records.Count == 0)
                {
                    data.IsValidated = false;
                    data.HeaderError = $"Danh sách báo giá rỗng";
                    result.Result = data;
                    return result;
                }


                int errorRecordCount = 0;
                int i = 2;
                int invalidRowInput = 0;
                string key = "";
                data.Errors = new string[records.Count];
                Dictionary<string, Guid> serviceIds = new Dictionary<string, Guid>();
                Dictionary<string, int> duplicatedQuotation = new Dictionary<string, int>();
                foreach (ServiceCostHistoryRecord record in records)
                {
                    StringBuilder error = new StringBuilder();
                    errorRecordCount = 0;
                    Guid serviceId = Guid.Empty;
                    if (record.No != i - 1)
                    {
                        error.Append($"{errorRecordCount + 1}. Thứ tự đúng {i - 1}.\n");
                        errorRecordCount++;
                    }

                    if (string.IsNullOrEmpty(record.ServiceName) || string.IsNullOrEmpty(record.Unit))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô tên hoặc đơn vị tính trống.\n");
                        errorRecordCount++;
                    }

                    //SD.EnumType.serviceUnit.TryGetValue(record.Unit, out int serviceUnit);
                    key = record.ServiceName + '-' + record.Unit;

                    if (serviceIds.ContainsKey(key)) serviceId = serviceIds[key];
                    else
                    {
                        bool containsUnit = SD.EnumType.ServiceCostUnit.TryGetValue(record.Unit, out int index);
                        if (containsUnit)
                        {
                            var service = await serviceRepository!.GetByExpression(m => m.Name.Equals(record.ServiceName) && m.ServiceProviderId == serviceProdviderDb.Id && m.IsActive);
                            if (service == null)
                            {
                                error.Append($"{errorRecordCount + 1}. Dịch vụ: {record.ServiceName} từ nhà cung cấp {serviceProdviderDb.Name} không tồn tại hoặc không có sẵn.\n");
                                errorRecordCount++;
                            }
                            else
                            {
                                serviceId = service.Id;
                                serviceIds.Add(key, serviceId);
                            }
                        }
                        else
                        {
                            error.Append($"{errorRecordCount + 1}. Đơn vị tính: {record.Unit} không tồn tại.\n");
                            errorRecordCount++;
                        }
                    }

                    if (record.MOQ <= 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Số lượng tối thiểu phải lớn hơn 0.\n");
                        errorRecordCount++;
                    }

                    if (record.Price <= 0)
                    {
                        error.Append($"{errorRecordCount + 1}.Đơn giá tối thiểu phải lớn hơn 0.\n");
                        errorRecordCount++;
                    }

                    string duplicatedKey = $"{record.ServiceName}-{record.MOQ}";
                    if (duplicatedQuotation.ContainsKey(duplicatedKey))
                    {
                        error.Append($"{errorRecordCount + 1}. Đơn dịch vụ và đơn giá tối thiểu trùng tại dòng {duplicatedQuotation[duplicatedKey]}.\n");
                        errorRecordCount++;
                    }
                    else
                    {
                        duplicatedQuotation.Add(duplicatedKey, i - 1);
                    }

                    if ((await _repository.GetByExpression(m => m.MOQ == record.MOQ && m.FacilityServiceId == serviceId && date == m.Date)) != null)
                    {
                        error.Append($"{errorRecordCount + 1}. Tồn tại một báo giá dịch vụ tương tự trong cùng ngày.\n");
                        errorRecordCount++;
                    }

                    if (errorRecordCount != 0)
                    {
                        data.Errors[i - 2] = error.ToString();
                        invalidRowInput++;
                    }
                    i++;
                }

                if (invalidRowInput > 0)
                {
                    data.IsValidated = false;
                    result.Result = data;
                    return result;
                }

                data.IsValidated = true;
                data.Errors = null;
                data.HeaderError = null;
                result.Result = data;
            }
            catch (Exception ex)
            {
                data.IsValidated = false;
            }
            return result;

        }
        private async Task<List<ServiceCostHistoryRecord>> GetListFromExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        List<ServiceCostHistoryRecord> records = new List<ServiceCostHistoryRecord>();

                        for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                        {
                            ServiceCostHistoryRecord record = new ServiceCostHistoryRecord()
                            {
                                No = (worksheet.Cells[row, 1].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                ServiceName = (worksheet.Cells[row, 2].Value == null) ? "" : worksheet.Cells[row, 2].Value.ToString(),
                                Unit = (worksheet.Cells[row, 3].Value == null) ? "" : worksheet.Cells[row, 3].Value.ToString(),
                                MOQ = (worksheet.Cells[row, 4].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 4].Value.ToString()),
                                Price = (worksheet.Cells[row, 5].Value == null) ? 0 : double.Parse(worksheet.Cells[row, 5].Value.ToString()),
                            };
                            records.Add(record);
                        }
                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<AppActionResult> GetLastCostHistory(List<Guid> servicesId)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var latestCostHistory = new Dictionary<Guid, ServiceCostHistory>();

                foreach (var serviceId in servicesId)
                {
                    var latestHistoryForService = await _repository.GetAllDataByExpression(
                        a => a.FacilityServiceId == serviceId,
                        0,
                        0,
                        null
                    );
                    latestHistoryForService.Items = latestHistoryForService.Items!.OrderByDescending(a => a.Date).ToList();
                    if (latestHistoryForService.Items != null && latestHistoryForService.Items.Any())
                    {
                        latestCostHistory[serviceId] = latestHistoryForService.Items.First();
                    }
                }


                result.Result = latestCostHistory.Values;
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetServiceCostByFacilityIdAndServiceType(Guid facilityId, ServiceType serviceTypeId, int pageNumber, int pageSize)
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
                        var servicecostHistoryRepository = Resolve<IRepository<ServiceCostHistory>>();
                        var servicecostHistoryDb = await servicecostHistoryRepository!.GetAllDataByExpression(s => (s.FacilityServiceId != null && facilityServiceIds.Contains((Guid)s.FacilityServiceId))
                                                                                                                    || (s.MenuId != null && menuIds.Contains((Guid)s.MenuId))
                                                                                                                    || (s.TransportServiceDetailId != null && transportIds.Contains((Guid)s.TransportServiceDetailId))
                                                                                                                    , pageNumber, pageSize, s => s.Date, false, s => s.Transport.FacilityService, s => s.FacilityService, s => s.Menu.FacilityService);
                        result.Result = servicecostHistoryDb;
                    }
                }

            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> UploadMenuQuotation(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<AppActionResult> ValidateMenuExcelFile(IFormFile file)
        {
            AppActionResult result = new AppActionResult();
            ExcelValidatingResponse data = new ExcelValidatingResponse();
            if (file == null || file.Length == 0)
            {
                data.IsValidated = false;
                data.HeaderError = "Kiểm tra file Excel xảy ra lỗi.\n Vui lòng thử lại.";
                result.Result = data;
                return result;
            }
            try
            {
                //Format: Name_ddmmyyy
                string nameDateString = file.FileName;
                if (file.FileName.Contains("(ErrorColor)"))
                    nameDateString = nameDateString.Substring("(ErrorColor)".Length);
                string[] serviceInfo = nameDateString.Split('_');
                if (serviceInfo.Length != 2)
                {
                    data.IsValidated = false;
                    data.HeaderError = "Tên file không đúng định dạng.\nVui lòng làm theo định dạng: tênNhàCungCấp_ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string serviceProviderName = serviceInfo[0];
                if (serviceInfo[1].Length < 8)
                {
                    data.IsValidated = false;
                    data.HeaderError = "Sai định dạng ngày. Vui lòng làm theo định dạng: ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string dateString = serviceInfo[1].Substring(0, 8);
                if (!DateTime.TryParseExact(dateString, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    data.IsValidated = false;
                    data.HeaderError = $"{dateString} không theo định dạng: ddMMyyyy";
                    result.Result = data;
                    return result;
                }

                string errorHeader = await _excelService.CheckHeader(file, SD.ExcelHeaders.MENU_SERVICE_QUOTATION);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                errorHeader = await _excelService.CheckHeader(file, SD.ExcelHeaders.MENU_DISH);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                var serviceRepository = Resolve<IRepository<Facility>>();
                var serviceProviderRepository = Resolve<IRepository<ServiceProvider>>();
                var serviceProdviderDb = await serviceProviderRepository.GetByExpression(s => s.Name.ToLower().Equals(serviceProviderName.ToLower()));
                if (serviceProdviderDb == null)
                {
                    data.IsValidated = false;
                    data.HeaderError = $"Nhà cung cấp dịch vụ tên: {serviceProviderName} không tồn tại!";
                    result.Result = data;
                    return result;
                }

                List<ServiceCostHistoryRecord> records = await GetListFromExcel(file);
                if (records.Count == 0)
                {
                    data.IsValidated = false;
                    data.HeaderError = $"Danh sách báo giá rỗng";
                    result.Result = data;
                    return result;
                }

                int errorRecordCount = 0;
                int i = 2;
                int invalidRowInput = 0;
                string key = "";
                data.Errors = new string[records.Count];
                Dictionary<string, Guid> serviceIds = new Dictionary<string, Guid>();
                Dictionary<string, int> duplicatedQuotation = new Dictionary<string, int>();
                foreach (ServiceCostHistoryRecord record in records)
                {
                    StringBuilder error = new StringBuilder();
                    errorRecordCount = 0;
                    Guid serviceId = Guid.Empty;
                    if (record.No != i - 1)
                    {
                        error.Append($"{errorRecordCount + 1}. Thứ tự đúng {i - 1}.\n");
                        errorRecordCount++;
                    }

                    if (string.IsNullOrEmpty(record.ServiceName) || string.IsNullOrEmpty(record.Unit))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô tên hoặc đơn vị tính trống.\n");
                        errorRecordCount++;
                    }

                    //SD.EnumType.serviceUnit.TryGetValue(record.Unit, out int serviceUnit);
                    key = record.ServiceName + '-' + record.Unit;

                    if (serviceIds.ContainsKey(key)) serviceId = serviceIds[key];
                    else
                    {
                        bool containsUnit = SD.EnumType.ServiceCostUnit.TryGetValue(record.Unit, out int index);
                        if (containsUnit)
                        {
                            var service = await serviceRepository.GetByExpression(m => m.Name.Equals(record.ServiceName) && m.ServiceProviderId == serviceProdviderDb.Id && m.IsActive);
                            if (service == null)
                            {
                                error.Append($"{errorRecordCount + 1}. Dịch vụ: {record.ServiceName} từ nhà cung cấp {serviceProdviderDb.Name} không tồn tại hoặc không có sẵn.\n");
                                errorRecordCount++;
                            }
                            else
                            {
                                serviceId = service.Id;
                                serviceIds.Add(key, serviceId);
                            }
                        }
                        else
                        {
                            error.Append($"{errorRecordCount + 1}. Đơn vị tính: {record.Unit} không tồn tại.\n");
                            errorRecordCount++;
                        }
                    }
                    //else
                    //{
                    //    error.Append($"{errorRecordCount + 1}. Supplier {serviceProviderName} is a {supplier.Type.ToString()} so they don't supply {record.serviceName}.\n");
                    //    errorRecordCount++;
                    //}

                    if (record.MOQ <= 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Số lượng tối thiểu phải lớn hơn 0.\n");
                        errorRecordCount++;
                    }

                    if (record.Price <= 0)
                    {
                        error.Append($"{errorRecordCount + 1}.Đơn giá tối thiểu phải lớn hơn 0.\n");
                        errorRecordCount++;
                    }

                    string duplicatedKey = $"{record.ServiceName}-{record.MOQ}";
                    if (duplicatedQuotation.ContainsKey(duplicatedKey))
                    {
                        error.Append($"{errorRecordCount + 1}. Đơn dịch vụ và đơn giá tối thiểu trùng tại dòng {duplicatedQuotation[duplicatedKey]}.\n");
                        errorRecordCount++;
                    }
                    else
                    {
                        duplicatedQuotation.Add(duplicatedKey, i - 1);
                    }

                    if ((await _repository.GetByExpression(m => m.MOQ == record.MOQ && m.FacilityServiceId == serviceId && date == m.Date)) != null)
                    {
                        error.Append($"{errorRecordCount + 1}. Tồn tại một báo giá dịch vụ tương tự trong cùng ngày.\n");
                        errorRecordCount++;
                    }

                    if (errorRecordCount != 0)
                    {
                        data.Errors[i - 2] = error.ToString();
                        invalidRowInput++;
                    }
                    i++;
                }

                if (invalidRowInput > 0)
                {
                    data.IsValidated = false;
                    result.Result = data;
                    return result;
                }

                data.IsValidated = true;
                data.Errors = null;
                data.HeaderError = null;
                result.Result = data;
            }
            catch (Exception ex)
            {
                data.IsValidated = false;
            }
            return result;
        }

        public async Task<IActionResult> GetMenuPriceQuotationTemplate()
        {
            IActionResult result = null;
            try
            {
                List<MenuServiceCostHistoryRecord> sampleData = new List<MenuServiceCostHistoryRecord>();
                sampleData.Add(new MenuServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, Price = 4, MenuName="Thực đơn ăn sáng mặn" });
               
                result = _fileService.GenerateExcelContent<MenuServiceCostHistoryRecord, MenuRecord>(sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION, "Báo giá menu");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public async Task<IActionResult> GetMenuDishUpdateTemplate()
        {
            IActionResult result = null;
            try
            {
                List<MenuServiceCostHistoryRecord> sampleData = new List<MenuServiceCostHistoryRecord>();
                sampleData.Add(new MenuServiceCostHistoryRecord
                { No = 1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, Price = 4, MenuName = "Thực đơn ăn sáng mặn" });
                List<MenuRecord> menuSampleData = new List<MenuRecord>();
                menuSampleData.Add(new MenuRecord
                { No = 1, FacilityServiceName = "Facility Service name", MenuName = "Thực đơn ăn sáng mặn", DishName = "Canh chua cá mập", Description = "Ngon", MenuType = "Soup" });
                result = _fileService.GenerateExcelContent<MenuServiceCostHistoryRecord, MenuRecord>(sampleData, menuSampleData, SD.ExcelHeaders.SERVICE_QUOTATION, "Báo giá menu", SD.ExcelHeaders.MENU_SERVICE_QUOTATION, "Cập nhật menu");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public async Task<List<MenuRecord>> GetMenuRecordList(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Assuming data is in the first sheet

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        List<MenuRecord> records = new List<MenuRecord>();

                        for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                        {
                            MenuRecord record = new MenuRecord()
                            {
                                No = (worksheet.Cells[row, 1].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 1].Value.ToString()),                              
                                FacilityServiceName = (worksheet.Cells[row, 2].Value == null) ? "" : worksheet.Cells[row, 2].Value.ToString(),
                                MenuName = (worksheet.Cells[row, 3].Value == null) ? "" : worksheet.Cells[row, 3].Value.ToString(),
                                DishName = (worksheet.Cells[row, 4].Value == null) ? "" : worksheet.Cells[row, 4].Value.ToString(),
                                Description = (worksheet.Cells[row, 5].Value == null) ? "" : worksheet.Cells[row, 5].Value.ToString(),
                                MenuType = (worksheet.Cells[row, 6].Value == null) ? "" : worksheet.Cells[row, 6].Value.ToString()
                            };
                            records.Add(record);
                        }
                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<List<MenuServiceCostHistoryRecord>> GetMenuQuotationList(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first sheet

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;

                        List<MenuServiceCostHistoryRecord> records = new List<MenuServiceCostHistoryRecord>();

                        for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                        {
                            MenuServiceCostHistoryRecord record = new MenuServiceCostHistoryRecord()
                            {
                                No = (worksheet.Cells[row, 1].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                ServiceName = (worksheet.Cells[row, 2].Value == null) ? "" : worksheet.Cells[row, 2].Value.ToString(),
                                MenuName = (worksheet.Cells[row, 3].Value == null) ? "" : worksheet.Cells[row, 3].Value.ToString(),
                                Unit = (worksheet.Cells[row, 4].Value == null) ? "" : worksheet.Cells[row, 4].Value.ToString(),
                                MOQ = (worksheet.Cells[row, 5].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 5].Value.ToString()),
                                Price = (worksheet.Cells[row, 6].Value == null) ? 0 : double.Parse(worksheet.Cells[row, 6].Value.ToString()),
                            };
                            records.Add(record);
                        }
                        return records;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

    }
}
