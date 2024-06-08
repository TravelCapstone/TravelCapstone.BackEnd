using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class FacilityAndPortRequest
    {
        public Guid optionId {  get; set; }
        public List<PlanLocation> planLocations { get; set; } = new List<PlanLocation>();
    }
}
