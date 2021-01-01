using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface ICompanyAgentMappingRepository : IRepository<CompanyAgentMapping>
    {
        IEnumerable<CompanyAgentMapping> GetAllMapping(Expression<Func<CompanyAgentMapping, bool>> expression);
    }
}
