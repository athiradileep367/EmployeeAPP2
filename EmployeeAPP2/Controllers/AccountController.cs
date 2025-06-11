using BCrypt.Net;
using EmployeeAPP2.Models;
using System;
using System.Web.Mvc;
using EmployeeAPP2.DataAccess;


namespace EmployeeAPP2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserDAL userDAL = new UserDAL();

        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                //var existingUser = userDAL.GetUserByUsername(user.Username);
                //if (existingUser != null)
                //{
                //    ModelState.AddModelError("", "Username already exists.");
                //    return View(user);
                //}
                var existingUserMail = userDAL.GetUserByUsername(user.Email);
                if (existingUserMail != null)
                {
                    ModelState.AddModelError("", "User's Email already exists.");
                    return View(user);
                }


                //  Hash password
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                userDAL.Register(user);
                TempData["Message"] = "Registration successful. Please login.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userDAL.GetUserByUsername(model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    Session["Email"] = user.Email;
                    return RedirectToAction("Index", "Employee"); //  redirect to employee dashboard
                }
                else
                {
                    TempData["LoginError"] = "Invalid username or password.";
                }

            }

            return View(model);
        }

        // GET: /Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
