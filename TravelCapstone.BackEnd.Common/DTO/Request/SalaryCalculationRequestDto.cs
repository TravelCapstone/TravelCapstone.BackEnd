using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class SalaryCalculationRequestDto
    {
        public int Quantity { get; set; }
        public int NumOfWorkingDay { get; set; }
    }
}
