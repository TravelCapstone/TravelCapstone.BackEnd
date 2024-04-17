using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class EstimatedVehiclePrice
    {
        public VehicleType Vehicle {  get; set; }  
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
