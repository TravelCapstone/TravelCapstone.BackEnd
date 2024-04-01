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
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        public MaterialType MaterialType { get; set; }
        public int Quantity { get; set; }
        public Guid DayPlanId { get; set; }
        [ForeignKey(nameof(DayPlanId))]
        public DayPlan? DayPlan { get; set; }
    }
}
