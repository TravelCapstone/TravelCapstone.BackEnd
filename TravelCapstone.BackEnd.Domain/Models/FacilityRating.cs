using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class FacilityRating
    {
        [Key]
        public Guid Id { get; set; }
        public Enum.FacilityType FacilityTypeId { get; set; }
        [ForeignKey(nameof(FacilityTypeId))]
        public EnumModels.FacilityType? FacilityType { get; set; }

        public Enum.Rating RatingId { get; set; }
        [ForeignKey(nameof(RatingId))]
        public EnumModels.Rating? Rating { get; set; }
    }
}
