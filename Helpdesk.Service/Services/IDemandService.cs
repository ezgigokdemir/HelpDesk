using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface IDemandService
    {
        int Create(DemandDto demandDto);

        void Update(DemandDto demandDto);

        void Delete(int Id);

        void UpdateForDeletedUser(DemandDto demandDto);

        IEnumerable<Demand> GetAll();

        IEnumerable<Demand> GetAll(string filterType, List<string> roles, Guid userAccountId);

        IEnumerable<Demand> GetAll(int? pageNumber, int? pageLength, string filterType);

        string GetDemandsJson(int? pageNumber, int? pageLength);

        string GetDemandsJson(int? pageNumber, int? pageLength, Guid? userAccountId);

        string GetDemandsJson(int? pageNumber, int? pageLength, int? companyId);

        string GetDemandByIdJson(int Id);

        DemandDto GetDemandDto(int Id);

        List<DemandDto> GetAllDemand();

        void AssignDemand(DemandDto demandDto);

        void SetDemandAsSolved(DemandDto demandDto);

        void CancelDemandAssignment(DemandDto demandDto);

        void SetDemandAsCompleted(DemandDto demandDto);
        
        //string GetDemandsByCompanyUserId(int companyUserId);
    }
}
