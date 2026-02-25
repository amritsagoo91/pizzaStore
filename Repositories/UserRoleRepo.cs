using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Data;
using PizzaStore.ViewModels;

namespace PizzaStore.Repositories
{
    public class UserRoleRepo
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRoleRepo(UserManager<IdentityUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserRoleVM>> GetAllUsers()
        {
            var users = await _userManager.Users.Select(u => new UserRoleVM
            {
                Email = u.Email
            }).ToListAsync();

            return users;
        }


        public async Task<IEnumerable<UserRoleVM>> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Enumerable.Empty<UserRoleVM>();
            }

            var getUserRoles = await _userManager.GetRolesAsync(user);

            return getUserRoles.Select(rn => new UserRoleVM
            {
                RoleName = rn
            }).ToList();
        }

        public async Task<bool> IsUserAssigned(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var findUserRole = await _userManager.IsInRoleAsync(user, roleName);
            return findUserRole;
        }

        public async Task<bool> CreateUserRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return false;
            }
            var isAlreadyAssgined = IsUserAssigned(email, roleName);
            if (await isAlreadyAssgined)
            {
                return false;
            }

            var addUserRole = await _userManager.AddToRoleAsync(user, roleName);
            return addUserRole.Succeeded;
        }


        public async Task<bool> DeleteUserRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var deleteRole = await _userManager.RemoveFromRoleAsync(user, roleName);
            return deleteRole.Succeeded;
        }

    }
}
