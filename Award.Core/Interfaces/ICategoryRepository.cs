using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Award.Core.Interfaces
{
   public interface ICategoryRepository
    {
        DataTable GetCategoryCount();
        DataTable GetSectorsCount();
        DataTable GetSubmissionFromQSM();
        DataTable GetAuditManagerSubmission();
        DataTable GetSubmissionFromJury();


    }
}
