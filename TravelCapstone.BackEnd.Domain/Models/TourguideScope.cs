using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TourguideScope
    {
        [Key]
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
        public Guid DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public District? District { get; set; }
    }
}
