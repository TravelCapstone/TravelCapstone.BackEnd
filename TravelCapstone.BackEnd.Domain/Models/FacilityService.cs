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
    public class FacilityService
    {
        [Key]
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public int ServingQuantity { get; set; }
        public double SurchargePercent { get; set; } = 0;
        public Guid FacilityId { get; set; }
        [ForeignKey(nameof(FacilityId))]    
        public Facility? Facility { get; set; }
        public ServiceAvailability ServiceAvailabilityId { get; set; }
        [ForeignKey(nameof(ServiceAvailabilityId))]
        public Models.EnumModels.ServiceAvailability? ServiceAvailability { get; set; }
        public ServiceType ServiceTypeId { get; set; }
        [ForeignKey(nameof(ServiceTypeId))]
        public Models.EnumModels.ServiceType? ServiceType { get; set; }
        public Enum.Unit UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public Models.EnumModels.Unit? Unit { get; set; }
       
        public bool IsActive { get; set; } = true;
    }
}
