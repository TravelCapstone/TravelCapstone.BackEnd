using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class OptionListResponseForCustomer
    {
        public PrivateTourResponseDto? PrivateTourResponse { get; set; }
        public OptionResponseForCustomer Option1 { get; set; }
        public OptionResponseForCustomer Option2 { get; set; }
        public OptionResponseForCustomer Option3 { get; set; }
    }

    public class OptionResponseForCustomer
    {
        public OptionQuotation? OptionQuotation { get; set; }
        public List<QuotationDetail>? QuotationDetails { get; set; }
        public List<VehicleQuotationDetail>? VehicleQuotationDetails { get; set; }
        public List<VehicleQuotationDetail>? InnerProvinceVehicleQuotationDetails { get; set; }
    }
}
