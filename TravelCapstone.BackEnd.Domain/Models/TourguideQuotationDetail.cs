using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Domain.Models
{
    public class TourguideQuotationDetail
    {
        public Guid Id { get; set; }
        public bool IsMainTourGuide { get; set; }
        public int Quantity { get; set; }
        public int NumOfDay { get; set; }
        public double Total {  get; set; }
        public Guid? ProvinceId { get; set; }
        [ForeignKey(nameof(ProvinceId))]
        public Province? Province { get; set; }
        public Guid? OptionId { get; set; }
        [ForeignKey(nameof(OptionId))]
        public OptionQuotation? OptionQuotation { get; set; }
    }
}
