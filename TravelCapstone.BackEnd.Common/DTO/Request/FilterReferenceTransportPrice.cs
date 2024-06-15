using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class FilterReferenceTransportPrice
    {
        public FilterLocation FirstLocation { get; set; } = null!;
        public FilterLocation SecondLocation { get; set; } = null!;
        public PortType PortType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class FilterLocation
    {
        public Guid? ProvinceId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? CommuneId { get; set; }
    }
}
