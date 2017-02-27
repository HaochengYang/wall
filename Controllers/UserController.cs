using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wall.Models;
using wall.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace wall.Controllers
{
    public class UserController : Controller
    {
        private readonly UserFactory _UserFactory;

        public UserController(UserFactory User){
            _UserFactory = User;
        }

        
        [HttpGet]
        [Route("")]
        public IActionResult LoginPage()
        {
            ViewBag.Errors = "";
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string username, string password)    
        {
            if(username != null){
            User CheckUser = _UserFactory.GetUserByUsername(username);

            if(CheckUser != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(CheckUser,CheckUser.password,password)){
                  HttpContext.Session.SetInt32("CurrUserId", CheckUser.Userid);
                  return RedirectToAction("dashbord","Message");
                }
               }
            }
            ViewBag.Errors = "";
            return View("LoginPage");
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult RegisterPage()
        {
            ViewBag.Errors = "";
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegUser newUser)
        {
            if(ModelState.IsValid){
                PasswordHasher<RegUser> Hasher = new PasswordHasher<RegUser>();
                newUser.password = Hasher.HashPassword(newUser, newUser.password);
                User user = new User
                {
                    firstname = newUser.firstname,
                    lastname = newUser.lastname,
                    email = newUser.email,
                    username = newUser.username,
                    password = newUser.password
                };

                _UserFactory.Add(user);
                User CurrentUser = _UserFactory.GetLatestUser();
                HttpContext.Session.SetInt32("CurrUserId", CurrentUser.Userid);
                return RedirectToAction("dashbord","Message");
            }
            ViewBag.Errors = ModelState.Values;
            return View("RegisterPage");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout(){
        HttpContext.Session.Clear();
        return RedirectToAction("LoginPage");
        }
    }
}
