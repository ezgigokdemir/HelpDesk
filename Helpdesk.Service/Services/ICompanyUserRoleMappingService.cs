using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface ICompanyUserRoleMappingService
    {
        void Insert(CompanyUserRoleMapping entity);
        void Update(CompanyUserRoleMapping entity);
        void Delete(int id);
        CompanyUserRoleMapping Find(int id);
        IEnumerable<CompanyUserRoleMapping> GetAll();
    }
}
