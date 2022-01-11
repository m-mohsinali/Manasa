using Award.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Award.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateAndGetUserAsync(string userName, string email);
        Task<User> GetUserByUserIdAsync(long id);
        Task<User> GetUserByUserNameAsync(string username);
        Task<User> GetUserRolesAsync(long userId);
        Task<IReadOnlyList<User>> GetUserRolesListAsync();
        Task<User> DeleteUserRolesAsync(long userId);
        Task<List<Role>> GetAllRolesAsync();
        Task<User> EditUserRolesAsync(long userId, List<Role> newUserRoles);
        Task<IReadOnlyList<User>> GetAllUsersWithRoleNameAsync(string roleName);
        Task<IReadOnlyList<User>> GetAllUsersWithRoleNameandSectorIDAsync(string roleName, long sectorId); 

        Task<Teams> GetCategoryTeam(long categoryId);
        Task<Teams> CreateOrEditCategoryTeam(Teams teamList);
        Teams GetCategoryTeamFromCategoryId(long categoryId);
        Task<bool> DeleteCategoryTeam(Teams team);
        Task AssingQualitySectorManager(long sectorId, List<User> newQualitySectionManagerUsersList);

        Task<Teams> CreateTeam(Teams team);
        Task<List<JuryTeam>> CreateJuryTeam(List<JuryTeam> team);

        Task<bool> DeleteTeam(long teamId);
        Task<Teams> EditEmployeesTeam(Teams team);
        Task<IReadOnlyList<User>> GetAllUsersWithRoleNameAsyncByIds(string roleName, string[] ids); 

        //Task<User> InsertBulkUserInDB();

    }
}