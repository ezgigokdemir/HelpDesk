using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface IDemandRepository : IRepository<Demand>
    {
        IEnumerable<Demand> GetAllDemand(Expression<Func<Demand, bool>> expression);
        string GetAllDemandJson(Expression<Func<Demand, bool>> whereCondition, out int records, Expression<Func<Demand, object>> orderCondition = null, int? skip = null, int? take = null);

        IEnumerable<Demand> GetAllDemand(Expression<Func<Demand, bool>> whereCondition, out int records, Expression<Func<Demand, object>> orderCondition = null, int? skip = null, int? take = null);

        string GetDemandByCondition(Expression<Func<Demand, bool>> expression);
    }
}
