using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Award.Core.Interfaces
{
   public interface ICategoryServices
    {
        public DataTable GetCategoryCount();
        public DataTable GetSectorsCount();
        public DataTable GetSubmissionFromQSM();
        public DataTable GetAuditManagerSubmission();
        public DataTable GetSubmissionFromJury();



    }
}
