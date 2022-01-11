using Award.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Award.Web.Models
{
    public class TeamMemberViewModel
    {
        public bool IsSelected { get; set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public string Status { get; set; }
        public long CategoryTeamsId { get; set; }
        public long TeamId { get; set; }
        public string CatName { get; set; }
        public string Auditor { get; set; }
        public string AuditManager { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string Award { get; set; }
        public long AuditorId { get; set; }
        public long CatId { get; set; }
        public long TotalMarks { get; set; }

        public long AwardId { get; set; }
        public long AlreadyAnnounced { get; set; }
        public long RoleId { get; set; }

    }
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            AuditManagerUsersList = new List<TeamMemberViewModel>();
            AuditorUsersList = new List<TeamMemberViewModel>();
            JuryUserList = new List<TeamMemberViewModel>();

            AuditManagerUsersIdList = new List<long>();
            AuditorUsersIdList = new List<long>();
            JuryUserIdList = new List<long>();
        }

        public long TeamId { get; set; }
        public string TeamName { get; set; }
        public long CategoryId { get; set; }
        
        public List<TeamMemberViewModel> AuditManagerUsersList { get; set; }
        public List<TeamMemberViewModel> AuditorUsersList  { get; set; }
        public List<TeamMemberViewModel> JuryUserList { get; set; }

        public List<long> AuditManagerUsersIdList { get; set; }
        public List<long> AuditorUsersIdList { get; set; }
        public List<long> JuryUserIdList { get; set; }
    }

    public class AuditingUserAnswerViewModel
    {
        public string Description { get; set; }
        public int MaxMarks { get; set; }
        public string  Answer { get; set; }
        public long  Marks { get; set; }
        public string Strength { get; set; }
        public string Suggestion { get; set; }
        public long  SubCriteriaId { get; set; }
        public long TeamEntryId { get; set; }
        public string Document1 { get; set; }
        public string Document2 { get; set; }
        public long CatId { get; set; }
        public long QsmEntryId { get; set; }
        public string Criteria { get; set; }
        public string Comments { get; set; }
        public string TotalMarks { get; set; }
        public long UserAnswerId { get; set; }


        //subCiteria =csc.Description ,
        //                      maxMarks =csc.Marks ,
        //                      Answer=ua.Answer ,
        //                      Marks=aua.Marks.ToString() ==null ?0 :aua.Marks ,
        //                      Strength=aua.Strength ==null ?"" :aua.Strength ,
        //                      Suggestion=aua.Suggestion ==null ? "":aua.Suggestion ,
        //                      SubCriteriaId =csc.Id

    }
}
