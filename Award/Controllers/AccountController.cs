using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Infrastructure.Data.Repositories;
using Award.Infrastructure.Services;
using Award.Web.Common.Auth;
using Award.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Award.Web.Controllers
{
    //[Authorize]
    public class AccountController : BaseController
    {
        private readonly string _rootDomain = "dnrd_root";
        private readonly ISignInManager _signInManager;
        private readonly ILdapAuthenticationService _authService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly AwardDbContext _context;
        private readonly IStringLocalizer<CategoriesController> _localizer;


        public AccountController(
            ISignInManager signInManager,
            ILdapAuthenticationService authService,
            IConfiguration configuration,
            IUserService userService,
            AwardDbContext context, IStringLocalizer<CategoriesController> localizer) : base(context)
        {
            _context = context;
            this._signInManager = signInManager;
            this._authService = authService;
            this._configuration = configuration;
            this._userService = userService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<IActionResult> Signin(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin(SigninViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            var ldapPath = _configuration["ADConnection"];
            var environment = _configuration["Environment"];
            var dataInsertRequired = _configuration["DataInsertRequired"];
            var isLoginWithoutAuthintication = _configuration["LoginWithoutAuthintication"];
            var loggedIn = _configuration["loggedIn"];

            //loggedIn


            var isAuthenticated = false;
            long roleId = 0;
            var email = "";
            if (dataInsertRequired == "true")
            {
                // var InsertAllUsersFromDNRDToAwardDb = _userService.InsertBulkUserInDB();

            }


            if (this.ModelState.IsValid)
            {
                try
                {
                    if (loggedIn == "true" )
                    {

                        DateTime dt = new DateTime(2021, 09, 02);

                        if (DateTime.Now < dt)
                        {
                            if (isLoginWithoutAuthintication == "true")
                            {
                                isAuthenticated = true;
                            }
                            else if (environment == "Local")
                            {
                                isAuthenticated = _authService.ValidateUser(model.UserName, model.Password);
                            }
                            else
                            {
                                isAuthenticated = _authService.IsAuthenticated(_rootDomain, model.UserName, model.Password, out string grp, out string emailLdap, ldapPath);
                                email = emailLdap;
                            }

                        }
                      
                    }
                    else
                    {
                        if (isLoginWithoutAuthintication == "true")
                        {
                            isAuthenticated = true;
                        }
                        else if (environment == "Local")
                        {
                            isAuthenticated = _authService.ValidateUser(model.UserName, model.Password);
                        }
                        else
                        {
                            isAuthenticated = _authService.IsAuthenticated(_rootDomain, model.UserName, model.Password, out string grp, out string emailLdap, ldapPath);
                            email = emailLdap;
                        }
                    }



                   

                    if (isAuthenticated)
                    {

                        var ldapUser = new LdapUser();
                        ldapUser.UserName = model.UserName;
                        ldapUser.Email = email;

                        var user = await _userService.CreateAndGetUserAsync(model.UserName, email);
                        if (user != null)
                        {
                            ldapUser.EmailAddress = email;

                            //var query = from userrol in _context.UserRoles
                            //            join rol in _context.Roles on userrol.UserId equals rol.Id
                            //            select new { UserRole = userrol, Role = rol };
                            // var rolList = query.Select(x => x.Role.Name).ToList();

                            //var userRolesNamesList = user.UserRoles != null && user.UserRoles.Count() > 0 ? user.UserRoles.Select(a => _context.Roles.Find(a.RoleId).Name).ToList() : new List<string>();



                            //if (userRolesNamesList.Count == 1)
                            //{ 

                            //}

                            //if (userRolesNamesList.Count > 1)
                            //{
                            //    var userRolesList = new List<UserRoles>();

                            //    for (var i = 0; i < userRolesNamesList.Count; i++)
                            //    {
                            //        userRolesList.Add(new UserRoles { RoleName = userRolesNamesList[i].ToString() });

                            //    }
                            //    return View("~/Views/Home/LoggedInUserRoles.cshtml", userRolesList);

                            //}
                            var roles = _context.UserRoles
                                .Where(d => d.UserId == user.Id).ToList();

                            if (roles.Count == 1)
                            {
                                roleId = roles.FirstOrDefault().RoleId;
                            }
                            else
                            {
                                var userRolesList = new List<UserRoles>();

                                //Saad
                                foreach (var data in roles)
                                {
                                    userRolesList.Add(new UserRoles { RoleId = data.RoleId, UserId = user.Id, RoleName = _localizer[_context.Roles.Where(r => r.Id == data.RoleId).FirstOrDefault().Name], UserName = model.UserName, Email = email });

                                }
                                return View("~/Views/Home/LoggedInUserRoles.cshtml", userRolesList);
                            }
                            await _signInManager.SignInAsync(ldapUser, _context.Roles.Where(r => r.Id == roles.FirstOrDefault().RoleId).FirstOrDefault().Name, user);

                        }
                        else
                        {
                            this.TempData["ErrorMessage"] = "The username and/or password are incorrect!";
                        }
                        UpdateLastLoginUser(roleId, 0, user.Id);
                        return this.RedirectToLocal(returnUrl, roleId);
                    }

                    // I added the exclamation mark to make it more dramatic
                    if (loggedIn == "true")
                    {
                        this.TempData["ErrorMessage"] = "Please contact MAIT team!";

                    }
                    else
                    {
                        this.TempData["ErrorMessage"] = "The username and/or password are incorrect!";

                    }

                        

                    return this.View(model);
                }
                catch (Exception ex)
                {
                    this.TempData["ErrorMessage"] = ex.Message;

                    return this.View(model);
                }
            }

            return this.View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Signin2(SigninViewModel model, string returnUrl = null)
        //{
        //    this.ViewData["ReturnUrl"] = returnUrl;

        //    if (this.ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var isAuthenticated = _authService.ValidateUser(model.UserName, model.Password);
        //            if (isAuthenticated)
        //            {
        //                var user = _authService.GetUser(model.UserName, model.Password);

        //                await _signInManager.SignInAsync(user, new List<string>(), null);

        //                return this.RedirectToLocal(returnUrl);
        //            }

        //            // I added the exclamation mark to make it more dramatic
        //            this.TempData["ErrorMessage"] = "The username and/or password are incorrect!";

        //            return this.View(model);
        //        }
        //        catch (Exception)
        //        {
        //            this.TempData["ErrorMessage"] = "Something bad happened while logging in...";

        //            return this.View(model);
        //        }
        //    }

        //    return this.View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signout()
        {
            HttpContext.Session.SetString("alreadyLoggedIn", "");
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            var user = await _userService.GetUserByUserNameAsync(CurrentUserId);
            if (user == null)
            {
                return RedirectToLocal("/home/opening");
            }

            return this.View(user);
        }
        [Authorize]
        public async Task<IActionResult> UserRoles()
        {
            var userRoles = await _userService.GetUserRolesListAsync();
            var userRolesVM = userRoles != null && userRoles.Count() > 0 ? userRoles.Select(a => new UserRolesViewModel { UserId = a.Id, Name = CurrentLanguage == "en" ? a.Employee.NameEn : a.Employee.NameAr, SectorName = CurrentLanguage == "en" ? a.Employee.Sector.NameEn : a.Employee.Sector.NameAr, UserName = a.UserName, UserRoles = a.UserRoles.Select(b => new RoleViewModel(b.Role)).ToList() }).ToList() : new List<UserRolesViewModel>();
            for (int i = 0; i < userRolesVM.Count; i++)
            {
                for (int j = 0; j < userRolesVM[i].UserRoles.Count; j++)
                {
                    userRolesVM[i].UserRoles[j].Name = _localizer[userRolesVM[i].UserRoles[j].Name];
                }
                 userRolesVM[i].RolesNameList= userRolesVM[i].UserRoles?.Count() > 0 ? string.Join(',', userRolesVM[i].UserRoles.Select(r => _localizer[r.Name]).ToList()) : null;
            }

            return this.View(userRolesVM);
        }
        [Authorize]
        public async Task<IActionResult> UserRolesDetails(long userId)
        {
            var user = await _userService.GetUserRolesAsync(userId);
            var userRolesVM = user != null ? new UserRolesViewModel() { UserId = user.Id, UserName = CurrentLanguage == "en" ? user.FirstName : user.LastName, UserRoles = user.UserRoles.Select(b => new RoleViewModel(b.Role)).ToList() } : new UserRolesViewModel();
            userRolesVM.RolesNameList= userRolesVM.UserRoles?.Count() > 0 ? string.Join(',', userRolesVM.UserRoles.Select(r => _localizer[r.Name]).ToList()) : null;


            return this.View(userRolesVM);
        }
        [Authorize]
        public async Task<IActionResult> UserRolesDelete(long userId)
        {
            var user = await _userService.GetUserRolesAsync(userId);
            var userRolesVM = user != null ? new UserRolesViewModel { UserId = user.Id, UserName = user.UserName, UserRoles = user.UserRoles.Select(b => new RoleViewModel(b.Role)).ToList() } : new UserRolesViewModel();
            return this.View(userRolesVM);
        }
        [Authorize]
        [HttpPost, ActionName("UserRolesDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRolesDeleteConfirmed(long userId)
        {
            await _userService.DeleteUserRolesAsync(userId);
            return RedirectToAction(nameof(UserRoles));
        }
        [Authorize]
        public async Task<IActionResult> UserRolesEdit(long userId)
        {
            var user = await _userService.GetUserRolesAsync(userId);
            var userRolesVM = user != null ? new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = CurrentLanguage == "en" ? user.FirstName : user.LastName,
                UserRoles = user.UserRoles.Select(b => new RoleViewModel(b.Role)).ToList(),
            } : new UserRolesViewModel();
            userRolesVM.AllRoles = _userService.GetAllRolesAsync().Result?.Select(a => new RoleViewModel(a, userRolesVM.UserRoles.Any(b => b.Name == a.Name))).ToList();
            for (int i = 0; i < userRolesVM.AllRoles.Count; i++)
            {
                userRolesVM.AllRoles[i].Name = _localizer[userRolesVM.AllRoles[i].Name];
            }

            return this.View(userRolesVM);
        }
        [Authorize]
        [HttpPost, ActionName("UserRolesEdit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRolesEdit(UserRolesViewModel userRole)
        {
            await _userService.EditUserRolesAsync(userRole.UserId, userRole.AllRoles?.Where(a => a.IsSeleclted).Select(a => new Role { Id = a.Id, Name =  a.Name }).ToList());
            return RedirectToAction(nameof(UserRoles));
        }
        private IActionResult RedirectToLocal(string returnUrl, long roleId = 0)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction(nameof(HomeController.Opening), "Home", new { roleId = roleId });
            }
        }

        public IActionResult UpdateLastLoginUser(long urole, long siginStatus = 0, long userId = 0, string userName = "", string email = "")
        {
            // Opening("1");
            try
            {
                var roleName = _context.Roles.Where(r => r.Id == urole).FirstOrDefault().Name;
                var isExist = _context.LastLogInUser.Where(x => x.UserId == userId).FirstOrDefault();
                if (isExist == null)
                {

                    var lastLogin = new LastLogInUser();
                    lastLogin.RoleId = urole;
                    lastLogin.UserId = userId;
                    lastLogin.RoleName = roleName;
                    lastLogin.CreateDate = DateTime.Now;
                    _context.Add(lastLogin);
                    _context.SaveChanges();

                }
                else
                {
                    isExist.RoleId = urole;
                    isExist.UpdatedDate = DateTime.Now;
                    isExist.RoleName = roleName;
                    //_context.AddAsync(isExist).ConfigureAwait(false);
                    _context.Update(isExist);
                    _context.SaveChanges();


                }
                //_context.SaveChangesAsync();



                //var Identity = HttpContext.User.Identity as ClaimsIdentity;
                //Identity.AddClaim(new Claim(ClaimTypes.Role, roleName));

                if (siginStatus == 1)
                {
                    var ldapUser = new LdapUser();
                    var user = new User();
                    ldapUser.UserName = userName;
                    ldapUser.Email = email;
                    user.Id = userId;
                    _signInManager.SignInAsync(ldapUser, roleName, user);
                    return RedirectToLocal(null, urole);
                }

                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [Authorize]
        public IActionResult SwitchUser()
        {
            // Opening("1");
            try
            {
                HttpContext.Session.SetString("alreadyLoggedIn", "");
                var roles = (from r in _context.UserRoles
                             join u in _context.Users on r.UserId equals u.Id
                             where u.Id == long.Parse(CurrentUserId)
                              select new { 
                              u.UserName,
                              u.Email,
                              Id=r.RoleId,
                              UserId=u.Id
                              }
                             
                             ).ToList();

                if (roles.Count == 1)
                {
                    return Redirect("~/Home/Opening");
                }
                else
                {
                    var userRolesList = new List<UserRoles>();
                    foreach (var data in roles)
                    {
                        userRolesList.Add(new UserRoles { RoleId = data.Id, UserId = data.UserId, RoleName = _localizer[_context.Roles.Where(r => r.Id == data.Id).FirstOrDefault().Name], UserName = data.UserName, Email = data.Email });
                    }
                    return View("~/Views/Home/LoggedInUserRoles.cshtml", userRolesList);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}