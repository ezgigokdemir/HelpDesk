using Helpdesk.Data.AppContext;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;

namespace Helpdesk.Data.Repositories
{
    public class CompanyUserRepository : Repository<CompanyUser>, ICompanyUserRepository
    {
        public CompanyUserRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public string GetAllJson(Expression<Func<CompanyUser, bool>> whereCondition, out int records, Expression<Func<CompanyUser, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
             p => new CompanyUser()
             {
                 Id = p.Id,
                 FirstName = p.FirstName,
                 LastName = p.LastName,
                 TrIdentityNumber = p.TrIdentityNumber,
                 MobileNumber = p.MobileNumber,
                 FixedNumber = p.FixedNumber,
                 Email = p.Email,
                 CompanyId = p.CompanyId,
                 AccountId = p.AccountId,

                 Address = new Helpdesk.Model.Entities.Address()
                 {
                     Province = p.Address.Province
                 },
                 Company = new Company()
                 {
                     Id = p.Company.Id,
                     CompanyName = p.Company.CompanyName,
                     UserLimit = p.Company.UserLimit

                 }

             });
            dynamic result = new ExpandoObject();
            result.records = records;
            result.data = data;
            return JsonConvert.SerializeObject(result);
        }

        public int GetOfCountCompanyUser(Expression<Func<CompanyUser, bool>> whereCondition, out int records, Expression<Func<CompanyUser, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
              p => new CompanyUser()
              {
                  Id = p.Id,
                  CompanyId = p.CompanyId,
                  AccountId = p.AccountId,
                  Company = new Company()
                  {
                      Id = p.Company.Id,
                      CompanyName = p.Company.CompanyName,
                      UserLimit = p.Company.UserLimit

                  }

              });
            dynamic result = new ExpandoObject();
            result.records = records;
            result.data = data;
            return JsonConvert.SerializeObject(result);
        }
    }
}
