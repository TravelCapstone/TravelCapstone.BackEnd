using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class FlightInformation
    {
        [Key]
        public Guid Id { get; set; }
        public string StartPoint { get; set; } = null!;
        public string EndPoint { get; set; } = null!;
        public Guid QuotationDetailId { get; set; } 
        [ForeignKey(nameof(QuotationDetailId))]
        public QuotationDetail QuotationDetail { get; set; } = null!;

    }
}
