using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface IOrderOfUrgencyRepository : IRepository<OrderOfUrgency>
    {
        string GetAllUrgencyJson(Expression<Func<OrderOfUrgency, bool>> whereCondition);
    }
}
