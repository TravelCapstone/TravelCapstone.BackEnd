using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
   public class VehicleRecord
    {
        public int No {  get; set; }
        public string VehicleType { get; set; }
        public string Plate { get; set; } = null!;
        public int Capacity { get; set; }
        public string EngineNumber { get; set; } = null!;
        public string ChassisNumber { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Color { get; set; } = null!;
    }
}
