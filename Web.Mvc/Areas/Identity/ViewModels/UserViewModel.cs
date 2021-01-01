using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Mvc.Areas.Identity.ViewModels
{
    public class UserViewModel
    {
        public ApplicationUserDto UserDto { get; set; } = new ApplicationUserDto();
        public string IdentityUserId { get; set; }
        public string Password { get; set; }
        public List<Role> UserRoles { get; set; }
        public List<Company> Companies { get; set; }
        public int RecordSelection { get; set; }

        public UserViewModel()
        {
            this.UserRoles = new List<Role>();
            this.UserRoles.Add(new Role() { RoleName = "Admin", DisplayName = "Admin" });
            this.UserRoles.Add(new Role() { RoleName = "Agent", DisplayName = "Temsilci" });
        }
    }

    public class Role
    {
        public string DisplayName { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
}
