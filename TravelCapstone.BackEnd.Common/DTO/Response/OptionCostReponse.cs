using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Enum;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class OptionCostReponse
    {
        public OptionClass OptionClass { get; set; }
        public double MinTourguideCost { get; set; }
        public double MaxTourguideCost { get; set; }
        public double MinDriverCost { get; set; }
        public double MaxDriverCost { get; set; }
        public double MaterialCost { get; set; }
        public double AssuranceCost { get; set; }
        public double EscortFee { get; set; }
        public double ContingencyFee { get; set; }
        public double OperatingFee { get; set; }
        public double OrganizationCost { get; set; }
        public double MinHotelCost { get; set; }
        public double MaxHotelCost { get; set; }
        public double MinRestaurantCost { get; set; }
        public double MaxRestaurantCost { get; set; }
        public double MinEntertainmentCost { get; set; }
        public double MaxEntertainmentCost { get; set; }
        public double MinVehicleCost { get; set; }
        public double MaxVehicleCost { get; set; }
        public double MinEventCost { get; set; }
        public double MaxEventCost { get; set; }
    }
}
