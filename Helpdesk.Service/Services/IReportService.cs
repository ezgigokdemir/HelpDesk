using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface IReportService
    {
        string AnnualOrMonthlyReportJson(int year, int month, string taskStatusfilterKey);
        string GetApplicationUserReportJson(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey);
        List<AppUserExcelGeneralReportDto> GetApplicationExcelUserReport(DateTime start, DateTime end, int reportFilterType, string taskStatusfilterKey);
        byte[] CreateExcelPackage(List<AppUserExcelGeneralReportDto> reports);
    }
}
