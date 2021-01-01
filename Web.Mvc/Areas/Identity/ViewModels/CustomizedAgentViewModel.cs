using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Mvc.Areas.Identity.ViewModels
{
    public class CustomizedAgentViewModel
    {
        public int CompanyId { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
    }
}
