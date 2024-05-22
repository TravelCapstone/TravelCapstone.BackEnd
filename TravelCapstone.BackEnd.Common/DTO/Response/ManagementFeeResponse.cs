using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class ManagementFeeResponse
    {
        public double MinOrganizationCost { get; set; }
        public double MaxOrganizationCost { get; set; }
        public double MinContingencyFee { get; set; }
        public double MaxContingencyFee { get; set; }
        public double MinEscortFee { get; set; }
        public double MaxEscortFee { get; set; }
        public double MinOperatingFee { get; set; }
        public double MaxOperatingFee { get; set; }
    }
}
