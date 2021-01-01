using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.Data.Repositories;
using Helpdesk.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Mvc.Areas.Identity.Data;
using Newtonsoft.Json;
using Web.Mvc.Areas.Identity.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Helpdesk.Service.Dto;
using Helpdesk.Model.Entities;

namespace Web.Mvc.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class CompanyUserController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> signInManager;
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICompanyUserService companyUserService;
        private readonly ICompanyUserRoleMappingService companyUserRoleMappingService;
        private readonly ICompanyService companyService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IApplicationUserService applicationUserService;
        private readonly ICompanyAgentMappingService companyAgentMappingService;

        public CompanyUserController(SignInManager<CustomIdentityUser> signInManager,
            UserManager<CustomIdentityUser> userManager,
            IUnitOfWork unitOfWork,
            ICompanyUserService companyUserService,
            ICompanyService companyService,
            IHttpContextAccessor httpContextAccessor,
            ICompanyUserRoleMappingService companyUserRoleMappingService,
            IApplicationUserService applicationUserService,
            ICompanyAgentMappingService companyAgentMappingService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.companyUserService = companyUserService;
            this.companyService = companyService;
            this.httpContextAccessor = httpContextAccessor;
            this.companyUserRoleMappingService = companyUserRoleMappingService;
            this.applicationUserService = applicationUserService;
            this.companyAgentMappingService = companyAgentMappingService;
        }

        public IActionResult Index()
        {
            // var Users = companyUserService.GetAll();
            // var Users = companyUserService.GetAllCompanyUserList(currentUser.CompanyId);

            //  ViewBag.Users = JsonConvert.SerializeObject(Users);
            // ViewBag.CompanyId = JsonConvert.SerializeObject(currentUser.CompanyId);
            return View();
        }

        public IActionResult Detail(int id)
        {
            var user = companyUserService.GetUserByIdJson(id);
            ViewBag.User = user;

            return View();
        }

        public ContentResult GetAll(int? pageNumber, int? pageLength, string filterType, string keyword)
        {
            var user = httpContextAccessor.HttpContext.User;
            IEnumerable<Claim> claims = user.Claims;
            var AccountId = claims.Where(x => x.Type == "AccountId").Select(c => c.Value).SingleOrDefault();

            var currentUser = companyUserService.GetCompanyUserbyUserAccountId(Guid.Parse(AccountId));

            if (currentUser != null)
            {
                int companyId = currentUser.CompanyId;
                return Content(companyUserService.GetUsersJson(pageNumber, pageLength, companyId, filterType, keyword), "application/json");
            }
            return Content(companyUserService.GetUsersJson(pageNumber, pageLength, null, filterType, keyword), "application/json");
        }

        public IActionResult Create()
        {
            var companies = companyService.GetAllCompanyJson();
            ViewBag.Companies = companies;
            var companyUserViewModel = new CompanyUserViewModel();
            ViewBag.Model = JsonConvert.SerializeObject(companyUserViewModel);

            var user = httpContextAccessor.HttpContext.User;
            IEnumerable<Claim> claims = user.Claims;
            var accId = claims.Where(x => x.Type == "AccountId").Select(c => c.Value).SingleOrDefault();
            var currentRoles = claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            ViewBag.CurrentRoles = JsonConvert.SerializeObject(currentRoles);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CompanyUserViewModel companyUserViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var companyUserModel = new CompanyUserDto();
                    var companyObje = new Company();
                    var currentUser = await userManager.GetUserAsync(HttpContext.User);
                    if (companyUserViewModel.CompanyId > 0) // Yönetici olarak giriş yapıldıysa CompanyId bulunmaz
                    {
                        companyObje = companyService.Find(companyUserViewModel.CompanyId);// limit bulunacak
                    }
                    else //firma Admini giriş yaptıysa
                    {
                        companyUserModel = companyUserService.GetCompanyUserbyUserAccountId(currentUser.AccountId);// limit bulunacak

                        companyObje = companyService.Find(companyUserModel.CompanyId);
                    }

                    int userAddedCount = companyUserService.GetOfCountCompanyUser(companyObje.Id);
                    if (companyObje.UserLimit > userAddedCount)
                    {
                        var companyUser = new CustomIdentityUser
                        {
                            FirstName = companyUserViewModel.UserDto.FirstName,
                            LastName = companyUserViewModel.UserDto.LastName,
                            UserName = companyUserViewModel.UserDto.Email,
                            Email = companyUserViewModel.UserDto.Email,
                            AccountId = Guid.NewGuid(),
                            IsEnabled = true
                        };
                        var result = await userManager.CreateAsync(companyUser, companyUserViewModel.Password).ConfigureAwait(false);
                        if (result.Succeeded)
                        {
                            var selectedRoles = companyUserViewModel.UserRoles.Where(p => p.Selected).Select(p => p.RoleName).ToList();
                            await userManager.AddToRolesAsync(companyUser, selectedRoles);
                            await userManager.AddClaimAsync(companyUser, new System.Security.Claims.Claim("FullName", string.Format("{0} {1}", companyUserViewModel.UserDto.FirstName, companyUserViewModel.UserDto.LastName)));
                            await userManager.AddClaimAsync(companyUser, new System.Security.Claims.Claim("AccountId", companyUser.AccountId.ToString()));

                            //   var currentUser = await userManager.GetUserAsync(HttpContext.User);
                            var companyuserDto = new Helpdesk.Service.Dto.CompanyUserDto()
                            {
                                CreatedUser = currentUser.FirstName + " " + currentUser.LastName,
                                ModifiedUser = currentUser.FirstName + " " + currentUser.LastName,
                                CreateDate = DateTime.Now,
                                ModifyDate = DateTime.Now,
                                FirstName = companyUserViewModel.UserDto.FirstName,
                                LastName = companyUserViewModel.UserDto.LastName,
                                TrIdentityNumber = companyUserViewModel.UserDto.TrIdentityNumber,
                                Title = companyUserViewModel.UserDto.Title,
                                Email = companyUserViewModel.UserDto.Email,
                                MobileNumber = companyUserViewModel.UserDto.MobileNumber,
                                AccountId = companyUser.AccountId,

                                Address = new Helpdesk.Model.Entities.Address(),
                                RecordStatus = Helpdesk.Model.Enums.RecordStatus.A,
                            };
                            if (companyUserViewModel.CompanyId > 0)
                            {
                                companyuserDto.CompanyId = companyUserViewModel.CompanyId;
                            }
                            else
                            {// Yönetici Admini ise
                                var companyuser = companyUserService.GetCompanyUserbyUserAccountId(currentUser.AccountId);

                                companyuserDto.CompanyId = companyuser.CompanyId;
                            }
                            //var currentRoles = userManager.GetRolesAsync(currentUser);

                            var CUserId = companyUserService.Create(companyuserDto);
                            return Ok();
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return BadRequest(ModelState);
                    }
                    else
                    {// limit bilgileri dolu ise
                        return UnprocessableEntity();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult Edit(int id)
        {
            var users = userManager.Users.Where(user => user.IsEnabled).ToList();
            var companyUser = companyUserService.GetAll().Where(user => user.Id == id && user.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A);
            var joinedData = users.Join(companyUser,
                p => p.AccountId,
                q => q.AccountId,
                (p, q) => p.Id).FirstOrDefault();
            var account = userManager.FindByIdAsync(joinedData).Result;
            var roles = userManager.GetRolesAsync(account).Result;
            var User = companyUserService.GetCompanyUserbyUserAccountId(account.AccountId);

            var CompanyUserViewModel = new CompanyUserViewModel();
            CompanyUserViewModel.UserDto.AccountId = User.AccountId;
            CompanyUserViewModel.UserDto.CompanyId = User.CompanyId;
            CompanyUserViewModel.UserDto.Email = User.Email;
            CompanyUserViewModel.UserDto.FirstName = User.FirstName;
            CompanyUserViewModel.UserDto.LastName = User.LastName;
            CompanyUserViewModel.UserDto.TrIdentityNumber = User.TrIdentityNumber;
            CompanyUserViewModel.IdentityUserId = account.Id;
            CompanyUserViewModel.UserDto.MobileNumber = User.MobileNumber;
            CompanyUserViewModel.CompanyId = User.CompanyId;
            foreach (var role in roles)
            {
                var userRole = CompanyUserViewModel.UserRoles.FirstOrDefault(p => p.RoleName == role);
                if (userRole != null)
                {
                    userRole.Selected = true;
                }
            }
            ViewBag.CompanyUserViewModel = JsonConvert.SerializeObject(CompanyUserViewModel);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody]CompanyUserViewModel companyUserViewModel)
        {
            try
            {
                var account = userManager.FindByIdAsync(companyUserViewModel.IdentityUserId).Result;
                var roles = userManager.GetRolesAsync(account).Result;
                account.FirstName = companyUserViewModel.UserDto.FirstName;
                account.LastName = companyUserViewModel.UserDto.LastName;
                account.Email = companyUserViewModel.UserDto.Email;
                account.PhoneNumber = companyUserViewModel.UserDto.MobileNumber;
                account.Id = companyUserViewModel.IdentityUserId;

                await userManager.UpdateAsync(account);
                // var v = CompanyUserViewModel.UserRoles.Where(p => !p.Selected && roles.Contains(p.RoleName)).Select(p => p.RoleName).ToList();
                var deleteRoles = userManager.RemoveFromRolesAsync(account, roles).Result;

                await userManager.AddToRolesAsync(account, companyUserViewModel.UserRoles.Where(p => p.Selected).Select(p => p.RoleName).ToList());

                companyUserViewModel.UserDto.ModifiedUser = User.FindFirst("FullName").Value;

                var companyUser = companyUserService.GetCompanyUserbyUserAccountId(account.AccountId);
                companyUserViewModel.UserDto.Id = companyUser.Id;
                companyUserViewModel.UserDto.AccountId = companyUser.AccountId;

                companyUserService.Update(companyUserViewModel.UserDto);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                companyUserService.DeleteCompanyUser(id, User.FindFirst("FullName").Value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //Burdan sonrası Ezgi'nin
        public IActionResult GetUserForCompany(int companyId)
        {
            var agentList = applicationUserService.GetAllUser();
            var mapping = companyAgentMappingService.GetMappingsByCompanyId(companyId);

            var joinedData = mapping.Join(agentList,
                            e1 => e1.ApplicationUserId,
                            e2 => e2.Id,
                            (e1, e2) => e2).Distinct();

            return Content(JsonConvert.SerializeObject(joinedData), "application/json");
        }

        public ActionResult AgentCustomizing()
        {
            try
            {
                var user = httpContextAccessor.HttpContext.User;
                IEnumerable<Claim> claims = user.Claims;

                var currentUserRole = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(p => p.Value);
                var currentUser = userManager.GetUserAsync(HttpContext.User).Result;
                var accountId = currentUser.AccountId;

                var companyId = 0;

                var mapping = companyAgentMappingService.GetAll();

                var customizedAgentViewModel = new CustomizedAgentViewModel();

                if (currentUserRole.Contains("Company Admin"))
                {
                    companyId = companyUserService.GetCompanyUserbyUserAccountId(accountId).CompanyId;
                    mapping = companyAgentMappingService.GetMappingsByCompanyId(companyId);
                    customizedAgentViewModel.CompanyId = companyId;
                }

                var agentList = applicationUserService.GetAllUser();

                var joinedData = mapping.Join(agentList,
                            e1 => e1.ApplicationUserId,
                            e2 => e2.Id,
                            (e1, e2) => e2).Distinct();

                ViewBag.Agents = JsonConvert.SerializeObject(joinedData);

                var companies = companyService.GetAllCompanyJson();
                ViewBag.Companies = companies;

                ViewBag.Model = JsonConvert.SerializeObject(customizedAgentViewModel);
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }

        [HttpPost]
        public ActionResult AgentCustomizing([FromBody]CustomizedAgentViewModel customizedModel)
        {
            try
            {
                if (customizedModel.CompanyId > 0 && customizedModel.ApplicationUsers != null)
                {
                    var mappings = companyAgentMappingService.GetAll().Where(p => p.CompanyId == customizedModel.CompanyId && customizedModel.ApplicationUsers.Select(q => q.Id).Contains(p.ApplicationUserId));

                    foreach (var map in mappings)
                    {
                        map.IsSpecifiedForCompany = true;
                        companyAgentMappingService.Update(map);
                    }
                    return View();
                }
                else
                {
                    return UnprocessableEntity();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}