using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class OrderOfUrgencyDto : DataTransferObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
