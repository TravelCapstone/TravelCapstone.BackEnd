using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class StaticFile
    {
        [Key]
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public Guid? FacilityId { get; set; }
        [ForeignKey(nameof(FacilityId))]
        public Facility? Facility { get; set; }
        public Guid? FacilityServiceId { get; set; }
        [ForeignKey(nameof(FacilityServiceId))]
        public FacilityService? FacilityService { get; set; }
    }
}
