using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.Data.Repositories;
using Helpdesk.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Mvc.Areas.Identity.Data;
using Web.Mvc.Areas.Identity.ViewModels;

namespace Web.Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class UserController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> signInManager;
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IApplicationUserService applicationUserService;
        private readonly ICompanyService companyService;
        private readonly ICompanyAgentMappingService companyAgentMappingService;
        private readonly IDemandService demandService;

        public UserController(SignInManager<CustomIdentityUser> signInManager,
            UserManager<CustomIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IApplicationUserService applicationUserService,
            ICompanyService companyService,
            ICompanyAgentMappingService companyAgentMappingService,
            IDemandService demandService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            this.applicationUserService = applicationUserService;
            this.companyService = companyService;
            this.companyAgentMappingService = companyAgentMappingService;
            this.demandService = demandService;
        }

        public IActionResult Index()
        {
            var applicationUsers = applicationUserService.GetAllUserJson();
            ViewBag.Users = applicationUsers;
            return View();
        }

        public ContentResult GetAll(int? pageNumber, int? pageLength, int? searchType, string searchString)
        {
            return Content(applicationUserService.GetUsersJson(pageNumber, pageLength, searchType, searchString), "application/json");
        }

        public IActionResult Create()
        {
            var companies = companyService.GetAllCompanyJson();
            ViewBag.Companies = companies;
            var userViewModel = new UserViewModel();
            ViewBag.Model = JsonConvert.SerializeObject(userViewModel);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new CustomIdentityUser
                    {
                        FirstName = userViewModel.UserDto.FirstName,
                        LastName = userViewModel.UserDto.LastName,
                        UserName = userViewModel.UserDto.Email,
                        Email = userViewModel.UserDto.Email,
                        AccountId = Guid.NewGuid(),
                        IsEnabled = true
                    };
                    var result = await userManager.CreateAsync(user, userViewModel.Password).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        var selectedRoles = userViewModel.UserRoles.Where(p => p.Selected).Select(p => p.RoleName).ToList();
                        await userManager.AddToRolesAsync(user, selectedRoles);
                        await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FullName", string.Format("{0} {1}", userViewModel.UserDto.FirstName, userViewModel.UserDto.LastName)));
                        await userManager.AddClaimAsync(user, new System.Security.Claims.Claim("AccountId", user.AccountId.ToString()));

                        var currentUser = await userManager.GetUserAsync(HttpContext.User);

                        var userDto = new Helpdesk.Service.Dto.ApplicationUserDto()
                        {
                            CreatedUser = currentUser.FirstName + " " + currentUser.LastName,
                            ModifiedUser = currentUser.FirstName + " " + currentUser.LastName,
                            CreateDate = DateTime.Now,
                            ModifyDate = DateTime.Now,
                            FirstName = userViewModel.UserDto.FirstName,
                            LastName = userViewModel.UserDto.LastName,
                            TrIdentityNumber = userViewModel.UserDto.TrIdentityNumber,
                            Title = userViewModel.UserDto.Title,
                            Email = userViewModel.UserDto.Email,
                            MobileNumber = userViewModel.UserDto.MobileNumber,
                            AccountId = user.AccountId,
                            Address = new Helpdesk.Model.Entities.Address(),
                            RecordStatus = Helpdesk.Model.Enums.RecordStatus.A
                        };

                        var userId = applicationUserService.Create(userDto);

                        if (userViewModel.Companies == null)
                        {
                            return Ok();
                        }
                        if (userViewModel.Companies.Count > 0)
                        {
                            companyAgentMappingService.Create(userViewModel.Companies, userId);
                        }

                        return Ok();
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return BadRequest(ModelState);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Detail(int id)
        {
            var user = applicationUserService.GetUserByIdJson(id);
            ViewBag.User = user;

            return View();
        }

        public IActionResult Edit(int id)
        {
            var companies = companyService.GetAllCompanyJson();
            ViewBag.Companies = companies;

            var userAccountId = applicationUserService.GetAll().Where(user => user.Id == id && user.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A).Select(user => user.AccountId).FirstOrDefault();

            var userIdentityId = userManager.Users.Where(user => user.IsEnabled).Where(user => user.AccountId == userAccountId).Select(user => user.Id).FirstOrDefault();

            var account = userManager.FindByIdAsync(userIdentityId).Result;

            var roles = userManager.GetRolesAsync(account).Result;
            var applicationUser = applicationUserService.GetUserDto(account.AccountId);

            var userViewModel = new UserViewModel();
            userViewModel.UserDto = applicationUser;
            userViewModel.IdentityUserId = account.Id;
            foreach (var role in roles)
            {
                var userRole = userViewModel.UserRoles.FirstOrDefault(p => p.RoleName == role);
                if (userRole != null)
                {
                    userRole.Selected = true;
                }
            }

            ViewBag.UserViewModel = JsonConvert.SerializeObject(userViewModel);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]UserViewModel userViewModel)
        {
            try
            {
                var account = userManager.FindByIdAsync(userViewModel.IdentityUserId).Result;
                var roles = userManager.GetRolesAsync(account).Result;
                account.FirstName = userViewModel.UserDto.FirstName;
                account.LastName = userViewModel.UserDto.LastName;
                account.Email = userViewModel.UserDto.Email;
                account.PhoneNumber = userViewModel.UserDto.MobileNumber;
                account.Id = userViewModel.IdentityUserId;

                await userManager.UpdateAsync(account);

                var deleteRoles = userManager.RemoveFromRolesAsync(account, roles).Result;

                await userManager.AddToRolesAsync(account, userViewModel.UserRoles.Where(p => p.Selected).Select(p => p.RoleName).ToList());

                userViewModel.UserDto.ModifiedUser = User.FindFirst("FullName").Value;

                if (userViewModel.Companies != null)
                {
                    var mapping = companyAgentMappingService.GetAll().Where(p => p.ApplicationUserId == userViewModel.UserDto.Id && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A);

                    //Eski maplerde olmayıp yeni seçtiği company'ler
                    var conflictedCompanies = userViewModel.Companies.Where(p => !mapping.Select(q => q.CompanyId).Contains(p.Id)).ToList();

                    //Eski maplerde olan fakat şimdi seçmediği company'ler
                    var nonConflictedMapping = mapping.Where(p => !userViewModel.Companies.Select(q => q.Id).Contains(p.CompanyId));

                    if (userViewModel.RecordSelection != 1)
                    {
                        foreach (var item in nonConflictedMapping)
                        {
                            companyAgentMappingService.Delete(item.Id);
                        }
                    }

                    if (conflictedCompanies.Count > 0)
                    {
                        companyAgentMappingService.Create(conflictedCompanies, userViewModel.UserDto.Id);
                    }
                }

                applicationUserService.Update(userViewModel.UserDto);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IActionResult Delete(int id)
        {
            var userAccountId = applicationUserService.GetAll().Where(user => user.Id == id && user.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A).Select(user => user.AccountId).FirstOrDefault();

            var userIdentityId = userManager.Users.Where(user => user.IsEnabled).Where(user => user.AccountId == userAccountId).Select(user => user.Id).FirstOrDefault();

            var account = userManager.FindByIdAsync(userIdentityId).Result;

            var roles = userManager.GetRolesAsync(account).Result;
            var applicationUser = applicationUserService.GetUserDto(account.AccountId);

            var userViewModel = new UserViewModel();
            userViewModel.UserDto = applicationUser;
            userViewModel.IdentityUserId = account.Id;
            foreach (var role in roles)
            {
                var userRole = userViewModel.UserRoles.FirstOrDefault(p => p.RoleName == role);
                if (userRole != null)
                {
                    userRole.Selected = true;
                }
            }
            ViewBag.UserViewModel = JsonConvert.SerializeObject(userViewModel);
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmedAsync(int id)
        {
            var userAccountId = applicationUserService.GetAll().Where(p => p.Id == id && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A).Select(p => p.AccountId).FirstOrDefault();

            var userIdentityId = userManager.Users.Where(p => p.IsEnabled).Where(p => p.AccountId == userAccountId).Select(p => p.Id).FirstOrDefault();

            var user = userManager.FindByIdAsync(userIdentityId).Result;

            if (user == null)
            {
                return new NotFoundResult();
            }

            var demands = demandService.GetAllDemand().Where(p => p.ApplicationUserAccountId == user.AccountId && ((p.IsAccepted && p.IsDissolved && p.IsCompleted != true)||(p.IsAccepted && p.IsDissolved != true && p.IsCompleted != true))).ToList();

            if (demands.Count > 0)
            {
                return UnprocessableEntity();
            }

            var userMapping = companyAgentMappingService.GetMappingsByUserId(id);

            user.IsEnabled = false;
            var result = userManager.UpdateAsync(user);            
            applicationUserService.Delete(user.AccountId);

            foreach (var map in userMapping)
            {
                companyAgentMappingService.Delete(map.Id);
            }

            foreach (var demand in demands)
            {
                demandService.UpdateForDeletedUser(demand);
            }

            return Ok();
        }
    }
}