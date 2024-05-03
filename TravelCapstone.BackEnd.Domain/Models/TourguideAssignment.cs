using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TourguideAssignment
    {
        [Key]
        public Guid Id { get; set; }
        public string AccountId { get; set; } = null!;
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
        public Guid ProvinceId { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        public Province? Province { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey(nameof(TourId))]
        public Tour? Tour { get; set; }
    }
}
