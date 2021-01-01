using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Helpdesk.Data.Repositories;
using Helpdesk.Service.Dto;
using Helpdesk.Service.Services;
using Microsoft.AspNetCore.Identity;
using Web.Mvc.Areas.Identity.Data;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using Helpdesk.Model.Entities;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Web.Mvc.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDemandService demandService;
        private readonly ICompanyAgentMappingService companyAgentMappingService;
        private readonly IApplicationUserService applicationUserService;
        private readonly IReportService reportService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<CustomIdentityUser> userManager;

        public ReportController(IUnitOfWork unitOfWork,
            IDemandService demandService,
           ICompanyAgentMappingService companyAgentMappingService,
           IApplicationUserService applicationUserService,
           IReportService reportService,
            IHostingEnvironment hostingEnvironment,
            UserManager<CustomIdentityUser>userManager
            )
        {
            this.unitOfWork = unitOfWork;
            this.demandService = demandService;
            this.companyAgentMappingService = companyAgentMappingService;
            this.applicationUserService = applicationUserService;
            this.reportService = reportService;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GeneralReport()
        {
            ViewBag.Users = applicationUserService.GetAllUserJson();
            return View();
        }

        public IActionResult GetUserReportData(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey)
        {
            try
            {
                var reports = reportService.GetApplicationUserReportJson(start.AddMinutes(01).AddHours(1), end.AddMinutes(59).AddHours(23), reportFilterType, taskStatusfilterKey);

                var provider = new FileExtensionContentTypeProvider();
                return Content(reports, provider.Mappings[".json"]);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            throw new NotImplementedException();

        }

        [HttpGet]
        public IActionResult GetAllUserExcelReportData(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey)
        {
           
            DateTime dtNow=  DateTime.Now;
            var reports = reportService.GetApplicationExcelUserReport(start.AddMinutes(01).AddHours(1).AddYears(2000), dtNow, reportFilterType, taskStatusfilterKey);

            var provider = new FileExtensionContentTypeProvider();
            string contentType = provider.Mappings[".xlsx"];
          

           return File(reportService.CreateExcelPackage(reports), contentType);


        }

        [HttpGet]
        public IActionResult GetUserExcelReportData(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey)
        {

            DateTime dtNow = DateTime.Now;
            var reports = reportService.GetApplicationExcelUserReport(start.AddMinutes(01).AddHours(1).AddYears(2000), dtNow, reportFilterType, taskStatusfilterKey);

            var provider = new FileExtensionContentTypeProvider();
            string contentType = provider.Mappings[".xlsx"];


            return File(reportService.CreateExcelPackage(reports), contentType);


        }



        public IActionResult AnnualReport() //yıllık rapor
        {
            return View();
        }
        public IActionResult GetAnnualReportData(int year, int month, string taskStatusfilterKey)
        {
            var provider = new FileExtensionContentTypeProvider();
            return Content(reportService.AnnualOrMonthlyReportJson(year, month, taskStatusfilterKey), provider.Mappings[".json"]);
        }

        public IActionResult MonthlyReport()
        {
            return View();
        }
        public IActionResult GetMonthlyReportData(int year, int month, string taskStatusfilterKey)
        {
            try
            {
                var provider = new FileExtensionContentTypeProvider();
                return Content(reportService.AnnualOrMonthlyReportJson(year, month, taskStatusfilterKey), provider.Mappings[".json"]);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            throw new NotImplementedException();

        }

    }
}