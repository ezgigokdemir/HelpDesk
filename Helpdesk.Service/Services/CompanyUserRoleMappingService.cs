using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class CompanyUserRoleMappingService : ICompanyUserRoleMappingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<CompanyUserRoleMapping> repository;

        public CompanyUserRoleMappingService(IUnitOfWork unitOfWork, IRepository<CompanyUserRoleMapping> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public void Delete(int id)
        {
            var entity = repository.Find(id);
            if (entity != null)
            {
                repository.Delete(entity);
                unitOfWork.SaveChanges();
            }
        }

        public CompanyUserRoleMapping Find(int id)
        {
            return repository.Find(id);
        }

        public IEnumerable<CompanyUserRoleMapping> GetAll()
        {
            return repository.GetAll();
        }

        public void Insert(CompanyUserRoleMapping entity)
        {
            repository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyUserRoleMapping entity)
        {
            repository.Update(entity);
            unitOfWork.SaveChanges();
        }
    }
}
