using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class ServicePriceRangeResponse
    {
        public double AdultMaxPrice { get; set; }
        public double AdultMinPrice { get; set; }
        public double ChildMaxPrice { get; set; }
        public double ChildMinPrice { get; set; }
    }
}
