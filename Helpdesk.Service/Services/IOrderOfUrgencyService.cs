using Helpdesk.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Services
{
    public interface IOrderOfUrgencyService
    {
        string GetAllUrgencyJson();

        IEnumerable<OrderOfUrgency> GetAll();
    }
}
