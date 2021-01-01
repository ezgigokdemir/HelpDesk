using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface ICompanyUserRepository : IRepository<CompanyUser>
    {
        string GetAllJson(Expression<Func<CompanyUser, bool>> whereCondition, out int records, Expression<Func<CompanyUser, object>> orderCondition = null, int? skip = null, int? take = null);
    }
}
