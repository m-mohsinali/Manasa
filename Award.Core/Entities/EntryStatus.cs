using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class EntryStatus : BaseEntity
    {
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}
