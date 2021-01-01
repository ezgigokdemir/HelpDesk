using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class CompanyUserDto : DataTransferObject
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
        public int CompanyId { get; set; }
    }
}
