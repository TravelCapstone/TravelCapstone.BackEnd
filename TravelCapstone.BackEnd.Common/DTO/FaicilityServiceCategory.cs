using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO
{
    public class FaicilityServiceCategory
    {
        public ServiceType ServiceTypeId { get; set; }
        public Rating RatingId { get; set; }
    }
}
