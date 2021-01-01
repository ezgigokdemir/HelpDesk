using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class OrderOfUrgency : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
