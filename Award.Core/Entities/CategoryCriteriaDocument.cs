using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
    public class CategoryCriteriaDocument
    {
        public int Id { get; set; }
        public string SupportingDocumentPath { get; set; }
        public long IdCategoryCriteria { get; set; }
        public virtual CategoryCriteria CategoryCriteria { get; set; }
    }
}
