using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Domain.Models;

namespace TravelCapstone.BackEnd.Common.DTO.Response
{
    public class MenuResponse
    {
        public Menu Menu { get; set; }
        public List<Dish> Dishes { get; set; }
    }
}
