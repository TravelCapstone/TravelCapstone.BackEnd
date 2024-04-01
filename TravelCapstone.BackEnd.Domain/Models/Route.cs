using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Route
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public Guid DayPlanId { get; set; }
        [ForeignKey(nameof(DayPlanId))]
        public DayPlan? DayPlan { get; set; } 
        public Guid? StartPointId { get; set; }
        [ForeignKey(nameof(StartPointId))]
        public Destination? StartPoint { get; set; }
        public Guid EndPointId { get; set; }
        [ForeignKey(nameof(EndPointId))]
        public Destination? EndPoint { get; set; }
        public Guid? ParentRouteId { get; set; }
        [ForeignKey(nameof(ParentRouteId))]
        public Route? ParentRoute { get; set; }


    }
}
