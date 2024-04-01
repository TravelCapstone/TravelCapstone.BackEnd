using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Plate { get; set; } = null!;
        public int Capacity { get; set; }
        public string EngineNumber { get; set; } = null!;
        public string ChassisNumber { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Color { get; set; } = null!;

    }
}
