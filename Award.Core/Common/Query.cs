using System;
using System.Collections.Generic;
using System.Text;

namespace Award.Core.Common
{
    public class Query
    {
        public const string GetCategoryCountAwardWise = "Select  case when AwardType=1 then 'Individual' when AwardType =2 then 'GroupIndividual' else 'Corporate' end as Type ,TotalCategories,'' Color from(SELECT count(*) as TotalCategories,AwardType  FROM [award_].[award].[Categories]group by AwardType) a";
        public const string GetSectorCount = "SELECT nameEn , 1 Total, '' as color  FROM [award_].[award].Sectors(nolock)";
        public const string GetSubmissionFromQSM = "SELECT c.Name CatName ,a.Name AwardName,s.NameEn,s.NameAr,(select ur.FirstName   from [award_].[award].Users ur where ur.Id =q.AssignedUserId  ) AssignedUser,u.FirstName SubmittedUser  FROM [award_].[award].QSMEntries q  inner join [award_].[award].Categories c on q.CategoryId =c.Id   inner join [award_].[award].Awards a on c.IdAward  =a.Id   inner join [award_].[award].Users u on q.SubmittedUserId =u.Id  inner join [award_].[award].Employee e on u.idemployee =e.id inner join [award_].[award].Sectors s on e.sectorid =s.id";
        public const string GetAuditManagerSubmission = "select c.Name CatName,(select FirstName from [award_].[award].Users u where u.Id =te.AuditoruserId  )Auditor,u.FirstName as AuditManager,te.CreateDate SubmittedDate from [award_].[award].QSMEntries q  inner join  [award_].[award].Categories c on q.CategoryId = c.Id inner join [award_].[award].CategoryTeamEntries cte on q.Id   =cte.QsmEntryId   inner join [award_].[award].TeamEntry  te on cte.Id =te.CatigoryTeamEntryId inner join [award_].[award].Users u on te.AuditmanageruserId = u.id";
        public const string GetSubmissionFromJury = "select a.Name AwardName, c.Name CatName, e.NameEn as Winner,u.FirstName as Jury from [award_].[award].Winners w inner join [award_].[award].Categories(nolock) c on w.CategoryId =c.Id   inner join [award_].[award].Awards(nolock) a on w.AwardId =a.Id inner join [award_].[award].Employee(nolock) e on e.Id =EmployeeId inner join [award_].[award].Users(nolock) u on w.JuryId =u.Id";

        
    }
}
