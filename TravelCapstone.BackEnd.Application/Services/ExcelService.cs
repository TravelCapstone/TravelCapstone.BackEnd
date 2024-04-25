using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.ConfigurationModel;
using TravelCapstone.BackEnd.Common.Utils;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class ExcelService : GenericBackendService, IExcelService
    {

        private readonly BackEndLogger _logger;

        public ExcelService(BackEndLogger logger, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = logger;
        }

        public async Task<string> CheckHeader(IFormFile file, List<string> headerTemplate)
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
    }
}
