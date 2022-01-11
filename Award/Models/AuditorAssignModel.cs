using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class AuditorAssignModel
    {
        public string UserName { get; set; }
        public long QsmEntryId { get; set; }
        public long CatId { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime? SubmittedAt { get; set; }
        public List<TeamDetailViewModel> TeamDetailViewModel { get; set; }
        public string CatName { get; set; }
        public bool IsSubmitted { get; set; }
        public string AwardName { get; set; }
        public long QsmUserId { get; set; }




    }

    public class TeamDetailViewModel
    {
        public string TeamName { get; set; }
        public long Id { get; set; }

        public static implicit operator TeamDetailViewModel(List<TeamDetailViewModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
