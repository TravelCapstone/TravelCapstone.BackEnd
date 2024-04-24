using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models.EnumModels;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class ReferenceTransportPrice
    {
        public Guid Id { get; set; }
        public Guid DepartureId { get; set; }
        [ForeignKey(nameof(DepartureId))]
        public Port? Departure { get; set; }
        public Guid ArrivalId { get; set; }
        [ForeignKey(nameof(ArrivalId))]
        public Port? Arrival { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public double AdultPrice { get; set; }
        public double ChildPrice { get; set; }
        public Guid FacilityRatingId { get; set; }
        [ForeignKey(nameof(FacilityRatingId))]
        public FacilityRating? FacilityRating { get; set; }

        public Enum.ServiceType ServiceTypeId { get; set; }
        [ForeignKey(nameof(ServiceTypeId))]
        public ServiceType? ServiceType { get; set; }
        public string ProviderName { get; set; } = null!;
    }
}
