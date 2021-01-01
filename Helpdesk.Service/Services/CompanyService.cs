using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICompanyRepository companyRepository;

        public CompanyService(IUnitOfWork unitOfWork, ICompanyRepository companyRepository)
        {
            this.unitOfWork = unitOfWork;
            this.companyRepository = companyRepository;
        }

        public int Create(CompanyDto companyDto)
        {
            var company = new Company()
            {
                CreatedUser = companyDto.CreatedUser,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                ModifiedUser = companyDto.ModifiedUser,
                CompanyName = companyDto.CompanyName,
                UserLimit = companyDto.UserLimit
            };

            companyRepository.Insert(company);
            unitOfWork.SaveChanges();

            return company.Id;
        }

        public void Delete(int id)
        {
            var entity = companyRepository.Find(id);
            if (entity != null)
            {
                companyRepository.Delete(entity);
                unitOfWork.SaveChanges();
            }
        }

        public Company Find(int id)
        {
            return companyRepository.Find(id);
        }

        public IEnumerable<Company> GetAll()
        {
            return companyRepository.GetAll();
        }

        public string GetAllCompanyJson()
        {
            Expression<Func<Company, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var data = companyRepository.GetAllCompanyJson(expression, out int records);

            return data;
        }

        public string GetCompaniesJson(int? pageNumber, int? pageLength, int? searchType, string searchString)
        {
            Expression<Func<Company, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;
            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            switch (searchType)
            {
                case 1:
                    expression = expression.And(p => p.Id == Convert.ToInt32(searchString));
                    break;
                case 2:
                    expression = expression.And(p => p.CompanyName == searchString);
                    break;
                case 3:
                    expression = expression.And(p => p.UserLimit == Convert.ToInt32(searchString));
                    break;
                default:
                    break;
            }

            var data = companyRepository.GetAllCompanyJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public string GetCompanyByIdJson(int Id)
        {
            Expression<Func<Company, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.Id == Id;
            return companyRepository.GetCompanyById(expression);
        }

        public void Insert(Company entity)
        {
            companyRepository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyDto companyDto)
        {
            var company = companyRepository.Find(companyDto.Id);
            if (company == null)
            {
                throw new KeyNotFoundException();
            }

            company.CompanyName = companyDto.CompanyName;
            company.UserLimit = companyDto.UserLimit;
            company.ModifiedUser = companyDto.ModifiedUser;
            company.ModifyDate = DateTime.Now;

            companyRepository.Update(company);
            unitOfWork.SaveChanges();
        }
    }
}
