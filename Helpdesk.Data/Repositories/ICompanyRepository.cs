using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        string GetAllCompanyJson(Expression<Func<Company, bool>> whereCondition, out int records, Expression<Func<Company, object>> orderCondition = null, int? skip = null, int? take = null);

        string GetCompanyById(Expression<Func<Company, bool>> expression);
    }
}
