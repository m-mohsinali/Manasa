using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
    public class WinnerAnnouncement : BaseEntity
    {
        public string Description { get; set; }
        public string AwardName { get; set; }
        public string CategoryName { get; set; }
        public string AwardYear { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public string ImagePath { get; set; }
        public bool IsShow { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Awards")]
        public long? AwardId { get; set; }
        public virtual Awards Awards { get; set; }

        [ForeignKey("Category")]
        public long? CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }
}
