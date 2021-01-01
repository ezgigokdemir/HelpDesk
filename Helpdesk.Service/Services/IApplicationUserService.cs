using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface IApplicationUserService
    {
        int Create(ApplicationUserDto userDto);

        void Update(ApplicationUserDto userDto);

        void Delete(Guid accountId);

        ApplicationUser Find(int id);

        IEnumerable<ApplicationUser> GetAll();

        IEnumerable<ApplicationUser> GetAllUser();

        string GetAllUserJson();

        string GetUserByIdJson(int Id);

        string GetUsersJson(int? pageNumber, int? pageLength, int? searchType, string searchString);

        ApplicationUserDto GetUserDto(Guid uniqueId);

        IEnumerable<int> GetUsersInRole(string roleId1, string roleId2);
    }
}
