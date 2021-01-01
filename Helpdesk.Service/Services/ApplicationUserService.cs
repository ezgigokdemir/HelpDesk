using AutoMapper;
using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly IMapper mapper;

        public ApplicationUserService(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.applicationUserRepository = applicationUserRepository;
            this.mapper = mapper;
        }

        public void Delete(Guid accountId)
        {
            //var entity = applicationUserRepository.Find(id);
            //if (entity != null)
            //{
            //    applicationUserRepository.Delete(entity);
            //    unitOfWork.SaveChanges();
            //}

            var user = applicationUserRepository.GetSingle(p => p.AccountId == accountId);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            user.RecordStatus = Helpdesk.Model.Enums.RecordStatus.D;
            applicationUserRepository.Update(user);
            unitOfWork.SaveChanges();
        }

        public ApplicationUser Find(int id)
        {
            return applicationUserRepository.Find(id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return applicationUserRepository.GetAll();
        }

        public int Create(ApplicationUserDto userDto)
        {
            var user = new ApplicationUser()
            {
                CreatedUser = userDto.CreatedUser,
                CreateDate = userDto.CreateDate,
                ModifyDate = userDto.ModifyDate,
                ModifiedUser = userDto.ModifiedUser,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                TrIdentityNumber = userDto.TrIdentityNumber,
                Title = userDto.Title,
                Email = userDto.Email,
                MobileNumber = userDto.MobileNumber,
                AccountId = userDto.AccountId,
                Address = new Address(),
            };

            applicationUserRepository.Insert(user);
            unitOfWork.SaveChanges();

            return user.Id;
        }

        public void Update(ApplicationUserDto userDto)
        {
            var user = applicationUserRepository.Find(userDto.Id);
            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.TrIdentityNumber = userDto.TrIdentityNumber;
            user.Title = userDto.Title;
            user.MobileNumber = userDto.MobileNumber;
            user.FixedNumber = userDto.FixedNumber;
            user.Address = userDto.Address;
            user.ModifyDate = DateTime.Now;

            applicationUserRepository.Update(user);
            unitOfWork.SaveChanges();
        }

        public string GetAllUserJson()
        {
            Expression<Func<ApplicationUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var data = applicationUserRepository.GetAllUserJson(expression, out int records);

            return data;
        }

        public string GetUsersJson(int? pageNumber, int? pageLength, int? searchType, string searchString)
        {
            Expression<Func<ApplicationUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            switch (searchType)
            {
                case 1:
                    expression = expression.And(p => p.Id == Convert.ToInt32(searchString));
                    break;
                case 2:
                    expression = expression.And(p => p.FirstName == searchString);
                    break;
                case 3:
                    expression = expression.And(p => p.LastName == searchString);
                    break;
                case 4:
                    expression = expression.And(p => p.FirstName + " " + p.LastName == searchString);
                    break;
                case 5:
                    expression = expression.And(p => p.TrIdentityNumber.Contains(searchString));
                    break;
                default:
                    break;
            }

            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            var data = applicationUserRepository.GetAllUserJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public string GetUserByIdJson(int Id)
        {
            Expression<Func<ApplicationUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.Id == Id;
            return applicationUserRepository.GetUserByConditionJson(expression);
        }

        public ApplicationUserDto GetUserDto(Guid uniqueId)
        {
            var applicationUser = applicationUserRepository.GetSingle(p => p.AccountId == uniqueId && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A);
            if (applicationUser == null)
            {
                throw new KeyNotFoundException();
            }

            return mapper.Map<ApplicationUserDto>(applicationUser);
        }

        public IEnumerable<ApplicationUser> GetAllUser()
        {
            Expression<Func<ApplicationUser, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;
            return applicationUserRepository.GetAllUser(expression);
        }

        public IEnumerable<int> GetUsersInRole(string roleId1, string roleId2)
        {
            var data = applicationUserRepository.GetUsersInrole(roleId1, roleId2);

            return data;
        }
    }
}
