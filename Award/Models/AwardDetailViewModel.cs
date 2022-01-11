using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class AwardDetailViewModel
    {
        public AwardsViewModel award { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
  
}
