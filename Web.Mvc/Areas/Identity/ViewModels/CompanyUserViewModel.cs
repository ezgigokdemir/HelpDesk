using DocumentFormat.OpenXml.ExtendedProperties;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Mvc.Areas.Identity.ViewModels
{
    public class CompanyUserViewModel
    {
        public CompanyUserDto UserDto { get; set; } = new CompanyUserDto();
        public string IdentityUserId { get; set; }
        public string Password { get; set; }
        public List<Role> UserRoles { get; set; }
        public int CompanyId { get; set; }

        public CompanyUserViewModel()
        {
            this.UserRoles = new List<Role>();
            this.UserRoles.Add(new Role() { RoleName = "Company Admin", DisplayName = "Firma Admin" });
            this.UserRoles.Add(new Role() { RoleName = "Company User", DisplayName = "Firma Kullanıcısı" });
        }
    }
}
