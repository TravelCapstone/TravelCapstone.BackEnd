using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class OptionEvent
    {
        [Key]
        public Guid Id { get; set; }
        public string CustomEvent { get; set; }
        public Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }
        public Guid OptionId { get; set; }
        [ForeignKey(nameof(OptionId))]
        public OptionQuotation? OptionQuotation { get; set; }
    }
}
