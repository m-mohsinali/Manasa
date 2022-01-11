using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
     public class CategorySubCriteria
    {
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, RegularExpression("^\\d{1,6}", ErrorMessage = "Enter Digits")]
        public short Marks { get; set; }
        [Required, DisplayName("Max Words"), RegularExpression("^\\d{1,6}", ErrorMessage = "Enter Digits")]
        public short MaxWords { get; set; }
        public string? Answer { get; set; }
        public short? Comments { get; set; }
        //public long IdCategoryCriteria { get; set; }
        public long CategoryCriteriaId { get; set; }

        public virtual CategoryCriteria CategoryCriteria { get; set; }
    }
}
