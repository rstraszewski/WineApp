using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Wine.Core.DataAccess;
using Wine.Core.Entities;
using Wine.Web.Models;

namespace Wine.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly WineDbContext _dbContext;

        public UserController()
        {
            _dbContext = new WineDbContext();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(NewUser newUser)
        {
            var user = new User(newUser.UserName, newUser.Email, newUser.Password);
            user.Roles.Add(_dbContext.Roles.First(x => x.Name == "Viewer"));
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return View("Failed");
            }

            return View("Success");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Unauthorized");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser newUser)
        {
            try
            {
                var credentials = _dbContext.Users.Where(x => x.Username == newUser.Username).Select(x => x.Credentials).First();

                var validatePassword = credentials.ValidatePassword(newUser.Password);

                if (validatePassword)
                {
                    FormsAuthentication.SetAuthCookie(newUser.Username, false);
                }
            }
            catch
            {
                return View("Failed");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var username = HttpContext.User.Identity.Name;
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "UserManager")]
        public ActionResult Manage()
        {
            var users = _dbContext.Users.Select(x => x.Username).ToList();

            return View(users);
        }

        [Authorize(Roles = "UserManager")]
        public ActionResult Roles(string userName)
        {
            var roles = _dbContext.Users.Include("Roles").First(x => x.Username == userName).Roles.Select(x => x.Name);
            //var dataSourceResult = roles.Select(x => x.Name).ToDataSourceResult(new DataSourceRequest());
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult SetRoles(List<string> roles, string userName)
        {
            var user = _dbContext.Users.Include("Roles").First(x => x.Username == userName);
            var rolesToGive = _dbContext.Roles.Where(x => roles.Contains(x.Name)).ToList();
            user.Roles = rolesToGive;
            _dbContext.SaveChanges();

            return Json("");
        }

        [Authorize(Roles = "UserManager")]
        public ActionResult AllRoles()
        {
            var roles = _dbContext.Roles;
            //var dataSourceResult = roles.Select(x => x.Name).ToDataSourceResult(new DataSourceRequest());
            return Json(roles, JsonRequestBehavior.AllowGet);
        }

        public new void Dispose()
        {
            _dbContext.Dispose();
            base.Dispose();
        }
    }

    public class LoginUser
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}