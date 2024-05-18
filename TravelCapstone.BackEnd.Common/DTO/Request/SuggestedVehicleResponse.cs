using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class SuggestedVehicleResponse
    {
        public List<SuggestedVehicleItem> suggestedVehicleItems {  get; set; } = new List<SuggestedVehicleItem>();
        public double MinCostperPerson {  get; set; }
        public double MaxCostperPerson {  get; set; }
    }

    public class SuggestedVehicleItem{
        public VehicleType VehicleType { get; set; }
        public int Quantity { get; set; }
    }
}
