using Helpdesk.Data.AppContext;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public class CompanyAgentMappingRepository : Repository<CompanyAgentMapping>, ICompanyAgentMappingRepository
    {
        public CompanyAgentMappingRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<CompanyAgentMapping> GetAllMapping(Expression<Func<CompanyAgentMapping, bool>> expression)
        {
            var data = GetAll(expression).ToList();

            return data;
        }
    }
}
