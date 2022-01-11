using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Award.Core.Entities;

namespace Award.Web.Common.Auth
{
    public class SignInManager : ISignInManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignInAsync(LdapUser user, string roleNames, User sysUser)
        {

            try
            {
                string sysUserId = sysUser.Id.ToString();
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, sysUserId ?? System.Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email==null?"abc@gamil.com":user.Email),
                new Claim(ClaimTypes.Role , roleNames)

            };

                var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await _httpContextAccessor.HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
            //var claims = new List<Claim>
            //{
            //    //new Claim(ClaimTypes.NameIdentifier, user.UserName),
            //    new Claim(ClaimTypes.Sid, user.Id?.ToString()??System.Guid.NewGuid().ToString()),
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Email, user.Email),
            //    //new Claim(ClaimTypes.GivenName, user.Name),
            //    //new Claim(ClaimTypes.Surname, user.Name)
            //};

            //foreach (string roleName in roleNames)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, roleName));
            //}

            //var identity = new ClaimsIdentity(claims, "local", "name", "role");
            //var principal = new ClaimsPrincipal(identity);

            //await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //string sysUserId = sysUser.Id.ToString();
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Sid, sysUserId ?? System.Guid.NewGuid().ToString()),
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.Role , roleNames)

            //};
            //foreach(var r in roleNames)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, r));
            //}

          
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
