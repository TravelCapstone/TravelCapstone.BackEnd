using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Application.IServices
{
    public interface IFileService
    {
        public IActionResult GenerateExcelContent<T>(IEnumerable<T> dataList, List<string> header, string sheetName);
    }
}
