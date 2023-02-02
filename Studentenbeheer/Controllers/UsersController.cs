using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : ApplicationController
    {

       

        public UsersController(IdentityContext context,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
        }
        [AllowAnonymous]
        public IActionResult Index(string userName, string name, string email, int? pageNumber)
        {
            if (userName == null) userName = "";
            if (name == null) name = "";
            if (email == null) email = "";
            List<StudentenbeheerUser> users =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (name == "" || (u.FirstName.Contains(name) || u.LastName.Contains(name)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.LastName + " " + u.FirstName)
                .ToList();
            List<ApplicationUserViewModel> userViewModels = new List<ApplicationUserViewModel>();
            foreach (var user in users)
            {
                userViewModels.Add(new ApplicationUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Lockout = user.LockoutEnd != null,
                    PhoneNumber = user.PhoneNumber,
                    Admin = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Admin").Count() > 0,
                    Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                    Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = name;
            ViewData["email"] = email;
            if (pageNumber == null) pageNumber = 1;
            PaginatedList<ApplicationUserViewModel> model = new PaginatedList<ApplicationUserViewModel>(userViewModels, userViewModels.Count, 1, 10);
            return View(model);
        }

        public async Task<ActionResult> Locking(string id)
        {
            StudentenbeheerUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user.LockoutEnd != null)
                user.LockoutEnd = null;
            else
                user.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Roles(string id)
        {
            StudentenbeheerUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            ApplicationUserViewModel model = new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Lockout = user.LockoutEnd != null,
                PhoneNumber = user.PhoneNumber,
                Admin = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Admin").Count() > 0,
                Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles([Bind("Id, UserName, FirstName, LastName, User, Student, Docent")] ApplicationUserViewModel model)
        {
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == model.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }
            if (model.Admin) _context.Add(new IdentityUserRole<string> { RoleId = "User", UserId = model.Id });
            if (model.Student) _context.Add(new IdentityUserRole<string> { RoleId = "Student", UserId = model.Id });
            if (model.Docent) _context.Add(new IdentityUserRole<string> { RoleId = "Docent", UserId = model.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }
}
