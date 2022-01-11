using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class CategoryEntryComments : BaseEntity
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Comment { get; set; }
    }
}
