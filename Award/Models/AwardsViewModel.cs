using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Award.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Award.Web.Models
{
    public class AwardsViewModel
    {
        public Awards award { get; set; }
        public IFormFile awardImagePath { get; set; }
        public IList<IFormFile> AwardDocuments { get; set; }
        public List<Category> Categories { get; set; }
        public Category category { get; set; }
        public string MediaType { get; set; }

        // public List<IFormFile> documentPath { get; set; }       
    }
}
