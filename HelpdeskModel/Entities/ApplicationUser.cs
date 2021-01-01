using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TrIdentityNumber { get; set; }
        public string Title { get; set; }
        public Address Address { get; set; }
        public string MobileNumber { get; set; }
        public string FixedNumber { get; set; }
        public string Email { get; set; }
        public Guid AccountId { get; set; }
    }
}
