namespace Award.Web.Common.Auth
{
    public interface IUserSession
    {
        string FirstName { get; }
        string FullName { get; }
        int Id { get; }
        bool IsAuthenticated { get; }
        string LastName { get; }
        string UserName { get; }
        bool Isadmin { get; }
        bool IsQSM { get; }
        bool IsEndUser { get; }
        bool IsAuditManager { get; }
        bool IsAuditor { get; }
        bool IsJury { get; }
        bool IsSuperAdmin { get; }
        bool IsInRole(string roleName);
        public string CurrentProfileImage { get; set; }
        public string CurrentUserRole { get; set; }

    }
}