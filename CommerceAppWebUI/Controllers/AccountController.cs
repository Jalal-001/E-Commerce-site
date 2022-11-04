using CommerceAppWebUI.EmailServices;
using CommerceAppWebUI.Extensions;
using CommerceAppWebUI.Identity;
using CommerceAppWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace CommerceAppWebUI.Controllers
{
    [AutoValidateAntiforgeryToken]

    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {

            return View(new LoginModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "There is no such a user");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (! await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData.Put("message", new AlertMessage()
                {
                    AlertType="danger",
                    Title="Account Verify",
                    Message= "Please verify your account using the email link!"
                });
                
                return View(model);
            }

            if (result.Succeeded)
            {
                // null-dan ferqlidirse => HomePage
                return Redirect(model.ReturnUrl ?? "~/");
            }
            ModelState.AddModelError("", "Username or Password is wrong");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // generate token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = token
                });

                // send email
                string subject = $"Please follow the <a href='https://localhost:7043{url}'>link</a> to verify your account";
                string htmlMessage = "Please verify your account using the email link!";
                await _emailSender.SendEmailAsync(model.Email, htmlMessage, subject);

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View();

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        AlertType= "success",
                        Title= "Successfully verified",
                        Message= "Your account has been successfully verified"
                    });
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage()
            {
                AlertType = "danger",
                Title = "Could not be verified",
                Message = "Your account could not be verified!"
            });
            return View();
        }

        public  IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult>ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View();
                //message
            }
            var user=_userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return View();
                //message
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(await user);

            var url = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = token
            });

            // send email
            string subject = $" <a href='https://localhost:7043{url}'>Reset Password</a> ";
            string htmlMessage = "Please reset password using the email link!";
            await _emailSender.SendEmailAsync(email, htmlMessage, subject);

            TempData.Put("message", new AlertMessage()
            {
                AlertType = "success",
                Title = "Verification link",
                Message = "A verification link has been sent to your account!"
            });

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(int userId,string token)
        {
            if(userId == null || token == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = "Not found",
                    Message = "User not found!"
                });
            }
            var model = new ResetPasswordModel { Token = token };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>ResetPassword(ResetPasswordModel model)
        {
            var user = _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "danger",
                    Title = "not found",
                    Message = "User not found!"
                });
            }

            var result = await _userManager.ResetPasswordAsync(await user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
                TempData.Put("message", new AlertMessage()
                {
                    AlertType = "success",
                    Title = "Successfully updated",
                    Message = "Your password has been successfully updated"
                });
            }
            return View();
        }



        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
