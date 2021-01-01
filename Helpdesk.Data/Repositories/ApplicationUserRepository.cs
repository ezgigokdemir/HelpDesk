using Helpdesk.Data.AppContext;
using Helpdesk.Data.Extension;
using Helpdesk.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Data.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private IMemoryCache memoryCache;
        private readonly IConfiguration configuration;
        private readonly ApplicationContext dbContext;

        public ApplicationUserRepository(ApplicationContext dbContext, IMemoryCache memoryCache, IConfiguration configuration) : base(dbContext)
        {
            this.memoryCache = memoryCache;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        public IEnumerable<ApplicationUser> GetAllUser(Expression<Func<ApplicationUser, bool>> expression)
        {
            var data = GetAll(expression).ToList();

            return data;
        }

        public string GetAllUserJson(Expression<Func<ApplicationUser, bool>> whereCondition, out int records, Expression<Func<ApplicationUser, object>> orderCondition = null, int? skip = null, int? take = null)
        {
            var data = GetAll(whereCondition, out records, orderCondition, false, skip, take).Select(
              p => new ApplicationUser()
              {
                  Id = p.Id,
                  FirstName = p.FirstName,
                  LastName = p.LastName,
                  TrIdentityNumber = p.TrIdentityNumber,
                  MobileNumber = p.MobileNumber,
                  FixedNumber = p.FixedNumber,
                  Email = p.Email,
                  Address = new Helpdesk.Model.Entities.Address()
              });
            dynamic result = new ExpandoObject();
            result.records = records;
            result.data = data;
            return JsonConvert.SerializeObject(result);
        }

        public string GetUserByConditionJson(Expression<Func<ApplicationUser, bool>> expression)
        {
            var data = GetAll(expression).Select(
             p => new
             {
                 Id = p.Id,
                 FirstName = p.FirstName,
                 LastName = p.LastName,
                 TrIdentityNumber = p.TrIdentityNumber,
                 MobileNumber = p.MobileNumber,
                 FixedNumber = p.FixedNumber,
                 Email = p.Email,
                 Address = new Helpdesk.Model.Entities.Address()

             }).FirstOrDefault();

            return JsonConvert.SerializeObject(data);
        }

        public IEnumerable<int> GetUsersInrole(string roleId1, string roleId2)
        {
            List<string> role = new List<string> { roleId1, roleId2 };

            var query = @"select appuser.Id from AspNetUsers aspuser inner join AspNetUserRoles aspuserroles on aspuser.Id = aspuserroles.UserId
inner join  ApplicationUsers appuser on appuser.AccountId = aspuser.AccountId
where (aspuserroles.RoleId = @roleId1 or aspuserroles.RoleId = @roleId2) and appuser.RecordStatus = 'A'";

            // query = string.Format(query, role);

            AdoNetValueConverter adoNetValueConverter = new AdoNetValueConverter();

            List<int> reportData = new List<int>();

            //  string connectionString = configuration["ConnectionStrings:ApplicationContextConnection"];
            var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.Parameters.Add(new SqlParameter("@roleId1", roleId1));
                command.Parameters.Add(new SqlParameter("@roleId2", roleId2));
                connection.Open();
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportData.Add(adoNetValueConverter.GetValue<int>(reader["Id"]));
                        }
                    }
                }
            }

            return reportData.ToList();
        }
    }
}
