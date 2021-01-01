using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Helpdesk.Model.Enums;

namespace Helpdesk.Model.Entities
{
    public class BaseEntity
    {
        //public Guid Id { get; set; }
        //public Guid UniqueId { get; set; }

        public DateTime? CreateDate { get; set; }

        [System.ComponentModel.DataAnnotations.StringLength(128)]
        public string CreatedUser { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifyDate { get; set; }

        [System.ComponentModel.DataAnnotations.StringLength(128)]
        public string ModifiedUser { get; set; }

        [Column(TypeName = "nvarchar(1)")]
        public RecordStatus RecordStatus { get; set; }
    }
}
