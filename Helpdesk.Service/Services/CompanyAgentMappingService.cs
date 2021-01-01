using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class CompanyAgentMappingService : ICompanyAgentMappingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<CompanyAgentMapping> repository;
        private readonly ICompanyAgentMappingRepository mappingRepository;

        public CompanyAgentMappingService(IUnitOfWork unitOfWork, IRepository<CompanyAgentMapping> repository,
            ICompanyAgentMappingRepository mappingRepository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.mappingRepository = mappingRepository;
        }

        public void Create(List<Company> companies, int userId)
        {
            foreach (var company in companies)
            {
                var mapping = new CompanyAgentMapping()
                {
                    ApplicationUserId = userId,
                    CompanyId = company.Id,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    RecordStatus = Helpdesk.Model.Enums.RecordStatus.A
                };

                mappingRepository.Insert(mapping);
            }
            unitOfWork.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = repository.Find(id);
            if (entity == null)
            {
                throw new KeyNotFoundException();
            }

            entity.RecordStatus = Helpdesk.Model.Enums.RecordStatus.D;
            repository.Update(entity);
            unitOfWork.SaveChanges();

        }

        public CompanyAgentMapping Find(int id)
        {
            return repository.Find(id);
        }

        public IEnumerable<CompanyAgentMapping> GetAll()
        {
            Expression<Func<CompanyAgentMapping, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var data = mappingRepository.GetAllMapping(expression);
            return data;
        }

        public IEnumerable<CompanyAgentMapping> GetMappingsByCompanyId(int companyId)
        {
            Expression<Func<CompanyAgentMapping, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.CompanyId == companyId;

            var data = mappingRepository.GetAllMapping(expression);
            return data;
        }

        public IEnumerable<CompanyAgentMapping> GetAllForCompanies(List<int> companyIds)
        {
            Expression<Func<CompanyAgentMapping, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && companyIds.Contains(p.CompanyId);

            var data = mappingRepository.GetAllMapping(expression);
            return data;
        }

        public IEnumerable<CompanyAgentMapping> GetMappingsByUserId(int userId)
        {
            Expression<Func<CompanyAgentMapping, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.ApplicationUserId == userId;

            var data = mappingRepository.GetAllMapping(expression);
            return data;
        }

        public void Insert(CompanyAgentMapping entity)
        {
            repository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyAgentMapping entity)
        {
            repository.Update(entity);
            unitOfWork.SaveChanges();
        }
    }
}
