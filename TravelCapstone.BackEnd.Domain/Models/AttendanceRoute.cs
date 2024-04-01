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
    public class AttendanceRoute
    {
        [Key]
        public Guid Id { get; set; }
        public AttendanceRouteType AttendanceRouteType { get; set; }
        public Guid RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public Route? Route { get; set; }
    }
}
