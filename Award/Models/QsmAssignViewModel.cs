using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class QsmAssignViewModel
    {

        public long CatId { get; set; }
        public string Name { get; set; }
        public string AwardName { get; set; }
        public long AwardType { get; set; }
        public string UserName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime OpeningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime ClosingDate { get; set; }
        public long SectorId { get; set; }
        public string SectorName { get; set; }
        public AwardType AwardTypeNew { get; set; }







    }
}
