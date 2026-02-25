using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Data;
using PizzaStore.ViewModels;

namespace PizzaStore.Repositories
{
    public class RoleRepo
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleRepo(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleVM>> GetAllRoles()
        {
            var allRoles = await _roleManager.Roles.Select(rs => new RoleVM
            {
                Name = rs.Name
            }).ToListAsync();

            return allRoles;
            
        }

        public async Task<bool> CreateRole(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            var itIsExist = await _roleManager.RoleExistsAsync(name);
            if (itIsExist)
            {
                return false;
            }

            var createResult = await _roleManager.CreateAsync(new IdentityRole(name));
            return createResult.Succeeded;
        }
        
        public async Task<bool> DeleteRole(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if(role == null)
            {
                return false;
            }
            var delete = await _roleManager.DeleteAsync(role);
            return delete.Succeeded;
        }
    }
}
