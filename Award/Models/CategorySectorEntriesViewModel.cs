using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class CategorySectorEntriesViewModel
    {
        public long Id { get; set; }
        public string NameEn   {get;set;}
        public string NameAr   {get;set;}
        public bool IsSubmitted{get;set;}
        public bool IsQsmAssigned { get; set; }

        


    }
    public class CategoryTeamEntries
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsSubmitted { get; set; }
        public string UserName { get; set; }



    }


}
