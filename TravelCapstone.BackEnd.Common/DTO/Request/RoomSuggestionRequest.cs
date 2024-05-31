using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class RoomSuggestionRequest
    {
        public int NumOfSingleMale { get; set; }
        public int NumOfSingleFemale { get; set; }
        public List<FamilyDetail>? FamilyDetails { get; set; } = new List<FamilyDetail>();
    }
}
