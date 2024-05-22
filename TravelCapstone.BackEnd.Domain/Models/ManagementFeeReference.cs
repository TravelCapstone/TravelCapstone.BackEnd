using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class ManagementFeeReference
    {
        public Guid Id { get; set; }
        public int Moq {  get; set; }
        public double MinFee { get; set; }
        public double MaxFee { get; set; }
        public ManagementFeeType ManagementFeeTypeId { get; set; }
        [ForeignKey(nameof(ManagementFeeTypeId))]
        public Models.EnumModels.ManagementFeeType? ManagementFeeType { get; set; }
    }
}
