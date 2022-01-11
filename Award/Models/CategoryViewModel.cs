using Award.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public Sector Sector { get; set; }
        public IList<IFormFile> CategoryDocuments { get; set; }
        public List<string> CatigoryDocumentPath { get; set; }
        public IList<Awards> Awards { get; set; }
        public long IdAward { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AwardType AwardType { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]

        public DateTime OpeningDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm}")]

        public DateTime ClosingDate { get; set; }
        public List<CriteriaViewModel> criteriaViewModel { get; set; }
        public string SaveStatus { get; set; }
        public bool IsSubmitted { get; set; }
        public bool EnableSubmitChkBox { get; set; }
        public string Message { get; set; }
        public long AssignedUserId { get; set; }
        public long QsmEntryId { get; set; }

        public IList<AwardsType> AwardsType { get; set; }
        public long CurrentStatus { get; set; }

        public string ButtonClickStatus { get; set; }


    }
    public class CriteriaViewModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public List<SubCriteriaViewModel> subCriteriaViewModel { get; set; }
    }


    public class SubCriteriaViewModel
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public short Marks { get; set; }
        public short MaxWords { get; set; }
        //public string? Answer { get; set; }
        //public string? Comments { get; set; }
        //public string? CommentsDetail { get; set; }
        public string Answer { get; set; }
        public string Comments { get; set; }
        public string CommentsDetail { get; set; }

        public long CategoryCriteriaId { get; set; }
        public IList<IFormFile> SubCategoryDocuments1 { get; set; }
        public IFormFile SubCategoryDocuments2 { get; set; }
        public string  SubCategoryDocuments1Path { get; set; }
        public string SubCategoryDocuments2Path { get; set; }



        //saad
    }
}
