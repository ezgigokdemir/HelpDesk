using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface IApplicationUserRoleMappingService
    {
        void Insert(ApplicationUserRoleMapping entity);
        void Update(ApplicationUserRoleMapping entity);
        void Delete(int id);
        ApplicationUserRoleMapping Find(int id);
        IEnumerable<ApplicationUserRoleMapping> GetAll();
    }
}
