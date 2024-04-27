﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelCapstone.BackEnd.Common.DTO.Request
{
    public class ServiceCostHistoryRecord
    {
        public int No {  get; set; }
        public string ServiceName { get; set; } = null;
        public string Unit { get; set; }
        public int MOQ { get; set; }
        public double Price { get; set; }

    }

    public class MenuServiceCostHistoryRecord : ServiceCostHistoryRecord
    {
        public string? MenuName { get; set; }
    }
}
