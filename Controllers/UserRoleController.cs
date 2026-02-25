using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaStore.Repositories;
using PizzaStore.ViewModels;

namespace PizzaStore.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly UserRoleRepo _userRepo;
        private readonly RoleRepo _roleRepo;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRoleController(UserRoleRepo userRepo, RoleRepo roleRepo,RoleManager<IdentityRole> roleManager)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> AllRoles()
        {
            var allRoles = await _roleRepo.GetAllRoles();

            return View(allRoles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(CreateRole));
            }
            var createRole = await _roleRepo.CreateRole(name);
            if (!createRole)
            {
                return View(nameof(CreateRole));
            }

            return RedirectToAction(nameof(AllRoles));
        }


        public async Task<IActionResult> DeleteRole()
        {
            var roles = await _roleRepo.GetAllRoles();
            return View(roles.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string name)
        {
            if (!ModelState.IsValid)
            {
                var roles = (await _roleRepo.GetAllRoles())?.ToList() ?? new List<RoleVM>();
                return View(roles); // pass a non-null model
            }

            var delete = await _roleRepo.DeleteRole(name);
            if (!delete)
            {
                var roles = (await _roleRepo.GetAllRoles()).ToList();
                return View(roles);
            }

            return RedirectToAction(nameof(AllRoles));
        }


        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            return View(users);
        }

        public async Task<IActionResult> GetUserRoles(string email)
        {
            var userRoles = await _userRepo.GetUserRoles(email);
            ViewBag.Email = email;
            return View(userRoles);
        }

        public async Task<IActionResult> CreateUserRole()
        {
            var users = await _userRepo.GetAllUsers();
            ViewBag.Users = new SelectList(users, "Email", "Email");

            var roles = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole(string email, string roleName)
        {
            var create = await _userRepo.CreateUserRole(email, roleName);

            if (!create)
            {
                var users = await _userRepo.GetAllUsers();
                ViewBag.Users = new SelectList(users, "Email", "Email");

                ViewBag.Roles = _roleManager.Roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    }).ToList();

                ModelState.AddModelError("", "Failed to assign role.");
                return View();
            }

            return RedirectToAction(nameof(GetUserRoles), new { email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRole(string email, string roleName)
        {
            var deleted = await _userRepo.DeleteUserRole(email, roleName);

            if (!deleted)
            {
                TempData["Error"] = "Failed to remove role.";
            }
            else
            {
                TempData["Success"] = "Role removed.";
            }

            return RedirectToAction(nameof(GetUserRoles), new { email });
        }
    }
}
