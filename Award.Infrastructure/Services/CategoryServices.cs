using Award.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Award.Infrastructure.Services
{
   public class CategoryServices: ICategoryServices
    {
        private ICategoryRepository _categoryRepository;
        public CategoryServices(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public DataTable GetCategoryCount()
        {
            DataTable dt = _categoryRepository.GetCategoryCount();
            return dt;
        }

        public DataTable GetSectorsCount()
        {
            DataTable dt = _categoryRepository.GetSectorsCount();
            return dt;
        }

        public DataTable GetSubmissionFromQSM()
        {
            DataTable dt = _categoryRepository.GetSubmissionFromQSM();
            return dt;
        }
        public DataTable GetSubmissionFromJury()
        {
            DataTable dt = _categoryRepository.GetSubmissionFromJury();
            return dt;
        }
        

        public DataTable GetAuditManagerSubmission()
        {
            DataTable dt = _categoryRepository.GetAuditManagerSubmission();
            return dt;
        }
        
    }
}
