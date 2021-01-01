using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class Company : BaseEntity
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public int UserLimit { get; set; }
    }
}
