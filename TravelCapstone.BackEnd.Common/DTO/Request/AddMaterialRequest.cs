using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class AddMaterialRequest
    {
        public Guid TourId { get; set; }
        public List<MaterialRequest> MaterialRequests { get; set; } = new List<MaterialRequest>(); 
    }

    public class MaterialRequest
    {
        public Guid MaterialId { get; set; }
        public int Quantity { get; set; }
    }
}
