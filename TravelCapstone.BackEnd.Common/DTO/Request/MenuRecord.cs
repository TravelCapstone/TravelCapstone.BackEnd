using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class MenuRecord
    {
        public int No {  get; set; }
        public string? FacilityServiceName { get; set; }
        public string? MenuName { get; set; }
        public string? DishName { get; set; }
        public string? Description { get; set; }
        public string? MenuType { get; set; }
    }
}
