using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class NotificationViewModel
    {
        public long NotificationId { get; set; }
        public string NotificationText { get; set; }
        public long UserId { get; set; }
        public long NotificationTypeId { get; set; }
        public string NotificationType { get; set; }
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }
    }
}
