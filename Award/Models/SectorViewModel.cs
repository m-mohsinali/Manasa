using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class SectorViewModel
    {
        public SectorViewModel()
        {
            this.QualitySectorManagerUsers = new List<TeamMemberViewModel>();
        }

        public SectorViewModel(Sector sector)
        {
            this.Sector = sector;
            this.QualitySectorManagerUsers = new List<TeamMemberViewModel>();
        }

        public SectorViewModel(Sector sector, List<TeamMemberViewModel> qualitySectorManagerUsers)
        {
            this.Sector = sector;
            this.QualitySectorManagerUsers = qualitySectorManagerUsers;
        }

        public Sector Sector { get; set; }
        public string IsSubmitted { get; set; }
        public List<TeamMemberViewModel> QualitySectorManagerUsers { get; set; }
    }
}
