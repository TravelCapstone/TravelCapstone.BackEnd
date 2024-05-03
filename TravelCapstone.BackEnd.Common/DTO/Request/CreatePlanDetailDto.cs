using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class CreatePlanDetailDto
    {
        public List<PlanLocation> Locations {  get; set; } = new List<Location>();
    }
    
    public class PlanLocation
    {
        public Guid FacilityId { get; set; }
        public Guid SellPriceHistoryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int NumOfServiceUse {  get; set; }
    }

    public class EatingPlanLocation: PlanLocation
    {
        public int MealPerDay { get; set; }
    }


}
