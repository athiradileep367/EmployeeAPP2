using BCrypt.Net;
using EmployeeAPP2.DataAccess;
using EmployeeAPP2.Models;
using System;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt;



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
                var existingUserMail = userDAL.GetUserByEmail(user.Email);
                if (existingUserMail != null)
                {
                    ModelState.AddModelError("", "User's Email already exists.");
                    return View(user);
                }


                //  Hash password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

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
        public ActionResult Login(LoginViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var user = userDAL.GetUserByEmail(model.Email);
                
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password,user.PasswordHash))
                {
                    FormsAuthentication.SetAuthCookie(user.Email, true);
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
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Please fill in all required fields." });
            }

            string email = User.Identity.Name; // email stored during login via FormsAuthentication

            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "User is not authenticated." });
            }

            bool isChanged = userDAL.ChangePasswordByEmail(email, model.CurrentPassword, model.NewPassword);

            if (isChanged)
            {
                return Json(new { success = true, message = "Password changed successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Current password is incorrect." });
            }
        }





    }
}
