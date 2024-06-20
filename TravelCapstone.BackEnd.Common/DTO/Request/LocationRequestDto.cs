using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
	public class LocationRequestDto
	{
		public Guid? ProvinceId { get; set; }
		public Guid? DistrictId { get; set; }
		public Guid? CommuneId { get; set; }
	}
}
