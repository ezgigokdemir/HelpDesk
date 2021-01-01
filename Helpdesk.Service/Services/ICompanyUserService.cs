using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface ICompanyUserService
    {
        int Create(CompanyUserDto companyUserDto);
        void Insert(CompanyUser entity);
        void Update(CompanyUserDto entity);
        void Delete(int id);
        CompanyUser Find(int id);
        IEnumerable<CompanyUser> GetAll();
        CompanyUserDto GetCompanyUserbyUserAccountId(Guid currentAccountId);
        string GetAllCompanyIdbyUser(Guid currentAccountId);
        int GetOfCountCompanyUser(string accId);
        int GetOfCountCompanyUser(int CompanyId);
        string GetAllCompanyUserList(int CompanyId);
        string GetUsersJson(int? pageNumber, int? pageLength, int? companyId, string filterType, string keyword);
        string GetUserByIdJson(int Id);
        void DeleteCompanyUser(int id, string username);
    }
}
