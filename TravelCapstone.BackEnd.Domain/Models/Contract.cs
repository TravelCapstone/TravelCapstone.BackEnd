using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.BaseModel;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Contract:BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public int NumOfChild { get; set; }
        public int NumOfAdult {  get; set; }
        public double Total { get; set; }
        public double Deposit { get; set; }
        public string? Content { get; set; }
        public string? ContractUrl { get; set; }
        public DateTime DateOfContract { get; set; }
        public ContractStatus ContractStatus { get; set; }

        public Guid? TourId { get; set; }

        [ForeignKey(nameof(TourId))]
        public Tour? Project { get; set; }

        public string? CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Account? Customer { get; set; }
    }
}
