using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class CompanyAgentMapping : BaseEntity
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int ApplicationUserId { get; set; }

        public bool IsSpecifiedForCompany { get; set; }
    }
}
