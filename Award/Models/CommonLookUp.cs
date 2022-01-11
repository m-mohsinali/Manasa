using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class CommonLookUp
    {

        public IList<Sector> sector { get; set; }
        public IList<Section> section { get; set; }
        public IList<Unit> unit { get; set; }
        public IList<Branch> branch { get; set; }
        public IList<Department> department { get; set; }
        public IList<Employee> employee { get; set; }
        public IList<UsersDetail> userDetail { get; set; }
        public long CategoryId { get; set; }
        public long saveStatus { get; set; }
        public long selectedSectorId { get; set; }

        public bool iSAssignButtonEnable { get; set; }


    }
}
