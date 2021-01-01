using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        string GetAllUserJson(Expression<Func<ApplicationUser, bool>> whereCondition, out int records, Expression<Func<ApplicationUser, object>> orderCondition = null, int? skip = null, int? take = null);

        string GetUserByConditionJson(Expression<Func<ApplicationUser, bool>> expression);

        IEnumerable<ApplicationUser> GetAllUser(Expression<Func<ApplicationUser, bool>> expression);

        IEnumerable<int> GetUsersInrole(string roleId1, string roleId2);
    }
}
