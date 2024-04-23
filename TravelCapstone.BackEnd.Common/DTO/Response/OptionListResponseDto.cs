using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class OptionListResponseDto
    {
        public PrivateTourResponseDto? PrivateTourResponse { get; set; }
        public OptionResponseDto Option1 { get; set; }
        public OptionResponseDto Option2 { get; set; }
        public OptionResponseDto Option3 { get; set; }
    }
}
