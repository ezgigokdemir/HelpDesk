using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class CompanyUserRoleMappingDto : DataTransferObject
    {
        public int Id { get; set; }

        public string RoleId { get; set; }

        public int CompanyUserId { get; set; }
    }
}
