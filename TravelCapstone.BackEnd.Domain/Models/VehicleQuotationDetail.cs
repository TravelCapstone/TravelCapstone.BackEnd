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
    public class VehicleQuotationDetail
    {
        [Key]
        public Guid Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public int NumOfRentingDay { get; set; }
        public Guid StartPointId { get; set; }
        [ForeignKey(nameof(StartPointId))]
        public Province? StartPoint { get; set; }
        public Guid? StartPointDistrictId { get; set; }
        [ForeignKey(nameof(StartPointDistrictId))]
        public District? StartPointDistrict { get; set; }
        public Guid? EndPointId { get; set; }
        [ForeignKey(nameof(EndPointId))]
        public Province? EndPoint { get; set; }
        public Guid? EndPointDistrictId { get; set; }
        [ForeignKey(nameof(EndPointDistrictId))]
        public District? EndPointDistrict { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int NumOfVehicle { get; set; }
        public Guid OptionQuotationId { get; set; }
        [ForeignKey(nameof(OptionQuotationId))]
        public OptionQuotation? OptionQuotation { get; set; }

    }
}
