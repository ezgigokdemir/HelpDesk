using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface ICompanyService
    {
        void Insert(Company entity);
        void Update(CompanyDto companyDto);
        void Delete(int id);
        Company Find(int id);
        IEnumerable<Company> GetAll();
        string GetAllCompanyJson();
        int Create(CompanyDto companyDto);
        string GetCompanyByIdJson(int Id);
        string GetCompaniesJson(int? pageNumber, int? pageLength, int? searchType, string searchString);
    }
}
