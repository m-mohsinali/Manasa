namespace Award.Web.Common.Auth
{
    public interface ILdapAuthenticationService
    {
        LdapUser GetUser(string username, string password);
        bool ValidateUser(string username, string password);
        bool IsAuthenticated(string strDomain, string userName, string password, out string grp, out string email, params string[] activeDirectoryPathes);

    }
}