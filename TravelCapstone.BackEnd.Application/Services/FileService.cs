using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Application.IServices;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.Services
{
    public class FileService : GenericBackendService, IFileService
    {

        private AppActionResult _result;

        public FileService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _result = new();
        }
        public IActionResult GenerateExcelContent<T>(IEnumerable<T> dataList, List<string> header, string sheetName)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                PropertyInfo[] properties = typeof(T).GetProperties();
                bool isRecordTemplate = true;
                for (int i = 0; i < header.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = header[i];
                }

                int row = 2;

                if (isRecordTemplate)
                {
                    for (int i = row; i <= dataList.Count() + 1; i++)
                    {
                        worksheet.Cells[i, 1].Value = i - 1;
                    }
                }

                int j = isRecordTemplate ? 1 : 0;

                foreach (T item in dataList)
                {
                    for (; j < properties.Length; j++)
                    {
                        worksheet.Cells[row, j + 1].Value = properties[j].GetValue(item);
                    }
                    row++;
                }

                var excelBytes = package.GetAsByteArray();

                return new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = sheetName + ".xlsx"
                };
            }
        }
    }
}
