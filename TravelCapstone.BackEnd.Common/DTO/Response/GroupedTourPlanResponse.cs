using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class GroupedTourPlanResponse
    {
        public Tour Tour { get; set; } = null!;
        public List<GroupedPlanServiceDetail> PlanServiceCostDetails { get; set; } = new List<GroupedPlanServiceDetail>();
        public List<DayPlansDto> DayPlans { get; set; } = new List<DayPlansDto>();
        public List<TourguideAssignment> TourguideAssignments { get; set; } = new List<TourguideAssignment>();
        public List<MaterialAssignment> MaterialAssignments { get; set; } = new List<MaterialAssignment>();
    }

    public class GroupedPlanServiceDetail
    {
        public ServiceType serviceType { get; set; }
        public List<ProvincePlanserviceDetail> ProvincePlanserviceDetails { get; set; } = new List<ProvincePlanserviceDetail>();
    }

    public class ProvincePlanserviceDetail
    {
        public Province Province { get; set; }
        public List<PlanServiceCostDetail> PlanServiceCostDetails { get; set; } = new List<PlanServiceCostDetail>();

    }
}
