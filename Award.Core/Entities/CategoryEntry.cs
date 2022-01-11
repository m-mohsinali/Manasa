using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Award.Core.Entities
{
    public class CategoryEntry : BaseEntity
    {
        public long UserId { get; set; }
        public long CategoryId { get; set; }
        public long RoleId { get; set; }
        public long CurrentStatusId { get; set; }
        public string Comment { get; set; }
    }
    public class CategorySectorEntry : BaseEntity
    {
        public long CategoryId { get; set; }
        public long SectorId { get; set; }
        public long CurrentStatusId { get; set; }
        
    }
   
    public class TeamEntry : BaseEntity
    {
        public long CatigoryTeamEntryId { get; set; }

        public long AuditmanageruserId { get; set; }
        public long AuditoruserId { get; set; }
        public long CurrentStatusId { get; set; }

    }
    public class AuditingUserAnswers : BaseEntity
    {
        public long TeamEntryId { get; set; }
        public long SubCriteriaId { get; set; }
        public string Suggestion { get; set; }
        public string Strength { get; set; }
        public long CurrentStatusId { get; set; }
        public long Marks { get; set; }
        public long UserAnwserId { get; set; }



    }
    public class QSMEntries : BaseEntity
    {
        public long CategoryId { get; set; }
        public long SubmittedUserId { get; set; }
        public long AssignedUserId { get; set; }
        public long CurrentStatusId { get; set; }

    }

    public class UserAnswers : BaseEntity
    {
        public long QsmEntryId { get; set; }
        public long CategoryId { get; set; }
        public long CriteriaId { get; set; }
        public long SubCriteriaId { get; set; }
        public string Answer { get; set; }
        public long CurrentStatusId { get; set; }

    }
    public class UserAnswersComments : BaseEntity
    {
        public long UserAnswerId { get; set; }
        public string Comments { get; set; }
        public long CurrentStatusId { get; set; }
    }


    public class QsmSector : BaseEntity
    {
        public long SectorId { get; set; }
        public long UserID { get; set; }
        public long RoleId { get; set; }

    }
}
