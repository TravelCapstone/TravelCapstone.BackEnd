using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class PlanCostResponse
    {
        public double Total {  get; set; }
        public double PricePerAdult { get; set; }
        public double PricePerChildren { get; set; }
        public double TourguideCost { get; set; }
        public double DriverCost { get; set; }
        public double MaterialCost { get; set; }
        public double AssuranceCost { get; set; }
        public double EscortFee { get; set; }
        public double ContingencyFee { get; set; }
        public double OperatingFee { get; set; }
        public double OrganizationCost { get; set; }
        public double FacilityCost { get; set; }
        public double VehicleCost { get; set; }
        public double EventCost { get; set; }
    }
}
