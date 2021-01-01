using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string AddressLine { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }
}
