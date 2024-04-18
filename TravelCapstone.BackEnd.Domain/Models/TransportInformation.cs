using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TransportInformation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid StartPointId { get; set; } 
        [ForeignKey(nameof(StartPointId))]
        public Province? StartPoint { get; set; }
        public Guid EndPointId { get; set; } 
        [ForeignKey(nameof(EndPointId))]
        public Province? EndPoint { get; set; }
        public Guid QuotationDetailId { get; set; } 
        [ForeignKey(nameof(QuotationDetailId))]
        public QuotationDetail QuotationDetail { get; set; } = null!;

    }
}
