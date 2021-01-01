using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
   public class GeneralExcelDto
    {
        public ConfigDto config { get; set; }
       
        public int reportFilterType { get; set; }
        public string taskStatusfilterKey { get; set; }
    }
}
