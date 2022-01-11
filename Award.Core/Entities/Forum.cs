using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Award.Core.Entities
{
   public class Forum : BaseEntity
    {
        public string ForumText { get; set; }

        [ForeignKey("Category")]
        public long? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }
        public virtual User User { get; set; }
    }
    public class ForumDetails : BaseEntity
    {
        public string ForumDetailText { get; set; }
        [ForeignKey("User")]
        public long? UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Forum")]
        public long? ForumId { get; set; }
        public virtual Forum Forum { get; set; }
    }


}
