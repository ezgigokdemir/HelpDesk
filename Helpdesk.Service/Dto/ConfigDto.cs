using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
   public class ConfigDto
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int reportFilterType { get; set; }
        public string taskStatusfilterKey { get; set; }
    }
}
