using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class CustomEventStringResponse
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public double Total {  get; set; }
        public List<EventDetailPriceHistoryResponse> eventDetailPriceHistoryResponses { get; set; } = new List<EventDetailPriceHistoryResponse>();
    }

    public class EventDetailPriceHistoryResponse
    {
        public Guid EventDetailPriceHistoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
    }
}
