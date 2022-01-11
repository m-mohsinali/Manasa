using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
    public class UserEvent : BaseEntity
    {
        public string EventDescription { get; set; }

        [ForeignKey("FromUser")]
        public long? FromUserId { get; set; }
        public virtual User FromUser { get; set; }
        [ForeignKey("ToUser")]
        public long? ToUserId { get; set; }
        public virtual User ToUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
   
}
