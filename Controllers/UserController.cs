using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using beltReviewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using beltReviewer.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace beltReviewer.Controllers
{
    public class UserController : Controller
    {

        private readonly UserFactory userFactory;

        public UserController(UserFactory user) {
            userFactory = user;
        }

        // GET: /Home/
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
                return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                userFactory.Add(newUser);
                userFactory.FindbyEmail(newUser.email);
                var user = userFactory.FindbyEmail(newUser.email);
                HttpContext.Session.SetInt32("user_id", (int)user.id);
                HttpContext.Session.SetString("first_name", (string)user.first_name);
            return Redirect("groups");
            }
            else{
                return View("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RouteAttribute("login")]
        public IActionResult Login(User newUser)
        { 
            var user = userFactory.FindbyEmail(newUser.email);

            // if(ModelState.IsValid){
                if(user != null && newUser.password != null){
                    var Hasher = new PasswordHasher<User>();
                    if(0 != Hasher.VerifyHashedPassword(user, user.password, newUser.password)){
                        int user_id = (int)user.id;
                        HttpContext.Session.SetInt32("user_id", user_id);
                        return Redirect("groups");
                    }
                }
            // }

            TempData ["incorrect_login"] = "Incorrect Login Info";
           return RedirectToAction("Login");
        }
        
    }
}
