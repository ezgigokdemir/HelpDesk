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
    public class DemandRepository : Repository<Demand>, IDemandRepository
    {
        public DemandRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<Demand> GetAllDemand(Expression<Func<Demand, bool>> expression)
        {
            var data = GetAll(expression).ToList();

            return data;
        }

        public IEnumerable<Demand> GetAllDemand(Expression<Func<Demand, bool>> whereCondition, out int records, Expression<Func<Demand, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
              p => new Demand()
              {
                  Id = p.Id,
                  Text = p.Text,
                  CreateDate = p.CreateDate,
                  CreatedUser = p.CreatedUser,
                  ModifyDate = p.ModifyDate,
                  ModifiedUser = p.ModifiedUser,
                  RecordStatus = p.RecordStatus,
                  IsAccepted = p.IsAccepted,
                  IsCompleted = p.IsCompleted,
                  IsDissolved = p.IsCompleted,
                  OrderOfUrgencyId = p.OrderOfUrgencyId,
                  CompanyUserAccountId = p.CompanyUserAccountId,
                  CompanyId = p.CompanyId
              });

            return data;
        }

        public string GetAllDemandJson(Expression<Func<Demand, bool>> whereCondition, out int records, Expression<Func<Demand, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
              p => new Demand()
              {
                  Id = p.Id,
                  Text = p.Text,
                  CreateDate = p.CreateDate,
                  CreatedUser = p.CreatedUser,
                  ModifyDate = p.ModifyDate,
                  ModifiedUser = p.ModifiedUser,
                  RecordStatus = p.RecordStatus,
                  IsAccepted = p.IsAccepted,
                  IsCompleted = p.IsCompleted,
                  IsDissolved = p.IsCompleted,
                  OrderOfUrgencyId = p.OrderOfUrgencyId,
                  CompanyUserAccountId = p.CompanyUserAccountId,
                  CompanyId = p.CompanyId
              });
            dynamic result = new ExpandoObject();
            result.records = records;
            result.data = data;
            return JsonConvert.SerializeObject(result);
        }

        public string GetDemandByCondition(Expression<Func<Demand, bool>> expression)
        {
            var data = GetAll(expression).Select(
             p => new
             {
                 Id = p.Id,
                 Text = p.Text,
                 CreateDate = p.CreateDate,
                 CreatedUser = p.CreatedUser,
                 ModifyDate = p.ModifyDate,
                 ModifiedUser = p.ModifiedUser,
                 RecordStatus = p.RecordStatus,
                 IsAccepted = p.IsAccepted,
                 IsCompleted = p.IsCompleted,
                 IsDissolved = p.IsCompleted,
                 OrderOfUrgencyId = p.OrderOfUrgencyId,
                 CompanyUserAccountId = p.CompanyUserAccountId,
                 CompanyId = p.CompanyId

             }).FirstOrDefault();

            return JsonConvert.SerializeObject(data);
        }
    }
}
