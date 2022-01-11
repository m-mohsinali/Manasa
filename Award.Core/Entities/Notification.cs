using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string NotificationText { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("NotificationType")]
        public long? NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public DateTime? CheckDate { get; set; }
    }
    public class NotificationType : BaseEntity
    {
        public string TypeEn { get; set; }
        public string TypeAr { get; set; }
    }
}
