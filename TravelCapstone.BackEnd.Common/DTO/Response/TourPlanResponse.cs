using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class TourPlanResponse
    {
        public Tour Tour { get; set; } = null!;
        public List<PlanServiceCostDetail> PlanServiceCostDetails { get; set; } = new List<PlanServiceCostDetail>();
        public List<DayPlansDto> DayPlans { get; set; } = new List<DayPlansDto>();
        public List<TourguideAssignment> TourguideAssignments { get; set; } = new List<TourguideAssignment>(); 
        public List<MaterialAssignment> MaterialAssignments { get; set; } = new List<MaterialAssignment>();
    }

    public class DayPlansDto
    {
        public DayPlan DayPlan { get; set; } = null!;
        public List<VehicleRoute> VehicleRoutes { get; set; } = new List<VehicleRoute>();
    }
}
