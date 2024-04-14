using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersRepo usersRepo = new UsersRepo();

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult UserRegisteration(User user)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if(usersRepo.RegisterUser(user, ref message))
                    return RedirectToAction("Login");
            }
            else
            {
                message = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            ModelState.AddModelError(string.Empty, message);
            return View("Register", user);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ProcessLogin(User model)
        {
            if (ModelState.IsValid)
            {
                var user = usersRepo.GetUserByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User Not Found. Please Register!");
                }
                else if (usersRepo.VerifyPassword(model.Password, user.Password))
                {
                    HttpContext.Session["UserID"] = user.UserID.ToString();
                    return RedirectToAction("Index", "ToDoList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password");
                }
            }
            else
            {
                string message = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                ModelState.AddModelError(string.Empty, message);
            }
            return View("Login", model);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}