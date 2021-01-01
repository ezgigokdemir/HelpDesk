using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Helpdesk.Model.Entities
{
    public class CompanyUserRoleMapping : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string RoleId { get; set; }

        public int CompanyUserId { get; set; }
    }
}
