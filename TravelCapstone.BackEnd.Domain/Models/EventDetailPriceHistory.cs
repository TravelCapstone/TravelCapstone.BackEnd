using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class EventDetailPriceHistory
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public Guid EventDetailId { get; set; }
        [ForeignKey(nameof(EventDetailId))]
        public EventDetail? EventDetail { get; set; }
    }
}
