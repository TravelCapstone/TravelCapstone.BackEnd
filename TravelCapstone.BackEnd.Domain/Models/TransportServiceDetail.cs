using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TransportServiceDetail
    {
        [Key]
        public Guid Id { get; set; }
        public int ServingQuantity { get; set; }
        public VehicleType VehicleTypeId { get; set; }
        [ForeignKey(nameof(VehicleTypeId))]
        public EnumModels.VehicleType? VehicleType { get; set; }
        public Guid FacilityServiceId { get; set; }
        [ForeignKey(nameof(FacilityServiceId))] public FacilityService? FacilityService { get; set; }

    }
}
