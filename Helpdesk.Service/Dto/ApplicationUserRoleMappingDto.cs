using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class ApplicationUserRoleMappingDto : DataTransferObject
    {
        public int Id { get; set; }
        public string RoleId { get; set; }

        public int ApplicationUserId { get; set; }
    }
}
