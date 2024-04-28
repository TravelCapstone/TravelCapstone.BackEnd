using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
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
        public IActionResult GenerateExcelContent<T1, T2>(IEnumerable<T1> dataList, IEnumerable<T2> dataList2, List<string> header, string sheetName, List<string> header2 = null, string sheetName2 = null)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // Add the first sheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                PropertyInfo[] properties = typeof(T1).GetProperties();

                // Write header for the first sheet
                for (int i = 0; i < header.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = header[i];
                }

                int row = 2;

                // Write data for the first sheet
                foreach (T1 item in dataList)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cells[row, j + 1].Value = properties[j].GetValue(item);
                    }
                    row++;
                }

                // If the second sheet parameters are provided, add the second sheet
                if (header2 != null && sheetName2 != null && dataList2 != null)
                {
                    ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add(sheetName2);
                    PropertyInfo[] properties2 = typeof(T2).GetProperties();

                    // Write header for the second sheet
                    for (int i = 0; i < header2.Count; i++)
                    {
                        worksheet2.Cells[1, i + 1].Value = header2[i];
                    }

                    row = 2;

                    // Write data for the second sheet
                    foreach (T2 item in dataList2)
                    {
                        for (int j = 0; j < properties2.Length; j++)
                        {
                            worksheet2.Cells[row, j + 1].Value = properties2[j].GetValue(item);
                        }
                        row++;
                    }
                }

                // Get the Excel file as byte array
                var excelBytes = package.GetAsByteArray();

                // Return the Excel file
                return new FileContentResult(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "GeneratedExcel.xlsx"
                };
            }
        }

    }
}
