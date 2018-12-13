﻿using BetterTeamsWebApp.Models.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using System.Web.Security;

namespace BetterTeamsWebApp.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Register()
        {
            var userToRegister = new RegisterVM();

            return View(userToRegister);
        }
        [HttpPost]
        public ActionResult Register(RegisterVM userToRegister)
        {
            User user = new User()
            {
                Active = true,
                Username=userToRegister.Username,
                Password = userToRegister.Password,
                Name =userToRegister.Name,
                Surname= userToRegister.Surname,
                Email= userToRegister.Email,
                Role= userToRegister.Role,
                DateOfBirth= userToRegister.DateOfBirth.ToString()
            };

            UserRepository reg = new UserRepository();
            reg.Add(user);
            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult Login(LoginVM userToLogin)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            UserRepository userRepo = new UserRepository();
            User user = new User();
            user=userRepo.GetByUsername(userToLogin.Username);

            if (user!=null && user.Password ==userToLogin.Password)
            {
                var ticket = new FormsAuthenticationTicket(version: 1,
                                   name: userToLogin.Username,
                                   issueDate: DateTime.Now,
                                   expiration: DateTime.Now.AddDays(30),
                                   isPersistent: userToLogin.RememberMe,
                                   userData: user.Role);
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                    encryptedTicket);

                HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return null;
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}