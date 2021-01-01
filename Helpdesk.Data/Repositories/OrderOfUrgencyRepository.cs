using Helpdesk.Data.AppContext;
using Helpdesk.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public class OrderOfUrgencyRepository : Repository<OrderOfUrgency>, IOrderOfUrgencyRepository
    {
        public OrderOfUrgencyRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public string GetAllUrgencyJson(Expression<Func<OrderOfUrgency, bool>> whereCondition)
        {
            var data = GetAll(whereCondition).Select(
              p => new OrderOfUrgency()
              {
                  Id = p.Id,
                  CreateDate = p.CreateDate,
                  CreatedUser = p.CreatedUser,
                  ModifyDate = p.ModifyDate,
                  ModifiedUser = p.ModifiedUser,
                  RecordStatus = p.RecordStatus,
                  Title = p.Title
              });

            return JsonConvert.SerializeObject(data);
        }
    }
}
