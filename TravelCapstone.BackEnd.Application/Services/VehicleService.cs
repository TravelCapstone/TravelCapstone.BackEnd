using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IRepositories;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;
using TravelCapstone.BackEnd.Common.Utils;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;
using Vehicle = TravelCapstone.BackEnd.Domain.Models.Vehicle;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class VehicleService : GenericBackendService, IVehicleService
    {
        private readonly IRepository<Vehicle> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private IFileService _fileService;

        public VehicleService(
            IRepository<Vehicle> repository,
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
                List<VehicleRecord> sampleData = new List<VehicleRecord>();
                sampleData.Add(new VehicleRecord
                { No = 1, VehicleType = "X7", Plate = "PLATE INFO", Capacity = 7, EngineNumber = "ENGINE NUMBER", ChassisNumber = "CHASSIS NUMBER", Brand = "Toyota", Owner = "Nguyen Van Anh", Color = "Vàng" });
                result = _fileService.GenerateExcelContent<VehicleRecord, Object>("NHẬP THÔNG TIN XE",sampleData, null, SD.ExcelHeaders.SERVICE_QUOTATION, "ProviderName_ddMMyyyy");

            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string VehicleType { get; set; }
        public string Plate { get; set; } = null!;
        public int Capacity { get; set; }
        public string EngineNumber { get; set; } = null!;
        public string ChassisNumber { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Color { get; set; } = null!;

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
                List<VehicleRecord> records = await GetListFromExcel(file);
                List<Vehicle> data = new List<Vehicle>();

                foreach (var record in records)
                {

                    data.Add(new Vehicle
                    {
                        Id = Guid.NewGuid(),
                        Plate = record.Plate,
                        ChassisNumber = record.ChassisNumber,
                        EngineNumber = record.EngineNumber,
                        Capacity = record.Capacity,
                        SeatCapacity = record.SeatCapacity,
                        Brand = record.Brand,
                        Owner = record.Owner,
                        Color = record.Color
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
                string errorHeader = await CheckHeader(file, SD.ExcelHeaders.VEHICLE_REGISTRATION);
                if (!string.IsNullOrEmpty(errorHeader))
                {
                    data.IsValidated = false;
                    data.HeaderError = errorHeader;
                    result.Result = data;
                    return result;
                }

                List<VehicleRecord> records = await GetListFromExcel(file);
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
                data.Errors = new string[records.Count];
                Dictionary<string, int> chassisNumberSet = new Dictionary<string, int>();
                Dictionary<string, int> engineNumberSet = new Dictionary<string, int>();
                var vehicleRepository = Resolve<IRepository<Vehicle>>();
                foreach (VehicleRecord record in records)
                {
                    StringBuilder error = new StringBuilder();
                    errorRecordCount = 0;
                    Guid serviceId = Guid.Empty;
                    if (record.No != i - 1)
                    {
                        error.Append($"{errorRecordCount + 1}. Thứ tự đúng {i - 1}.\n");
                        errorRecordCount++;
                    }
                    //{ "No", "Biển số", "Dung tích", "Số động cơ", "Số khung", "Thương hiệu", "Tên chủ sở hữu", "Màu" };
                    if (string.IsNullOrEmpty(record.Plate))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô biển số trống.\n");
                        errorRecordCount++;
                    }
                    else if (!Regex.IsMatch(record.Plate, SD.Regex.PLATE))
                    {
                        error.Append($"{errorRecordCount + 1}. Biển số xe không hợp lệ.\n");
                        errorRecordCount++;
                    }

                    if (string.IsNullOrEmpty(record.EngineNumber))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô số động cơ trống.\n");
                        errorRecordCount++;
                    }
                    else if (!Regex.IsMatch(record.EngineNumber, SD.Regex.ENGINENUMBER))
                    {
                        error.Append($"{errorRecordCount + 1}. Số động cơ không hợp lệ.\n");
                        errorRecordCount++;
                    }
                    else
                    {
                        if (engineNumberSet.ContainsKey(record.EngineNumber))
                        {
                            error.Append($"{errorRecordCount + 1}. Số động cơ trùng với thông tin xe ở dòng {engineNumberSet[record.EngineNumber]}.\n");
                            errorRecordCount++;
                        }
                        else
                        {
                            var vehicleDb = await vehicleRepository!.GetAllDataByExpression(v => v.EngineNumber.Equals(record.EngineNumber), 0, 0, null, false, null);
                            if (vehicleDb.Items != null && vehicleDb.Items.Count > 0)
                            {
                                error.Append($"{errorRecordCount + 1}. Số động cơ {record.EngineNumber} đã tồn tại trong hệ thống.\n");
                                errorRecordCount++;
                            }
                            else
                            {
                                engineNumberSet.Add(record.EngineNumber, i - 1);
                            }
                        }

                    }

                    if (string.IsNullOrEmpty(record.ChassisNumber))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô số khung trống.\n");
                        errorRecordCount++;
                    }
                    else if (!Regex.IsMatch(record.ChassisNumber, SD.Regex.CHASSISNUMBER))
                    {
                        error.Append($"{errorRecordCount + 1}. Số khung không hợp lệ.\n");
                        errorRecordCount++;
                    }
                    else
                    {
                        if (chassisNumberSet.ContainsKey(record.ChassisNumber))
                        {
                            error.Append($"{errorRecordCount + 1}. Số khung trùng với thông tin xe ở dòng {chassisNumberSet[record.ChassisNumber]}.\n");
                            errorRecordCount++;
                        }
                        else
                        {
                            var vehicleDb = await vehicleRepository!.GetAllDataByExpression(v => v.ChassisNumber.Equals(record.ChassisNumber), 0, 0, null, false, null);
                            if (vehicleDb.Items != null && vehicleDb.Items.Count > 0)
                            {
                                error.Append($"{errorRecordCount + 1}. Số khung {record.ChassisNumber} đã tồn tại trong hệ thống.\n");
                                errorRecordCount++;
                            }
                            else
                            {
                                chassisNumberSet.Add(record.ChassisNumber, i - 1);
                            }
                        }

                    }

                    if (string.IsNullOrEmpty(record.Brand))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô tên thương hiệu trống.\n");
                        errorRecordCount++;
                    }
                    else if (!SD.CommonInformation.COMMON_BUS_BRAND_NAME_LIST.Contains(record.Brand))
                    {
                        error.Append($"{errorRecordCount + 1}. Tên thương hiệu có thể không đúng.\n");
                        errorRecordCount++;
                    }

                    if (record.Capacity < 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Dung tích là 1 số nguyên dương.\n");
                        errorRecordCount++;
                    }

                    if (record.SeatCapacity < 0)
                    {
                        error.Append($"{errorRecordCount + 1}. Số chỗ ngồi là 1 số nguyên dương.\n");
                        errorRecordCount++;
                    }
                    else if (record.SeatCapacity != 7 || record.SeatCapacity != 15 || record.SeatCapacity != 30 || record.SeatCapacity != 45)
                    {
                        error.Append($"{errorRecordCount + 1}.(Cảnh báo) Kiểm tra kĩ thông tin số chỗ ngồi.\n");
                        errorRecordCount++;
                    }

                    if (string.IsNullOrEmpty(record.Owner))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô tên chủ xe trống.\n");
                        errorRecordCount++;
                        continue;
                    }
                    else if (!Regex.IsMatch(record.Owner, SD.Regex.NAME))
                    {
                        error.Append($"{errorRecordCount + 1}. Tên chủ xe không hợp lệ.\n");
                        errorRecordCount++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(record.Color))
                    {
                        error.Append($"{errorRecordCount + 1}. Ô màu trống.\n");
                        errorRecordCount++;
                        continue;
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
        private async Task<List<VehicleRecord>> GetListFromExcel(IFormFile file)
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

                        List<VehicleRecord> records = new List<VehicleRecord>();

                        for (int row = 2; row <= rowCount; row++) // Assuming header is in the first row
                        {
                            VehicleRecord record = new VehicleRecord()
                            {
                                No = (worksheet.Cells[row, 1].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 1].Value.ToString()),
                                Plate = (worksheet.Cells[row, 2].Value == null) ? "" : worksheet.Cells[row, 2].Value.ToString(),
                                Capacity = (worksheet.Cells[row, 3].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 3].Value.ToString()),
                                SeatCapacity = (worksheet.Cells[row, 4].Value == null) ? 0 : int.Parse(worksheet.Cells[row, 4].Value.ToString()),
                                EngineNumber = (worksheet.Cells[row, 5].Value == null) ? "" : worksheet.Cells[row, 5].Value.ToString(),
                                ChassisNumber = (worksheet.Cells[row, 6].Value == null) ? "" : worksheet.Cells[row, 6].Value.ToString(),
                                Brand = (worksheet.Cells[row, 7].Value == null) ? "" : worksheet.Cells[row, 7].Value.ToString(),
                                Owner = (worksheet.Cells[row, 8].Value == null) ? "" : worksheet.Cells[row, 8].Value.ToString(),
                                Color = (worksheet.Cells[row, 9].Value == null) ? "" : worksheet.Cells[row, 9].Value.ToString(),
                                //{ "No", "Biển số", "Dung tích", "Số động cơ", "Số khung", "Thương hiệu", "Tên chủ sở hữu", "Màu" };
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

        public async Task<AppActionResult> GetAvailableVehicle(DateTime startTime, DateTime endTime, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var routeRepository = Resolve<IRepository<Route>>();
                var routeDb = await routeRepository!.GetAllDataByExpression(p => p.StartTime.Date >= startTime.Date && p.EndTime.Date <= endTime.Date ||
                                                                                p.StartTime.Date <= startTime.Date && p.EndTime.Date >= endTime.Date ||
                                                                                p.StartTime.Date <= startTime.Date && p.EndTime.Date >= endTime.Date, 0, 0, null, false, null);
                if (routeDb == null)
                {
                    result = BuildAppActionResultError(result, $"Không tìm thấy lịch trình yêu cầu với thời gian {startTime} và {endTime}");
                    return result;
                }
                else if (routeDb.Items != null && routeDb.Items.Count > 0)
                {
                    var vehicleRouteRepository = Resolve<IRepository<VehicleRoute>>();
                    List<Guid> vehicleIds = new List<Guid>();
                    foreach (var item in routeDb.Items)
                    {
                        var vehicleRouteDb = await vehicleRouteRepository!.GetAllDataByExpression(p => p.RouteId == item.Id, 0, 0, null, false, null);
                        vehicleIds.AddRange(vehicleRouteDb!.Items!.Where(v => v.VehicleId != null).Select(p => (Guid)p!.VehicleId!));
                    }
                    result.Result = await _repository.GetAllDataByExpression(p => !vehicleIds.Contains(p.Id), pageNumber, pageSize, null, false, null);
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }

        public async Task<AppActionResult> GetPriceForVehicle(FilterTransportServiceRequest filter, int pageNumber, int pageSize)
        {
            AppActionResult result = new AppActionResult();
            try
            {
                var provinceRepository = Resolve<IRepository<Province>>();
                var districtRepository = Resolve<IRepository<District>>();
                var firstProvinceDb = await provinceRepository!.GetById(filter.FirstLocation.ProvinceId);
                var secondProvinceDb = await provinceRepository!.GetById(filter!.SecondLocation!.ProvinceId);
                if (firstProvinceDb == null || secondProvinceDb == null)
                {
                    result = BuildAppActionResultError(result, $"Tỉnh với {filter.FirstLocation.ProvinceId} và {filter.SecondLocation.ProvinceId} này không tồn tại");
                    var firstDistrict = await districtRepository!.GetById(filter.FirstLocation.DistrictId!);
                    var secondDistrict = await districtRepository!.GetById(filter.SecondLocation.DistrictId!);
                    if (firstDistrict == null || secondDistrict == null)
                    {
                        result = BuildAppActionResultError(result, $"Huyện/xã với {filter.FirstLocation.DistrictId} và {filter.SecondLocation.DistrictId} này không tồn tại");
                    }
                }
                if (filter.VehicleType == Domain.Enum.VehicleType.PLANE || filter.VehicleType == Domain.Enum.VehicleType.BOAT)
                {
                    var referenceTransportPriceRepository = Resolve<IRepository<ReferenceTransportPrice>>();
                    result.Result = await referenceTransportPriceRepository!.GetAllDataByExpression(
                        p => p.Departure!.Commune!.District!.ProvinceId == filter.FirstLocation.ProvinceId && p.Arrival!.Commune!.District!.ProvinceId == filter.SecondLocation.ProvinceId
                        || p.Departure!.Commune!.DistrictId == filter.FirstLocation.DistrictId && p.Arrival!.Commune!.DistrictId == filter.SecondLocation.DistrictId,
                        pageNumber, pageSize, null, false, p => p.Departure!.Commune!.District!.Province!, p => p.Arrival!.Commune!.District!.Province!, p => p.ReferencePriceRating!
                        );
                } else 
                {
                    var facilityServiceRepository = Resolve<IRepository<Domain.Models.FacilityService>>();
                    var facilityServiceDb = await facilityServiceRepository!.GetAllDataByExpression(p => p.Facility!.Communce!.District!.ProvinceId == filter.FirstLocation.ProvinceId || p.Facility.Communce!.DistrictId == filter.FirstLocation.DistrictId && p.ServiceTypeId == ServiceType.VEHICLE
                    , 0, 0, null, false, null);
                    if (facilityServiceDb.Items != null && facilityServiceDb.Items.Count > 0)
                    {
                        var facilityServiceIds = facilityServiceDb.Items.Select(p => p.Id);
                        var sellPriceRepository = Resolve<IRepository<SellPriceHistory>>();
                        result.Result = await sellPriceRepository!.GetAllDataByExpression(p => facilityServiceIds.Contains(p.TransportServiceDetail!.FacilityServiceId), pageNumber, pageSize, p => p.Date, false, p => p.FacilityService!, p => p.TransportServiceDetail!);
                    }
                }
            }
            catch (Exception ex)
            {
                result = BuildAppActionResultError(result, ex.Message);
            }
            return result;
        }
    }
}
