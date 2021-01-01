using Helpdesk.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helpdesk.Service.Dto
{
    public class DataTransferObject
    {
        //public Guid Id { get; set; }
        //public Guid UniqueId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ModifyDate { get; set; }

        public string ModifiedUser { get; set; }

        public RecordStatus RecordStatus { get; set; }
    }
}
