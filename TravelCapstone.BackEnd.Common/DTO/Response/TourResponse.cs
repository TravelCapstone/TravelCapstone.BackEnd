using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class TourResponse
    {
        public Tour Tour { get; set; } = null!;
        public List<StaticFile> StaticFiles = null!;
    }
}
