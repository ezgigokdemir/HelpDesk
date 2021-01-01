using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class AppUserExcelGeneralReportDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
       
        public int Count { get; set; }
        public List<string> filterParameter { get; set; }
        public int reportFilterType { get; set; }

        public int OrderOfUrgencyId { get; set; } //Aciliyet durumu
        public int AssigmentCount { get; set; }
        public int CompletedCount { get; set; }
        public int DissolvedCount { get; set; } //Çözüldü
      
       


    }
}
