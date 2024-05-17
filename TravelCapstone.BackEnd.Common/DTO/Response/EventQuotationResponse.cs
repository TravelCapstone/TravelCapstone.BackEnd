using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class EventQuotationResponse
    {
        public Event Event { get; set; }
        public List<EventDetailReponse> EventDetailReponses { get; set; } = new List<EventDetailReponse>();
        public double Total { get; set; }
    }

    public class EventDetailReponse
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public bool PerPerson { get; set; }
        public double Price { get; set; }
        }
}
