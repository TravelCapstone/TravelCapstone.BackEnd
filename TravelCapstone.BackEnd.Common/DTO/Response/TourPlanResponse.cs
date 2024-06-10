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
        public List<DayPlan> DayPlans { get; set; } = new List<DayPlan>();
        public List<TourguideAssignment> TourguideAssignments { get; set; } = new List<TourguideAssignment>(); 
        public List<MaterialAssignment> MaterialAssignments { get; set; } = new List<MaterialAssignment>();
        public List<Route> Routes { get; set; } = new List<Route>();
        public List<VehicleRoute> VehicleRoutes { get; set; } = new List<VehicleRoute>(); 
    }
}
