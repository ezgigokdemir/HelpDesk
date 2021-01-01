using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class Demand : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(512)]
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
