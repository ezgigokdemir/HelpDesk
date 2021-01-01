using AutoMapper;
using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class DemandService : IDemandService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDemandRepository demandRepository;
        private readonly ICompanyUserRepository companyUserRepository;
        private readonly IMapper mapper;

        public DemandService(IUnitOfWork unitOfWork,
            IDemandRepository demandRepository,
            ICompanyUserRepository companyUserRepository,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.demandRepository = demandRepository;
            this.companyUserRepository = companyUserRepository;
            this.mapper = mapper;
        }

        public int Create(DemandDto demandDto)
        {
            var demand = mapper.Map<Demand>(demandDto);

            demandRepository.Insert(demand);
            unitOfWork.SaveChanges();

            return demand.Id;
        }

        public DemandDto GetDemandDto(int Id)
        {
            var demand = demandRepository.GetSingle(p => p.Id == Id && p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            return mapper.Map<DemandDto>(demand);
        }

        public List<DemandDto> GetAllDemand()
        {
            var demands = demandRepository.GetAllDemand(p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A);

            List<DemandDto> demandDtos = new List<DemandDto>();

            foreach (var demand in demands)
            {
                var demanDto = mapper.Map<DemandDto>(demand);
                demandDtos.Add(demanDto);
            }

            return demandDtos;
        }

        public string GetDemandByIdJson(int Id)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.Id == Id;
            return demandRepository.GetDemandByCondition(expression);
        }

        //public string GetDemandsByCompanyUserId(int companyUserId)
        //{
        //    Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A && p.CompanyUserId == companyUserId;
        //    return demandRepository.GetDemandByCondition(expression);
        //}

        public string GetDemandsJson(int? pageNumber, int? pageLength)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;
            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            var data = demandRepository.GetAllDemandJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public string GetDemandsJson(int? pageNumber, int? pageLength, Guid? userAccountId)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            if (userAccountId != null)
            {
                expression = expression.And(p => p.CompanyUserAccountId == userAccountId);
            }

            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            var data = demandRepository.GetAllDemandJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public void Delete(int Id)
        {
            var demand = demandRepository.Find(Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.RecordStatus = Helpdesk.Model.Enums.RecordStatus.D;
            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public string GetDemandsJson(int? pageNumber, int? pageLength, int? companyId)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var users = companyUserRepository.GetAll(p => p.CompanyId == companyId).Select(p => p.AccountId).ToList();

            if (companyId != null)
            {
                expression = expression.And(p => users.Contains(p.CompanyUserAccountId));
            }

            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            var data = demandRepository.GetAllDemandJson(expression, out int records, null, skip, pageLength);
            return data;
        }

        public void Update(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.Text = demandDto.Text;
            demand.OrderOfUrgencyId = demandDto.OrderOfUrgencyId;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public void UpdateForDeletedUser(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.ApplicationUserAccountId = null;
            demand.IsAccepted = false;
            demand.IsDissolved = false;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public void AssignDemand(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.IsAccepted = demandDto.IsAccepted;
            demand.ApplicationUserAccountId = demandDto.ApplicationUserAccountId;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public void SetDemandAsSolved(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.IsDissolved = demandDto.IsDissolved;
            demand.ApplicationUserAccountId = demandDto.ApplicationUserAccountId;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public void CancelDemandAssignment(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.IsAccepted = demandDto.IsAccepted;
            demand.ApplicationUserAccountId = null;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public void SetDemandAsCompleted(DemandDto demandDto)
        {
            var demand = demandRepository.Find(demandDto.Id);
            if (demand == null)
            {
                throw new KeyNotFoundException();
            }

            demand.IsCompleted = demandDto.IsCompleted;
            demand.ApplicationUserAccountId = demandDto.ApplicationUserAccountId;
            demand.ModifiedUser = demandDto.ModifiedUser;
            demand.ModifyDate = DateTime.Now;

            demandRepository.Update(demand);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<Demand> GetAll()
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var data = demandRepository.GetAllDemand(expression);
            return data;
        }

        public IEnumerable<Demand> GetAll(string filterType, List<string> roles, Guid userAccountId)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            if (!string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "completed":
                        expression = expression.And(p => p.IsCompleted && p.IsDissolved && p.IsAccepted && p.ApplicationUserAccountId == userAccountId);
                        break;

                    case "assigned":
                        expression = expression.And(p => p.IsAccepted && p.IsDissolved != true && p.IsCompleted != true && p.ApplicationUserAccountId == userAccountId);
                        break;

                    case "notAssigned":
                        expression = expression.And(p => p.IsAccepted != true && p.IsDissolved != true && p.IsAccepted != true);
                        break;

                    case "solved":
                        expression = expression.And(p => p.IsDissolved && p.IsCompleted != true && p.IsAccepted && p.ApplicationUserAccountId == userAccountId);
                        break;

                    default:
                        break;
                }
            }

            var data = demandRepository.GetAll(expression);
            return data;
        }

        public IEnumerable<Demand> GetAll(int? pageNumber, int? pageLength, string filterType)
        {
            Expression<Func<Demand, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            int? skip = null;
            if (pageNumber.HasValue && pageLength.HasValue)
            {
                skip = (pageNumber - 1) * pageLength;
            }

            if (!string.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "completed":
                        expression = expression.And(p => p.IsCompleted);
                        break;

                    case "assigned":
                        expression = expression.And(p => p.IsAccepted);
                        break;

                    case "notAssigned":
                        expression = expression.And(p => p.IsAccepted != true);
                        break;

                    default:
                        break;
                }
            }

            var data = demandRepository.GetAllDemand(expression, out int records, null, skip, pageLength);

            return data;
        }
    }
}
