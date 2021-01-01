using AutoMapper;
using Helpdesk.Data.Repositories;
using Helpdesk.Model.Entities;
using Helpdesk.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Helpdesk.Service.Services
{
    public class OrderOfUrgencyService : IOrderOfUrgencyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IOrderOfUrgencyRepository orderOfUrgencyRepository;

        public OrderOfUrgencyService(IUnitOfWork unitOfWork,
            IOrderOfUrgencyRepository orderOfUrgencyRepository)
        {
            this.unitOfWork = unitOfWork;
            this.orderOfUrgencyRepository = orderOfUrgencyRepository;
        }

        public IEnumerable<OrderOfUrgency> GetAll()
        {
            return orderOfUrgencyRepository.GetAll();
        }

        public string GetAllUrgencyJson()
        {
            Expression<Func<OrderOfUrgency, bool>> expression = p => p.RecordStatus == Helpdesk.Model.Enums.RecordStatus.A;

            var data = orderOfUrgencyRepository.GetAllUrgencyJson(expression);

            return data;
        }
    }
}
