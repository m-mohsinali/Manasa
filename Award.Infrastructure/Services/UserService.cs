using Award.Core.Common;
using Award.Core.Entities;
using Award.Core.Interfaces;
using Award.Infrastructure.Data;
using Award.Infrastructure.Data.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Award.Infrastructure.Services

{

    public class UserService : IUserService

    {

        private readonly AwardDbContext _dbContext;

        private readonly IEmployeeService _employeeService;

        private readonly IManasaEmployeeRepository _manasaEmployeeRepository;

        private readonly IAsyncRepository<User> _userRepository;

        private readonly IAsyncRepository<Role> _roleRepository;

        private readonly IAsyncRepository<UserRole> _userRoleRepository;

        private readonly IConfiguration _configuration;

        private string _connectionString => _configuration.GetConnectionString("AwardDbContextConnection");



        public UserService(AwardDbContext dbContext, IEmployeeService employeeService, IConfiguration configuration, IManasaEmployeeRepository manasaEmployeeRepository, IAsyncRepository<User> userRepository, IAsyncRepository<Role> roleRepository, IAsyncRepository<UserRole> userRoleRepository)

        {

            _dbContext = dbContext;

            _employeeService = employeeService;

            _configuration = configuration;

            _manasaEmployeeRepository = manasaEmployeeRepository;

            _userRepository = userRepository;

            _roleRepository = roleRepository;

            _userRoleRepository = userRoleRepository;

        }



        public async Task<User> CreateAndGetUserAsync(string userName, string email)

        {

#if DEBUG

            // userName = "m1456";

#endif

            try

            {

                User user = null;

                var employee = await _dbContext.Employee.FirstOrDefaultAsync(e => e.UserDomain == userName)

                    .ConfigureAwait(false);



                if (employee != null)

                {

                    user = await CreateAndGetUserWithRoles(employee, email).ConfigureAwait(false);



                    return user;

                }



                var manasaEmployee = _manasaEmployeeRepository.GetByUsername(userName, email);



                if (manasaEmployee == null)

                {

                    return null;

                }

                employee = await _employeeService.CreateEmployee(manasaEmployee);

                _dbContext.Add(employee);

                var success = await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                if (success <= 0)

                {

                    return null;

                }



                user = await CreateAndGetUserWithRoles(employee, email).ConfigureAwait(false);



                return user;

            }

            catch (Exception ex)

            {



                throw ex;

            }

        }



        private async Task<User> CreateAndGetUserWithRoles(Employee employee, string email)

        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.UserName == employee.UserDomain);



            if (user == null)

            {

                var role = await GetAndCreateRole(Constants.Roles.END_USER);



                user = new User

                {

                    UserName = employee.UserDomain,

                    Email = email,

                    FirstName = employee.NameAr,

                    IdEmployee = employee.Id

                };





                _dbContext.Users.Add(user);

                var result = await _dbContext.SaveChangesAsync().ConfigureAwait(false);



                var userRole = new UserRole

                {

                    UserId = user.Id,

                    RoleId = 3,

                    CreateDate = DateTime.Now,

                    Id = 1

                };





                _dbContext.UserRoles.Add(userRole);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            }





            return user;

        }



        private async Task<Role> GetAndCreateRole(string roleName)

        {

            var role = await _dbContext.Roles.FirstOrDefaultAsync(a => a.Name == roleName);

            if (role == null)

            {

                role = new Role() { Name = Constants.Roles.END_USER };

                _dbContext.Roles.Add(role);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            }
            return role;
        }
        public async Task<User> GetUserByUserIdAsync(long id)
        {
            if (id <= 0)
            {
                return null;
            }
            return await _dbContext.Users.Include(u => u.Employee).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<User> GetUserByUserNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            return await _dbContext.Users.Include(u => u.Employee).Include(u => u.Employee.Department).Include(u => u.Employee.Section).FirstOrDefaultAsync(a => a.Id == long.Parse(username));
        }
        public async Task AddUserRoleAsync(long userId, string roleName)
        {
            var role = await GetAndCreateRole(roleName);
            await _userRoleRepository.AddAsync(new UserRole { UserId = userId, RoleId = role.Id });
        }
        public async Task<User> GetUserRolesAsync(long userId)
        {
            var userRolesList = await _dbContext.Users.Include(a => a.UserRoles).ThenInclude(b => b.Role).SingleOrDefaultAsync(u => u.Id == userId);
            return userRolesList;
        }
        public async Task<IReadOnlyList<User>> GetUserRolesListAsync()
        {
            //var /*userRolesList*/ = await _dbContext.Users.Include(a => a.UserRoles).ThenInclude(b => b.Role).ToListAsync();

            //var userRolesListNew = await _dbContext.Users.Include(a => a.Employee).ThenInclude(b => b.Sector).ToListAsync();


            //Course course = db.Courses
            //    .Include(i => i.Modules.Select(s => s.Chapters))
            //    .Include(i => i.Lab)
            //    .Single(x => x.Id == id);


            var userRolesList = await _dbContext.Users
                .Include(i => i.Employee).ThenInclude(s=>s.Sector)
                .Include(i => i.UserRoles).ThenInclude(ur=>ur.Role)
                .ToListAsync();


            //var res = (from u in _dbContext.Users
            //           join e in _dbContext.Employee on u.IdEmployee equals e.Id
            //           join s in _dbContext.Sectors on e.SectorId equals s.Id
            //           join ur in _dbContext.UserRoles on u.Id equals ur.RoleId
            //           join r in _dbContext.Roles on ur.RoleId equals r.Id
            //           select new
            //           {
            //               u.UserName,
            //               e.NameEn,
            //               e.NameAr,
            //               SectorNameEn = s.NameEn,
            //               SectorNameAr = s.NameAr,
            //               r.Name

            //           }).ToList();

            //foreach (var data in res)
            //{
            //    var users = new List<User>();
            //    users.Add(new User { UserName =data.UserName,Employee.});
            //}
            return userRolesList;
        }



        public async Task<User> DeleteUserRolesAsync(long userId)
        {
            var user = await GetUserRolesAsync(userId);
            user.UserRoles = null;
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<List<Role>> GetAllRolesAsync()
        {
            var rolesList = await _dbContext.Roles.ToListAsync();
            return rolesList;
        }
        public async Task<User> EditUserRolesAsync(long userId, List<Role> newUserRoles)
        {

            //var user = await GetUserRolesAsync(userId);
            //_dbContext.UserRoles.RemoveRange(user.UserRoles);
            //await _dbContext.SaveChangesAsync();
            //foreach (var data in newUserRoles)
            //{
            //    await AddUserRoleAsync(userId, data.Name.ToString());
            //}
            var user = await GetUserRolesAsync(userId);
            _dbContext.UserRoles.RemoveRange(user.UserRoles);
            if (newUserRoles.Count > 0)
            {
                var userRoles = newUserRoles.Select(a => new UserRole { RoleId = a.Id, UserId = user.Id, CreateDate = DateTime.Now, Id = 1 });
                _dbContext.UserRoles.AddRange(userRoles);
            }
            await _dbContext.SaveChangesAsync();
            return user;
        }
        public async Task<IReadOnlyList<User>> GetAllUsersWithRoleNameAsync(string roleName)
        {
            var users = await _dbContext.Users.Where(a => a.UserRoles.Any(b => b.Role.Name == roleName)).ToListAsync();
            return users;
        }

        public async Task<IReadOnlyList<User>> GetAllUsersWithRoleNameandSectorIDAsync(string roleName ,long sectorId)
        {


            var users = await _dbContext.Users .Include(d => d.Employee)
 //.ThenInclude(d => d.)
 .Where(d => d.Employee.SectorId == sectorId).Where(a => a.UserRoles.Any(b => b.Role.Name == roleName)).ToListAsync();
 //.FirstOrDefaultAsync(d => d.Id == id);



            return users;





        }
        public async Task<IReadOnlyList<User>> GetAllUsersWithRoleNameAsyncByIds(string roleName,string [] ids)
        {
            if (ids == null)
            {
            var users = await _dbContext.Users.Where(a => a.UserRoles.Any(b => b.Role.Name == roleName)).ToListAsync();
                return users;
            }
            else 
            { 
            var users = await _dbContext.Users.Where(a => a.UserRoles.Any(b => b.Role.Name == roleName) && ids.Contains(a.Id.ToString())).ToListAsync();
                return users;
            }
        }
        public Teams GetCategoryTeamFromCategoryId(long categoryId)
        {

            //var categoryTeam = _dbContext.CategoryTeams.FirstOrDefault(a => a.TeamId  == categoryId);//Need to Check Saad

            //return GetCategoryTeam(categoryTeam?.TeamId ?? 0).Result;

            return null;

        }



        public async Task<Teams> GetCategoryTeam(long teamId)

        {

            if (teamId <= 0)

            {

                return null;

            }



            var team = await _dbContext.Teams.FirstOrDefaultAsync(a => a.Id == teamId);

            team.CategoryTeams = team != null ? _dbContext.CategoryTeamEntries.Where(a => a.TeamId == teamId).ToList() : null;

            return team;

        }



        public async Task<Teams> CreateOrEditCategoryTeam(Teams team)

        {

            var oldTeam = await this.GetCategoryTeam(team.Id);



            if (oldTeam == null && (team?.CategoryTeams?.Count() ?? 0) > 0)

            {

                team.Name = _dbContext.Categories.First(a => a.Id == team.CategoryTeams.First().TeamId).Name + " - Group";//Need to Check Saad

                var categoryTeam = team.CategoryTeams;

                team.CategoryTeams = null;

                await _dbContext.Teams.AddAsync(team);

                await _dbContext.SaveChangesAsync();

                categoryTeam?.ToList().ForEach(a => a.TeamId = Convert.ToInt32(team.Id));

                _dbContext.CategoryTeamEntries.AddRange(categoryTeam);

                await _dbContext.SaveChangesAsync();

            }

            else

            {

                _dbContext.CategoryTeamEntries.RemoveRange(oldTeam.CategoryTeams);

                if (team.CategoryTeams == null || team.CategoryTeams.Count() == 0)

                {

                    _dbContext.Teams.Remove(oldTeam);

                }

                else

                {

                    _dbContext.CategoryTeamEntries.AddRange(team.CategoryTeams);

                }

                await _dbContext.SaveChangesAsync();

            }





            return team;

        }



        public async Task<bool> DeleteCategoryTeam(Teams team)

        {

            var oldTeam = await this.GetCategoryTeam(team.Id);



            if (oldTeam != null)

            {

                if (oldTeam.CategoryTeams != null && oldTeam.CategoryTeams.Count() > 0)

                {

                    _dbContext.CategoryTeamEntries.RemoveRange(oldTeam.CategoryTeams);

                }

                _dbContext.Teams.Remove(oldTeam);

                await _dbContext.SaveChangesAsync();

            }





            return true;

        }



        public async Task AssingQualitySectorManager(long sectorId, List<User> newQualitySectionManagerUsersList)

        {

            var oldQsmList = _dbContext.QualitySectorManagers.Where(u => u.SectorId == sectorId);

            _dbContext.QualitySectorManagers.RemoveRange(oldQsmList);



            if (newQualitySectionManagerUsersList.Count > 0)

            {

                var qsmList = newQualitySectionManagerUsersList.Select(a => new QualitySectorManager { SectorId = sectorId, UserId = a.Id });

                _dbContext.QualitySectorManagers.AddRange(qsmList);

            }



            await _dbContext.SaveChangesAsync();

        }



        public async Task<Teams> CreateTeam(Teams team)

        {

            team.CreateDate = DateTime.Now;

            await _dbContext.Teams.AddAsync(team);

            await _dbContext.SaveChangesAsync();



            return team;

        }
        public async Task<List<JuryTeam>> CreateJuryTeam(List<JuryTeam> NewJuryTeam)
        {

            var oldJuryList = _dbContext.JuryTeam;
            _dbContext.JuryTeam.RemoveRange(oldJuryList);

            if (NewJuryTeam.Count > 0)
            {
                //var qsmList = newQualitySectionManagerUsersList.Select(a => new QualitySectorManager { SectorId = sectorId, UserId = a.Id });
                _dbContext.JuryTeam.AddRange(NewJuryTeam);
            }
            await _dbContext.SaveChangesAsync();

            return NewJuryTeam;

        }
        

        public async Task<Teams> EditEmployeesTeam(Teams team)
        {
            try
            {
                var oldTeam = _dbContext.Teams.Include(a => a.EmployeeTeams).Single(a => a.Id == team.Id);
                oldTeam.Name = team.Name;
                _dbContext.EmployeeTeams.RemoveRange(oldTeam.EmployeeTeams);
                if (team.EmployeeTeams != null && team.EmployeeTeams.Count() > 0)
                {
                    _dbContext.EmployeeTeams.AddRange(team.EmployeeTeams);
                }
                await _dbContext.SaveChangesAsync();
                return team;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            

        }



        public async Task<bool> DeleteTeam(long teamId)

        {

            var oldTeam = _dbContext.Teams.Find(teamId);

            _dbContext.Teams.Remove(oldTeam);

            await _dbContext.SaveChangesAsync();



            return true;

        }
        public async Task<object> UploadBulkUserInDB(string logfilepath)
        {
            try
            {
                 var data =_manasaEmployeeRepository.UpdateBulkData(logfilepath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public async Task<User> InsertBulkUserInDB()
        {
            try
            {
                //using (var searcher = new PrincipalSearcher(new UserPrincipal(new PrincipalContext(ContextType.Domain, Environment.UserDomainName))))
                {

                    var searcher = new PrincipalSearcher(new UserPrincipal(new PrincipalContext(ContextType.Domain, Environment.UserDomainName)));
                    List<UserPrincipal> users = searcher.FindAll().Select(u => (UserPrincipal)u).ToList();
                    //users = users.Where(u => u.EmailAddress != null).ToList();
                    //users = users.Where(u => u.SamAccountName != null).ToList();
                    foreach (var u in users)
                    {
                        DirectoryEntry d = (DirectoryEntry)u.GetUnderlyingObject();
                        var fullName = d.Properties["GivenName"]?.Value?.ToString() + d.Properties["sn"]?.Value?.ToString();
                        //var testemail = d.Properties["Mail"]?.Value?.ToString() + d.Properties["sn"]?.Value?.ToString();
                        var testEmail = u.EmailAddress;
                        var test = u.UserPrincipalName;
                        if (test == "c6068@dnrd.gov.ae")
                        {
                            string trt = "";
                        }
                        if (u.SamAccountName == "c5556" || u.SamAccountName == "c6061" || u.SamAccountName == "c6068")
                        {
                            string stre = "";
                        }
                        if (u.EmailAddress != null && u.SamAccountName != null)
                        {
                            var manasaEmployee = _manasaEmployeeRepository.GetByUsername(u.SamAccountName, u.EmailAddress);
                            if (manasaEmployee != null)
                            {
                                var success = 1;// await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                                if (success <= 0)
                                {
                                    return null;
                                }
                            }                            
                        }
                        else
                        {
                            string testEmailNEW = u.EmailAddress;
                            string TESTusernamr04u = u.SamAccountName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private Employee GetByUsername(string username)

        {

            Employee employee = null;



            using (var conn = new SqlConnection(_connectionString))

            {

                string sql = "SELECT TOP 1 * FROM award_.award.Employee WHERE UserDomain = @username";

                var employeesList = conn.Query<Employee>(sql, new { username });

                employee = employeesList != null && employeesList.Count() > 0 ? employeesList.FirstOrDefault() : null;

            }



            return employee;

        }

    }

}