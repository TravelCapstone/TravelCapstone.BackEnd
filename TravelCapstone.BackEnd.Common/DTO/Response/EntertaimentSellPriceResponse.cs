using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class EntertaimentSellPriceResponse
    {
        public Guid FacilityId { get; set; }
        public string FacilityName { get; set; }
        public List<SellPriceHistory>? AdultSellPrice { get; set; } = new List<SellPriceHistory>();
        public List<SellPriceHistory>? ChildrenSellPrice { get; set; } = new List<SellPriceHistory>();
        public List<SellPriceHistory>? CommonSellPrice { get; set; } = new List<SellPriceHistory>();
    }
}
