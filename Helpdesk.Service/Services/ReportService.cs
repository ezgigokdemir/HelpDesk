using AutoMapper;
using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Specialized;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Helpdesk.Service.Dto;
using System.IO;
using System.Drawing;

namespace Helpdesk.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IDemandRepository demandRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ICompanyAgentMappingRepository companyAgentMappingRepository;
        private readonly IApplicationUserService applicationUserService;
        private readonly IOrderOfUrgencyService orderOfUrgencyService;
        private readonly ICompanyService companyService;

        public ReportService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IDemandRepository demandRepository,
            IApplicationUserRepository applicationUserRepository,
            ICompanyAgentMappingRepository companyAgentMappingRepository,
            IApplicationUserService applicationUserService,
            IOrderOfUrgencyService orderOfUrgencyService,
            ICompanyService companyService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.demandRepository = demandRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.companyAgentMappingRepository = companyAgentMappingRepository;
            this.applicationUserService = applicationUserService;
            this.orderOfUrgencyService = orderOfUrgencyService;
            this.companyService = companyService;
        }

        public string AnnualOrMonthlyReportJson(int year, int month, string taskStatusfilterKey)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            if (year != 0 && month == 0)
                expression = expression.And(p => p.CreateDate.Value.Year == year);

            if (month != 0 && year != 0)
                expression = p => p.CreateDate.Value.Month == month && p.CreateDate.Value.Year == year;

            if (!string.IsNullOrEmpty(taskStatusfilterKey))
            {
                switch (taskStatusfilterKey)
                {
                    case "dissolved": // çözülmüş
                        expression = expression.And(p => p.IsDissolved == true);
                        break;

                    case "completed": // tamamlandı
                        expression = expression.And(p => p.IsCompleted == true);
                        break;

                    case "assigned": // atanmış
                        expression = expression.And(p => p.IsAccepted == true);
                        break;

                    case "notAssigned": // acık
                        expression = expression.And(p => p.IsAccepted != true);
                        break;

                    default:
                        expression = expression.And(p => p.IsAccepted != true);
                        break;
                }
            }
            else
            {
                expression = expression.And(p => p.IsAccepted != true);
            }

            var demands = demandRepository.GetAll(expression, out int records, null);

            if (taskStatusfilterKey == "notAssigned")
            {
                var emergencys = orderOfUrgencyService.GetAll();
                var joinedData = emergencys.GroupJoin(inner: demands,
                                         outerKeySelector: urgency => urgency.Id,
                                         innerKeySelector: d => d.OrderOfUrgencyId,
                                         resultSelector: (urgency, uList) => new
                                         {
                                             Key = urgency.Id,
                                             Name = urgency.Title,
                                             Count = uList.Count()
                                         });
                return JsonConvert.SerializeObject(joinedData);
            }
            else
            {
                var agents = applicationUserRepository.GetAll(/*a => a.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A*/);
                var joinedData = agents.GroupJoin(inner: demands,
                                         outerKeySelector: agent => agent.AccountId,
                                         innerKeySelector: d => d.ApplicationUserAccountId,
                                         resultSelector: (agent, dList) => new
                                         {
                                             Key = agent.Id,
                                             Name = agent.FirstName + " " + agent.LastName,
                                             Count = dList.Count()
                                         });

                return JsonConvert.SerializeObject(joinedData);
            }
        }

        public string GetApplicationUserReportJson(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey)
        {
            var query = demandRepository.GetAll().Where(p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A
                                                    && p.CreateDate.Value >= start && p.CreateDate.Value <= end);

            if (!string.IsNullOrEmpty(taskStatusfilterKey))
            {
                switch (taskStatusfilterKey)
                {
                    case "dissolved": // çözülmüş
                        query = query.Where(p => p.IsDissolved == true);
                        break;

                    case "completed": // tamamlandı
                        query = query.Where(p => p.IsCompleted == true);
                        break;

                    case "assigned": // atanmış
                        query = query.Where(p => p.IsAccepted == true);
                        break;

                    case "notAssigned": // acık
                        query = query.Where(p => p.IsAccepted != true);
                        break;

                    default:
                        query = query.Where(p => p.IsAccepted != true);
                        break;
                }
            }
            else
            {
                query = query.Where(p => p.IsAccepted != true);
            }

            if (taskStatusfilterKey == "notAssigned")
            {
                var emergencys = orderOfUrgencyService.GetAll();
                var joinedData = emergencys.GroupJoin(inner: query,
                                         outerKeySelector: urgency => urgency.Id,
                                         innerKeySelector: d => d.OrderOfUrgencyId,
                                         resultSelector: (urgency, uList) => new
                                         {
                                             Key = urgency.Id,
                                             Name = urgency.Title,
                                             Count = uList.Count()
                                         });
                return JsonConvert.SerializeObject(joinedData);
            }
            else
            {
                var agents = applicationUserService.GetAll();
                var joinedData = agents.GroupJoin(inner: query,
                                         outerKeySelector: agent => agent.AccountId,
                                         innerKeySelector: d => d.ApplicationUserAccountId,
                                         resultSelector: (agent, dList) => new
                                         {
                                             Key = agent.Id,
                                             Name = agent.FirstName + " " + agent.LastName,
                                             Count = dList.Count()
                                         });
                return JsonConvert.SerializeObject(joinedData);
            }
        }

        public List<AppUserExcelGeneralReportDto> GetApplicationExcelUserReport(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey)
        {
            var query = demandRepository.GetAll().Where(p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A
                                                      && p.CreateDate.Value >= start && p.CreateDate.Value <= end);

            var agents = applicationUserService.GetAll();

            var joinedData = agents.GroupJoin(inner: query,
                                          outerKeySelector: agent => agent.AccountId,
                                          innerKeySelector: d => d.ApplicationUserAccountId,
                                          resultSelector: (agent, dList) => new
                                          {
                                              Key = agent.Id,
                                              UserName = agent.FirstName + " " + agent.LastName,
                                              Count = dList.Count(),
                                              AssigmentCount = dList.Where(x => x.IsAccepted == false).Count(),
                                              CompletedCount = dList.Where(y => y.IsCompleted == true).Count(),
                                              DissolvedCount = dList.Where(y => y.IsDissolved == true).Count(),
                                          }).Select(c => new AppUserExcelGeneralReportDto()
                                          {
                                              Id = c.Key,
                                              UserName = c.UserName,
                                              Count = c.Count,
                                              AssigmentCount = c.AssigmentCount,
                                              CompletedCount = c.CompletedCount,
                                              DissolvedCount = c.DissolvedCount,
                                          }).ToList();

            return joinedData;
        }

        public byte[] CreateExcelPackage(List<AppUserExcelGeneralReportDto> reports)
        {
            using (var ExcelPkg = new ExcelPackage())
            {
                ExcelWorksheet workSheet = ExcelPkg.Workbook.Worksheets.Add("Users");

                workSheet.TabColor = System.Drawing.Color.Red;
                workSheet.DefaultRowHeight = 12;

                var modelCells = workSheet.Cells["A1"];
                var modelRows = reports.Count() + 1;
                string modelRange = "A1:F" + modelRows.ToString();
                var modelTable = workSheet.Cells[modelRange];

                // Assign borders
                modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                modelTable.Style.Border.Top.Color.SetColor(Color.Blue);
                modelTable.Style.Border.Bottom.Color.SetColor(Color.Blue);
                modelTable.Style.Border.Left.Color.SetColor(Color.Blue);
                modelTable.Style.Border.Right.Color.SetColor(Color.Blue);
                modelTable.AutoFitColumns();
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#4E73DF");
                workSheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(colFromHex);
                workSheet.Cells["A1:F1"].Style.Font.Color.SetColor(Color.White);

                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;

                workSheet.Cells[1, 1].Value = "Id";
                workSheet.Cells[1, 2].Value = "User Name";
                workSheet.Cells[1, 3].Value = "Count";
                workSheet.Cells[1, 4].Value = "AssigmentCount";
                workSheet.Cells[1, 5].Value = "CompletedCount";
                workSheet.Cells[1, 6].Value = "DissolvedCount";
                var rowCounter = 2;
                foreach (var v in reports)
                {
                    workSheet.Cells[rowCounter, 1].Value = v.Id;
                    workSheet.Cells[rowCounter, 2].Value = v.UserName;
                    workSheet.Cells[rowCounter, 3].Value = v.Count;
                    workSheet.Cells[rowCounter, 4].Value = v.AssigmentCount;
                    workSheet.Cells[rowCounter, 5].Value = v.CompletedCount;
                    workSheet.Cells[rowCounter, 6].Value = v.DissolvedCount;

                    rowCounter++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();

                ExcelPkg.Workbook.Properties.Title = "UsersStatus";

                return ExcelPkg.GetAsByteArray();
            }
        }
    }
}