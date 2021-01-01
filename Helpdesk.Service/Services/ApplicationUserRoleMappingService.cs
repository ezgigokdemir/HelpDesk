using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class ApplicationUserRoleMappingService : IApplicationUserRoleMappingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<ApplicationUserRoleMapping> repository;

        public ApplicationUserRoleMappingService(IUnitOfWork unitOfWork, IRepository<ApplicationUserRoleMapping> repository)
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

        public ApplicationUserRoleMapping Find(int id)
        {
            return repository.Find(id);
        }

        public IEnumerable<ApplicationUserRoleMapping> GetAll()
        {
            return repository.GetAll();
        }

        public void Insert(ApplicationUserRoleMapping entity)
        {
            repository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void Update(ApplicationUserRoleMapping entity)
        {
            repository.Update(entity);
            unitOfWork.SaveChanges();
        }
    }
}
