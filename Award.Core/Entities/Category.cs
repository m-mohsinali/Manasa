using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class Category : BaseEntity
    {
        //public long Id { get; set; }
        [Required]
        public long IdAward { get; set; }
        [Required, DisplayName("Category Name")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, DisplayName("Award Type")]
        public AwardType AwardType { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        [Required, DisplayName("Opening Date")]
        public DateTime OpeningDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        [Required, DisplayName("Closing Date")]
        public DateTime ClosingDate { get; set; }
        //public DateTime CreationDate { get; set; }
        //public DateTime? DeletionDate { get; set; }
        public long CurrentStatusId { get; set; }
       
        public virtual Awards Award { get; set; }

        public virtual IList<CategoryCriteria> CategoryCriteria { get; set; } /*=new List<CategoryCriteria>(10);*/
        public virtual IList<CategoryDocument> CategoryDocuments { get; set; }
        //public virtual IList<CategoryEmployee> CategoryEmployee { get; set; }
        //public virtual IList<CategorySector> CategorySector { get; set; }

        //public virtual ICollection<CategoryTeams> CategoryTeams { get; set; }

    }
}
