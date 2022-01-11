using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class CategoryCriteria
    {
        public long Id { get; set; }
        public long IdCategory { get; set; }
        [Required]
        public string Description { get; set; }
        public long CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual IList<CategoryCriteriaDocument> CategoryCriteriaDocuments { get; set; }
        public virtual IList<CategorySubCriteria> CategorySubCriteria { get; set; }
    }
}
