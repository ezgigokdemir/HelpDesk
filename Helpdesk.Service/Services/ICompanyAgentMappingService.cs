using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface ICompanyAgentMappingService
    {
        void Create(List<Company> companies, int userId);
        void Insert(CompanyAgentMapping entity);
        void Update(CompanyAgentMapping entity);
        void Delete(int id);
        CompanyAgentMapping Find(int id);
        IEnumerable<CompanyAgentMapping> GetAll();
        IEnumerable<CompanyAgentMapping> GetAllForCompanies(List<int> companyIds);
        IEnumerable<CompanyAgentMapping> GetMappingsByUserId(int userId);
        IEnumerable<CompanyAgentMapping> GetMappingsByCompanyId(int companyId);
    }
}
