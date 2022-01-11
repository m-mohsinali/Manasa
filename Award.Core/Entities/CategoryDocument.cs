using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
   public class CategoryDocument
    {
        public int Id { get; set; }
        public long CategoryId { get; set; }
        public string SupportingDocumentPath { get; set; }
        public string DocumentName { get; set; }
        public virtual Category Category { get; set; }
    }
   

}
