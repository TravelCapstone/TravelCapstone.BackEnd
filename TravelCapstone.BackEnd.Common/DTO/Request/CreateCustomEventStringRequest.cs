using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class CreateCustomEventStringRequest
    {
        public Guid EventId { get; set; }
        public List<EventDetailPriceHistoryRequest> eventDetailPriceHistoryRequests { get; set; } = new List<EventDetailPriceHistoryRequest>();
    }

    public class EventDetailPriceHistoryRequest
    {
        public Guid EventDetailPriceHistoryId { get; set; }
        public int Quantity { get; set; }
    }
}
