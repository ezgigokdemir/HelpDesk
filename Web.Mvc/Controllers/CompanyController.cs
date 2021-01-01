using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Helpdesk.Data.Repositories;
using Helpdesk.Service.Dto;
using Helpdesk.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Mvc.Areas.Identity.Data;

namespace Web.Mvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> signInManager;
        private readonly UserManager<CustomIdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICompanyService companyService;
        //private readonly IHttpContextAccessor httpContextAccessor;

        public CompanyController(IUnitOfWork unitOfWork,
            ICompanyService companyService,
            SignInManager<CustomIdentityUser> signInManager,
            UserManager<CustomIdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.companyService = companyService;
        }

        public IActionResult Index()
        {
            var companies = companyService.GetAllCompanyJson();
            ViewBag.Companies = companies;
            return View();
        }

        public ContentResult GetAll(int? pageNumber, int? pageLength, int? searchType, string searchString)
        {
            return Content(companyService.GetCompaniesJson(pageNumber, pageLength, searchType, searchString), "application/json");
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CompanyDto companyDto)
        {
            try
            {
                //var currentUserName = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                var currentUser = await userManager.GetUserAsync(HttpContext.User);

                if (!String.IsNullOrEmpty(companyDto.CompanyName))
                {
                    companyDto.CreatedUser = currentUser.FirstName + " " + currentUser.LastName;
                    companyDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                    companyDto.CreateDate = DateTime.Now;
                    companyDto.ModifyDate = DateTime.Now;
                    var result = companyService.Create(companyDto);
                    return Ok();
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Detail(int id)
        {
            var company = companyService.GetCompanyByIdJson(id);
            ViewBag.Company = company;

            return View();
        }

        public IActionResult Edit(int id)
        {
            var company = companyService.GetCompanyByIdJson(id);
            ViewBag.CompanyModel = company;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit([FromBody]CompanyDto companyDto)
        {
            try
            {
                var currentUser = await userManager.GetUserAsync(HttpContext.User);

                if (!String.IsNullOrEmpty(companyDto.CompanyName))
                {
                    companyDto.ModifiedUser = currentUser.FirstName + " " + currentUser.LastName;
                    companyService.Update(companyDto);
                }
                return View();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}