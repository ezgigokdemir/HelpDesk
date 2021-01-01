using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class CompanyDto : DataTransferObject
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int UserLimit { get; set; }
    }
}
