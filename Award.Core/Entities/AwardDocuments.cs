using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Entities
{
   public class AwardDocuments
    {
        public long Id { get; set; }
        public long IdAwards { get; set; }
        public string SupportingDocumentPath { get; set; }
        public string DocumentName { get; set; }

        public virtual Awards Awards { get; set; }
    }

    public class CriteriaDocument : BaseEntity
    {
        public long QSMEntryId { get; set; }
        public string SupportingDocumentPath { get; set; }
        public long CurrentStatusId { get; set; }
        public long SubCriteriaId { get; set; }
        public long DocumentNo { get; set; }
        public string DocumentName { get; set; }



    }
}
