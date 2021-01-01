using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;



namespace Helpdesk.Service.Services
{
    public class CompanyUserService : ICompanyUserService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICompanyUserRepository companyUserRepository;

        public CompanyUserService(IUnitOfWork unitOfWork,
            ICompanyUserRepository companyUserRepository)
        {
            this.unitOfWork = unitOfWork;
            this.companyUserRepository = companyUserRepository;
        }

        public int Create(CompanyUserDto companyUserDto)
        {
            var companyuser = new CompanyUser()
            {
                CreatedUser = companyUserDto.CreatedUser,
                CreateDate = companyUserDto.CreateDate,
                ModifyDate = companyUserDto.ModifyDate,
                ModifiedUser = companyUserDto.ModifiedUser,
                FirstName = companyUserDto.FirstName,
                LastName = companyUserDto.LastName,
                TrIdentityNumber = companyUserDto.TrIdentityNumber,
                Title = companyUserDto.Title,
                Email = companyUserDto.Email,
                MobileNumber = companyUserDto.MobileNumber,
                CompanyId = companyUserDto.CompanyId,
                AccountId = companyUserDto.AccountId,
                Address = new Address()
            };

            companyUserRepository.Insert(companyuser);
            unitOfWork.SaveChanges();

            return companyuser.Id;

        }

        public void Delete(int id)
        {
            var entity = companyUserRepository.Find(id);
            if (entity != null)
            {
                companyUserRepository.Delete(entity);
                unitOfWork.SaveChanges();
            }
        }

        public CompanyUser Find(int id)
        {
            return companyUserRepository.Find(id);
        }

        public IEnumerable<CompanyUser> GetAll()
        {
            return companyUserRepository.GetAll();
        }

        public CompanyUserDto GetCompanyUserbyUserAccountId(Guid accountId)
        {
            var currendUser = companyUserRepository.GetAll().Where(p => p.AccountId == accountId && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A)
                .Select(p => new CompanyUserDto()
                {
                    Id = p.Id,
                    CompanyId = p.CompanyId,
                    AccountId = p.AccountId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    TrIdentityNumber = p.TrIdentityNumber,
                    MobileNumber = p.MobileNumber,
                    Email = p.Email
                }).FirstOrDefault();

            return currendUser;
        }

        public string GetAllCompanyIdbyUser(Guid accountId)
        {
            Expression<Func<CompanyUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.AccountId == accountId;

            var data = companyUserRepository.GetAllJson(expression, out int records);

            return data;
        }

        public void Insert(CompanyUser entity)
        {
            companyUserRepository.Insert(entity);
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyUser entity)
        {
            companyUserRepository.Update(entity);
            unitOfWork.SaveChanges();
        }

        public int GetOfCountCompanyUser(string accId)
        {
            var currendUser = GetCompanyUserbyUserAccountId(Guid.Parse(accId));

            var userCount = companyUserRepository.GetAll().Where(p => p.CompanyId == currendUser.CompanyId && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A)
                         .Count();

            return userCount;
        }

        public int GetOfCountCompanyUser(int CompanyId)
        {
            var userCount = companyUserRepository.GetAll().Where(p => p.CompanyId == CompanyId && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A)
                         .Count();

            return userCount;
        }

        public string GetAllCompanyUserList(int CompanyId)
        {
            Expression<Func<CompanyUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.CompanyId == CompanyId;

            var data = companyUserRepository.GetAllJson(expression, out int records);

            return data;
        }

        public string GetUsersJson(int? pageNumber, int? pageLength, int? companyId, string filterType, string keyword)
        {
            Expression<Func<CompanyUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;


            if (companyId != null)
            {
                expression = expression.And(p => p.CompanyId == companyId);
            }

            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                switch (filterType)
                {
                    case "companyname":
                        expression = expression.And(p => p.Company.CompanyName.Contains(keyword));
                        break;

                    case "tridentity":
                        expression = expression.And(p => p.TrIdentityNumber.Contains(keyword));
                        break;


                    case "fullname":
                        expression = expression.And(p => (p.FirstName + " " + p.LastName).IndexOf(keyword) >= 0);
                        break;

                    case "id":
                        expression = expression.And(p => p.Id.ToString() == keyword);
                        break;

                    default:
                        break;
                }
            }

            var data = companyUserRepository.GetAllJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public string GetUserByIdJson(int Id)
        {
            Expression<Func<CompanyUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.Id == Id;
            return companyUserRepository.GetAllJson(expression, out int records);
        }

        public void Update(CompanyUserDto entity)
        {
            var user = companyUserRepository.Find(entity.Id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            user.FirstName = entity.FirstName;
            user.LastName = entity.LastName;
            user.TrIdentityNumber = entity.TrIdentityNumber;
            user.Title = entity.Title;
            user.MobileNumber = entity.MobileNumber;
            user.FixedNumber = entity.FixedNumber;
            user.Address = entity.Address;
            user.ModifiedUser = entity.ModifiedUser;
            user.ModifyDate = DateTime.Now;

            companyUserRepository.Update(user);
            unitOfWork.SaveChanges();
        }

        public void DeleteCompanyUser(int id, string username)
        {
            var user = companyUserRepository.Find(id);
            if (user == null)
            {
                throw new KeyNotFoundException(id.ToString());
            }

            user.ModifiedUser = username;
            user.ModifyDate = DateTime.Now;
            user.RecordStatus = Helpdesk.Model.Enums.RecordStatus.D;
            companyUserRepository.Update(user);
            unitOfWork.SaveChanges();
        }
    }
}
