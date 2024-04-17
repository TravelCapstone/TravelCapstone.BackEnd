using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class OptionResponseDto
    {
        public OptionQuotation? OptionQuotation { get; set; }
        public List<QuotationDetail>? QuotationDetails { get; set; }
    }
}
