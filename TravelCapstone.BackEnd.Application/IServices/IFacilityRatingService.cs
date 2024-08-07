﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelCapstone.BackEnd.Common.DTO.Request;
using TravelCapstone.BackEnd.Common.DTO.Response;

namespace TravelCapstone.BackEnd.Application.IServices
{
	public interface IFacilityRatingService
	{
		public Task<AppActionResult> GetFacilityRatingByLocation(LocationRequestDto dto);
	}
}
