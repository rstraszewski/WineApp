using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
            user.Roles.Add(_dbContext.Roles.First(x => x.Name == "Standard"));

            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser newUser)
        {
            var credentials = _dbContext.Users.Where(x => x.Username == newUser.Username).Select(x => x.Credentials).First();
            var validatePassword = credentials.ValidatePassword(newUser.Password);

            if (validatePassword)
            {
                FormsAuthentication.SetAuthCookie(newUser.Username, false);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
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