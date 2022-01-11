using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public enum AwardType
    {
        SpecialAward = 1,
        Individual = 2,
        Corporate = 3
    }

     

    public enum AwardStatus
    {
        New = 1,
        Open = 2,
        Closed = 3
    }
    public class Awards : BaseEntity
    {
       // public long Id { get; set; }

        [Required, DisplayName("Award Name")]
        
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
       // public AwardType AwardType { get; set; }
        public AwardStatus AwardStatus { get; set; }

        [DisplayName("Award Image")]
        public string AwardImagePath { get; set; }

        //[DisplayName("Supporting Documents")]
        //public string DocumentPath { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required, DisplayName("Opening Date")]
        [DataType(DataType.Date)]
        public DateTime OpeningDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required, DisplayName("Closing Date")]
        [DataType(DataType.Date)]
        public DateTime ClosingDate { get; set; }

        public virtual IList<AwardDocuments> AwardDocuments { get; set; }
        /*
        //[DisplayName("Created Date")]
       //public DateTime CreationAt { get; set; }
       //public DateTime? DeletedAt { get; set; }

        //public virtual ICollection<Category> Category { get; set; }*/
    }
}
