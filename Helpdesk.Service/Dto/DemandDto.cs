using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class DemandDto : DataTransferObject
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int OrderOfUrgencyId { get; set; } //Aciliyet durumu
        public bool IsAccepted { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDissolved { get; set; } //Çözüldü
        public int CompanyId { get; set; }
        public Guid CompanyUserAccountId { get; set; }
        public Guid? ApplicationUserAccountId { get; set; }
    }
}
