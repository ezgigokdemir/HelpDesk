using Helpdesk.Data.AppContext;
using Helpdesk.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public string GetAllCompanyJson(Expression<Func<Company, bool>> whereCondition, out int records, Expression<Func<Company, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
              p => new Company()
              {
                  Id = p.Id,
                  CreateDate = p.CreateDate,
                  CreatedUser = p.CreatedUser,
                  ModifyDate = p.ModifyDate,
                  ModifiedUser = p.ModifiedUser,
                  RecordStatus = p.RecordStatus,
                  CompanyName = p.CompanyName,
                  UserLimit = p.UserLimit
              });
            dynamic result = new ExpandoObject();
            result.records = records;
            result.data = data;
            return JsonConvert.SerializeObject(result);
        }

        public string GetCompanyById(Expression<Func<Company, bool>> expression)
        {
            var data = GetAll(expression).Select(
             p => new
             {
                 Id = p.Id,
                 CompanyName = p.CompanyName,
                 UserLimit = p.UserLimit

             }).FirstOrDefault();

            return JsonConvert.SerializeObject(data);
        }
    }
}
