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
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class ServiceCostHistoryService : GenericBackendService, IServiceCostHistoryService
    {
        private readonly IRepository<ServiceCostHistory> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IFileService _fileService;

        public ServiceCostHistoryService(
            IRepository<ServiceCostHistory> repository,
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
        public async Task<IActionResult> GetPriceQuotationTemplate()
        {
            IActionResult result = null;
            try
            {
                List<ServiceCostHistoryRecord> sampleData = new List<ServiceCostHistoryRecord>();
                sampleData.Add(new ServiceCostHistoryRecord
                { No=1, ServiceName = "Service name", Unit = "Bar", MOQ = 1000, PricePerAdult = 9, PricePerChild = 4 });
                result = _fileService.GenerateExcelContent<ServiceCostHistoryRecord>(sampleData, "ProviderName_ddMMyyyy");

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
                var serviceRepository = Resolve<IRepository<Service>>();
                var serviceProviderDb = await serviceProviderRepository.GetByExpression(s => s.Name == serviceInfos[0]);
                DateTime.TryParseExact(serviceInfos[1].Substring(0, 8), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                List<ServiceCostHistoryRecord> records = await GetListFromExcel(file);
                List<ServiceCostHistory> data= new List<ServiceCostHistory>();
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
                        //PricePerAdult = record.PricePerAdult,
                       // PricePerChild = record.PricePerChild,
                        MOQ = record.MOQ,
                    //    Unit = (Domain.Enum.Unit)SD.EnumType.ServiceCostUnit[record.Unit],
                        Date = date,
                        ServiceId = serviceId
                    });
                }
            await _repository.InsertRange(data);
            if(!BuildAppActionResultIsError(result))
            {
                result.Result = data;
                await _unitOfWork.SaveChangesAsync();
            }
            } catch (Exception ex)
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

                string errorHeader = await CheckHeader(file, SD.ExcelHeaders.SERVICE_QUOTATION);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                var serviceRepository = Resolve<IRepository<Service>>();
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
                        continue;
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

                    if (record.PricePerAdult <= 0 || record.PricePerChild <= 0)
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

                    if ((await _repository.GetByExpression(m => m.MOQ == record.MOQ && m.ServiceId == serviceId && date == m.Date)) != null)
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
                                PricePerAdult = (worksheet.Cells[row, 5].Value == null) ? 0 : double.Parse(worksheet.Cells[row, 5].Value.ToString()),
                                PricePerChild = (worksheet.Cells[row, 6].Value == null) ? 0 : double.Parse(worksheet.Cells[row, 6].Value.ToString())
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

        private async Task<string> CheckHeader(IFormFile file, List<string> headerTemplate)
        {
            if (file == null || file.Length == 0)
            {
                return "Không tìm thấy file";
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

                        int colCount = worksheet.Columns.Count();
                        if (colCount != headerTemplate.Count && worksheet.Cells[1, colCount].Value != null)
                        {
                            return "Số lượng cột không đúng.";
                        }
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Tên cột sai: ");
                        bool containsError = false;
                        for (int col = 1; col <= Math.Min(5, worksheet.Columns.Count()); col++) // Assuming header is in the first row
                        {
                            if (!worksheet.Cells[1, col].Value.Equals(headerTemplate[col - 1]))
                            {
                                if (!containsError) containsError = true;
                                sb.Append($"{worksheet.Cells[1, col].Value}(Tên đúng: {headerTemplate[col - 1]}), ");
                            }
                        }
                        if (containsError)
                        {
                            return sb.Remove(sb.Length - 2, 2).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
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
                        a => a.ServiceId == serviceId,
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

    }



}
