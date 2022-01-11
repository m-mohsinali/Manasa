using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;


namespace Award.Web.Common.Auth
{
    public class LdapSettings
    {
        public string ServerName { get; set; }

        public int ServerPort { get; set; }

        public string DomainDistinguishedName { get; set; }
    }
    public class LdapUser : IdentityUser
    {
        [NotMapped]
        public string ObjectSid { get; set; }

        [NotMapped]
        public string ObjectGuid { get; set; }

        [NotMapped]
        public string ObjectCategory { get; set; }

        [NotMapped]
        public string ObjectClass { get; set; }

        [NotMapped]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "You must enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        public string Name { get; set; }

        [NotMapped]
        public string CommonName { get; set; }

        [NotMapped]
        public string DistinguishedName { get; set; }

        [NotMapped]
        public string SamAccountName { get; set; }

        [NotMapped]
        public int SamAccountType { get; set; }

        [NotMapped]
        public string[] MemberOf { get; set; }
        [NotMapped]
        public string[] MemberOfNameOnly { get; set; }

        [NotMapped]
        public bool IsDomainAdmin { get; set; }

        [NotMapped]
        public bool MustChangePasswordOnNextLogon { get; set; }

        [NotMapped]
        public string UserPrincipalName { get; set; }

        [NotMapped]
        public string DisplayName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "You must enter your first name!")]
        public string FirstName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "You must enter your last name!")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{this.FirstName} {this.LastName}";

        [NotMapped]
        [Required(ErrorMessage = "You must enter your email address!")]
        [EmailAddress(ErrorMessage = "You must enter a valid email address.")]
        public string EmailAddress { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public string Phone { get; set; }

        [NotMapped]
        public LdapAddress Address { get; set; }

        public override string SecurityStamp => Guid.NewGuid().ToString("D");

        public override string UserName
        {
            get => this.Name;
            set => this.Name = value;
        }

        public override string NormalizedUserName => this.UserName;

        public override string NormalizedEmail => this.EmailAddress;

        public override string Id => Guid.NewGuid().ToString("D");

        public override string Email => this.EmailAddress;

        public string GROUP => this.SamAccountName;
    }

    public class LdapAddress
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
    }


    public class LdapAuthenticationService : ILdapAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly LdapSettings _ldapSettings;
        private readonly string[] _attributes =
        {
            "objectSid", "objectGUID", "objectCategory", "objectClass", "memberOf", "name", "cn", "distinguishedName",
            "sAMAccountName", "sAMAccountName", "userPrincipalName", "displayName", "givenName", "sn", "description",
            "telephoneNumber", "mail", "streetAddress", "postalCode", "l", "st", "co", "c"
        };

        public LdapAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
            _ldapSettings = _configuration.GetSection("Ldap").Get<LdapSettings>();
        }

        private ILdapConnection GetConnection(string username, string password)
        {
            string userDn = string.Format(_ldapSettings.DomainDistinguishedName, username);
            var ldapConnection = new LdapConnection() { SecureSocketLayer = false };

            //Connect function will create a socket connection to the server - Port 389 for insecure and 3269 for secure    
            ldapConnection.Connect(this._ldapSettings.ServerName, this._ldapSettings.ServerPort);
            //Bind function with null user dn and password value will perform anonymous bind to LDAP server 
            ldapConnection.Bind(userDn, password);

            return ldapConnection;
        }
        public bool ValidateUser(string username, string password)
        {

            string userDn = string.Format(_ldapSettings.DomainDistinguishedName, username);
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(_ldapSettings.ServerName, _ldapSettings.ServerPort);
                    connection.Bind(userDn, password);

                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                // Log exception
            }

            return false;
        }
        public LdapUser GetUser(string username, string password)
        {
            LdapUser user = null;

            var filter = "";// "(objectClass=*)";
            var searchBase = string.Format(_ldapSettings.DomainDistinguishedName, username);
            using (var ldapConnection = this.GetConnection(username, password))
            {
                var searchResult = ldapConnection.Search(
                                    searchBase,
                                    2,
                                    filter,
                                    this._attributes,
                                    false
                                    );

                foreach (var ldapEntry in searchResult)
                {
                    user = this.CreateUserFromAttributes(searchBase, ldapEntry.GetAttributeSet());
                    break;
                }

            }

            return user;
        }

        private LdapUser CreateUserFromAttributes(string distinguishedName, LdapAttributeSet attributeSet)
        {
            var ldapUser = new LdapUser
            {
                UserName = attributeSet.FirstOrDefault(a => a.Key.Equals("sn", StringComparison.InvariantCultureIgnoreCase)).Value?.StringValue,
                Name = attributeSet.FirstOrDefault(a => a.Key.Equals("cn", StringComparison.InvariantCultureIgnoreCase)).Value?.StringValue,
                EmailAddress = attributeSet.FirstOrDefault(a => a.Key.Equals("mail", StringComparison.InvariantCultureIgnoreCase)).Value?.StringValue,
                SamAccountName = attributeSet.FirstOrDefault(a => a.Key.Equals("sAMAccountName", StringComparison.InvariantCultureIgnoreCase)).Value?.StringValue,
            };
            return ldapUser;
        }

        public bool IsAuthenticated(string strDomain, string userName, string password, out string grp, out string email, params string[] activeDirectoryPathes)
        {
            string[] _activeDirectoryPaths = { };
            string Filter;
            string DisplayName;

            if (activeDirectoryPathes.Length > 0)
            {
                _activeDirectoryPaths = activeDirectoryPathes;
            }
            var ldapPath = _configuration["ADConnection"];
            string ErrorMsg = "";


            grp = email = string.Empty;
            //#if DEBUG
            //grp = "ml409";
            //email = "ahmedal@ddd.com";
            //            return true;
            //#endif

            if (_activeDirectoryPaths.Length <= 0)
            {
                ErrorMsg = "The LDAP Path not supplied";
                return false;
            }

            foreach (var path in _activeDirectoryPaths)
            {
                var domainUserName = strDomain + @"\" + userName;
                var directoryEntry = new DirectoryEntry(path, domainUserName, password);
                try
                {
                    // Bind to the native AdsObject to force authentication.
                    var search = new DirectorySearcher(directoryEntry) { Filter = "(SAMAccountName=" + userName + ")" };
                    search.PropertiesToLoad.Add("cn");
                    search.PropertiesToLoad.Add("sAMAccountName");
                    search.PropertiesToLoad.Add("mail");
                    SearchResult result = search.FindOne();
                    if (result == null)
                    {
                        search = new DirectorySearcher(directoryEntry) { Filter = "(SAMAccountName=" + domainUserName + ")" };
                        search.PropertiesToLoad.Add("cn");
                        search.PropertiesToLoad.Add("sAMAccountName");
                        search.PropertiesToLoad.Add("mail");
                        result = search.FindOne();
                        if (result == null)
                        {
                            return false;
                        }

                    }
                    // Update the new path to the user in the directory
                    //m_strADPath1 = result.Path;
                    //sAMAccountName

                    Filter = (string)result.Properties["cn"][0];
                    DisplayName = (string)result.Properties["cn"][0];
                    if (result?.Properties?.PropertyNames != null)
                    {
                        //SLogger.Info("Inside Property");
                        foreach (string propertyName in result.Properties.PropertyNames)
                        {
                            var valueCollection = result.Properties[propertyName];
                            foreach (var propertyValue in valueCollection)
                            {
                                if (string.Equals(propertyName, "sAMAccountName", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    grp = propertyValue.ToString();
                                }
                                else if (string.Equals(propertyName, "mail", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    email = propertyValue.ToString();
                                }
                                //SLogger.Info("Property: " + propertyName + ": " + propertyValue.ToString());
                                //Debug.WriteLine("Property: " + propertyName + ": " + propertyValue.ToString());
                            }
                        }
                    }

                    //grp = (string)result.Properties["sAMAccountName"][0];

                }
                catch (Exception Dex)
                {
                    throw Dex;
                    ErrorMsg = "Invalid username or password";//Dex.Message;
                    return false;
                }
            }
            return true;
        }

    }
}
