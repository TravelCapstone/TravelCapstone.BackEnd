using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class Tour
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public MainVehicleTour MainVehicle { get; set; }
        public double Price { get; set; }
        public TourType TourType { get; set; }
        public string? QRCode { get; set; }
        public TourStatus TourStatus { get; set; }
        public Guid? BasedOnTourId { get; set; }
        [ForeignKey(nameof(BasedOnTourId))]
        public Tour? BasedTour { get; set; }
    }
}
