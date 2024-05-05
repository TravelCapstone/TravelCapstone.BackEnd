using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;  
        public string Description { get; set; } = null!;
        public MaterialType MaterialTypeId { get; set; }
        [ForeignKey(nameof(MaterialTypeId))]
        public Models.EnumModels.MaterialType? MaterialType { get; set; }
    }
}
