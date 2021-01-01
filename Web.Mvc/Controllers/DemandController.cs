using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using Helpdesk.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Mvc.Areas.Identity.Data;
using Web.Mvc.ViewModels;

namespace Web.Mvc.Controllers
{
    public class DemandController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> signInManager;
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDemandService demandService;
        private readonly IOrderOfUrgencyService urgencyService;
        private readonly IApplicationUserService applicationUserService;
        private readonly ICompanyUserService companyUserService;
        private readonly ICompanyAgentMappingService companyAgentMappingService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICompanyService companyService;


        public DemandController(IUnitOfWork unitOfWork,
            IDemandService demandService,
            SignInManager<CustomIdentityUser> signInManager,
            UserManager<CustomIdentityUser> userManager,
            IOrderOfUrgencyService urgencyService,
            IApplicationUserService applicationUserService,
            IHttpContextAccessor httpContextAccessor,
            ICompanyUserService companyUserService,
            ICompanyAgentMappingService companyAgentMappingService,
            ICompanyService companyService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.demandService = demandService;
            this.urgencyService = urgencyService;
            this.applicationUserService = applicationUserService;
            this.httpContextAccessor = httpContextAccessor;
            this.companyUserService = companyUserService;
            this.companyAgentMappingService = companyAgentMappingService;
            this.companyService = companyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ContentResult GetAllOutgoingDemand(int? pageNumber, int? pageLength)
        {
            var user = httpContextAccessor.HttpContext.User;
            IEnumerable<Claim> claims = user.Claims;

            var currentUserRole = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(p => p.Value);

            var userAccountId = claims.Where(x => x.Type == "AccountId").Select(c => c.Value).SingleOrDefault();

            if (currentUserRole.Contains("Company Admin"))
            {
                var companyId = companyUserService.GetCompanyUserbyUserAccountId(Guid.Parse(userAccountId)).CompanyId;
                return Content(demandService.GetDemandsJson(pageNumber, pageLength, companyId), "application/json");
            }

            return Content(demandService.GetDemandsJson(pageNumber, pageLength), "application/json");
        }

        public ContentResult GetAllOutgoingDemandByUser(int? pageNumber, int? pageLength)
        {
            var userAccountId = GetUserAccountId();
            return Content(demandService.GetDemandsJson(pageNumber, pageLength, userAccountId), "application/json");
        }

        private Guid GetUserAccountId()
        {
            var user = httpContextAccessor.HttpContext.User;
            IEnumerable<Claim> claims = user.Claims;
            var currentUserRole = httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role).Select(p => p.Value);
            var AccountId = claims.Where(x => x.Type == "AccountId").Select(c => c.Value).SingleOrDefault();

            return Guid.Parse(AccountId);
        }

        public IActionResult CreateDemand()
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemandAsync([FromBody]DemandDto demandDto)
        {
            try
            {

                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var userAccountId = GetUserAccountId();

                var roles = await userManager.GetRolesAsync(currentUser);



                demandDto.CreatedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandDto.CreateDate = DateTime.Now;
                demandDto.ModifyDate = DateTime.Now;
                demandDto.CompanyUserAccountId = userAccountId;

                if (roles.Contains("Company Admin") || roles.Contains("Company User"))
                {
                    var companyId = companyUserService.GetCompanyUserbyUserAccountId(userAccountId).CompanyId;
                    demandDto.CompanyId = companyId;
                }

                var result = demandService.Create(demandDto);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult OutgoingDemands()
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;
            return View();
        }

        public IActionResult OutgoingDemandsByUser()
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;
            return View();
        }

        public IActionResult DetailsOfOutgoingDemand(int id)
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;
            var demand = demandService.GetDemandByIdJson(id);
            ViewBag.Demand = demand;
            return View();
        }

        public IActionResult Delete(int id)
        {
            var demandDto = demandService.GetDemandDto(id);
            ViewBag.Demand = JsonConvert.SerializeObject(demandDto);
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmedAsync(int id)
        {
            try
            {
                demandService.Delete(id);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        public IActionResult Edit(int id)
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;
            var demandDto = demandService.GetDemandDto(id);
            ViewBag.Demand = JsonConvert.SerializeObject(demandDto);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit([FromBody]DemandDto demandDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandService.Update(demandDto);

                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public ContentResult GetAllIncomingDemands(int? orderType, string filterType)
        {
            var user = httpContextAccessor.HttpContext.User;
            IEnumerable<Claim> claims = user.Claims;
            var userIdentityId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = userManager.FindByIdAsync(userIdentityId).Result;
            var roles = userManager.GetRolesAsync(account).Result;
            var applicationUserId = applicationUserService.GetAllUser().Where(p => p.AccountId == account.AccountId).Select(p => p.Id).FirstOrDefault();

            //Belirtilen filtredeki tüm talepler
            var incomingDemands = demandService.GetAll(filterType, roles.ToList(), account.AccountId);

            if (!roles.Contains("Admin"))
            {
                //User'a ait tüm mappingler
                var mapping = companyAgentMappingService.GetMappingsByUserId(applicationUserId);

                //Özelleştirme koşularını sağlayan nihai talepler
                List<Demand> demands = new List<Demand>();

                //En az bir özelliştirme yapılan firmaların id'leri
                List<int> idsForCustomizedCompany = new List<int>();

                //Hiç özelliştirme yapılmayan firmaların id'leri
                List<int> idsForNoncustomizedCompany = new List<int>();

                foreach (var companyId in mapping.Select(p => p.CompanyId))
                {
                    var mapCompanyIds = companyAgentMappingService.GetMappingsByCompanyId(companyId);

                    if (mapCompanyIds.Where(p => p.IsSpecifiedForCompany).Select(q => q.CompanyId).ToList().Count > 0)
                    {
                        idsForCustomizedCompany.AddRange(mapCompanyIds.Where(p => p.IsSpecifiedForCompany).Select(q => q.CompanyId).ToList());
                    }
                    else
                    {
                        idsForNoncustomizedCompany.AddRange(mapCompanyIds.Select(q => q.CompanyId).ToList());
                    }
                }

                if (idsForCustomizedCompany.Count > 0)
                {
                    //Kullanıcının o firmada özelleştirilen bir mappingi var mı?
                    var maps = mapping.Where(p => idsForCustomizedCompany.Contains(p.CompanyId) && p.ApplicationUserId == applicationUserId && p.IsSpecifiedForCompany);

                    var demandsNew = incomingDemands.Where(p => maps.Select(q => q.CompanyId).Contains(p.CompanyId)).ToList();

                    demands.AddRange(demandsNew);
                }

                if (idsForNoncustomizedCompany.Count > 0)
                {
                    var maps = mapping.Where(p => idsForNoncustomizedCompany.Contains(p.CompanyId));

                    var demandsNew = incomingDemands.Where(p => maps.Select(q => q.CompanyId).Contains(p.CompanyId)).ToList();

                    demands.AddRange(demandsNew);
                }

                incomingDemands = demands;
            }

            if (orderType != null)
            {
                if (orderType == 1)
                {
                    incomingDemands = incomingDemands.OrderBy(p => p.OrderOfUrgencyId);
                }
                else if (orderType == 2)
                {
                    incomingDemands = incomingDemands.OrderByDescending(p => p.OrderOfUrgencyId);
                }
                else if (orderType == 3)
                {
                    incomingDemands = incomingDemands.OrderBy(p => p.CreateDate);
                }
                else if (orderType == 4)
                {
                    incomingDemands = incomingDemands.OrderByDescending(p => p.CreateDate);
                }
            }

            return Content(JsonConvert.SerializeObject(incomingDemands), "application/json");
        }

        public IActionResult IncomingDemands()
        {
            var urgencies = urgencyService.GetAllUrgencyJson();
            ViewBag.Urgencies = urgencies;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AssignDemand([FromBody]DemandDto demandDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var userId = applicationUserService.GetUserDto(currentUser.AccountId).AccountId;
                demandDto.ApplicationUserAccountId = userId;
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandService.AssignDemand(demandDto);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetDemandAsSolved([FromBody]DemandDto demandDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var userId = applicationUserService.GetUserDto(currentUser.AccountId).AccountId;
                demandDto.ApplicationUserAccountId = userId;
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandService.SetDemandAsSolved(demandDto);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CancelDemandAssignment([FromBody]DemandDto demandDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var userId = applicationUserService.GetUserDto(currentUser.AccountId).AccountId;
                demandDto.ApplicationUserAccountId = userId;
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandService.CancelDemandAssignment(demandDto);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetDemandAsCompleted([FromBody]DemandDto demandDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);
                var userId = applicationUserService.GetUserDto(currentUser.AccountId).AccountId;
                demandDto.ApplicationUserAccountId = userId;
                demandDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                demandService.SetDemandAsCompleted(demandDto);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

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

                    if (customizedModel.RecordSelection != 1)
                    {
                        var mappingsNonSpecified = companyAgentMappingService.GetAll().Where(p => p.CompanyId == customizedModel.CompanyId && !customizedModel.ApplicationUsers.Select(q => q.Id).Contains(p.ApplicationUserId));

                        foreach (var map in mappingsNonSpecified)
                        {
                            map.IsSpecifiedForCompany = false;
                            companyAgentMappingService.Update(map);
                        }
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