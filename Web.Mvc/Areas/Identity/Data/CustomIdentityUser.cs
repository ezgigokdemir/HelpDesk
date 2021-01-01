using Helpdesk.Model.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Mvc.Areas.Identity.Data
{
    public class CustomIdentityUser : IdentityUser
    {
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsEnabled { get; set; }
        public bool ShouldChangePasswordOnLogin { get; set; }
    }
}
