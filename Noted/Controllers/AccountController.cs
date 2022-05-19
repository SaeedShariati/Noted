using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Noted.Models.Authentication;
using Noted.Models.ViewModels;
using Noted.Services;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Noted.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> UserManager;
        SignInManager<AppUser> SignInManager;
        IEmailSender EmailSender;
        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IEmailSender emailSender)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await SignInManager.SignOutAsync();
            return LocalRedirect(returnUrl ?? "/");
        }
        [HttpGet]
        public IActionResult Login()
        {
            UserLogin userLogin = new UserLogin();
            return View(userLogin);
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]UserLogin userLogin,[FromQuery]string ReturnUrl)
        {
            await SignInManager.SignOutAsync();
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByNameAsync(userLogin.UserName);
                if (user != null)
                {
                    var result = await 
                        SignInManager.PasswordSignInAsync(userLogin.UserName,userLogin.Password,true,true);
                    if (result.Succeeded)
                    {
                        return LocalRedirect(ReturnUrl ?? "/");
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordIncorrect", "Password Is Incorrect");
                    }    
                }
                else
                {
                    ModelState.AddModelError("UserNotFound", "a User with this Username is not found.");
                }
            }
            return View(userLogin);
        }
        public IActionResult SignUp()
        {
            UserSignIn userSignIn = new UserSignIn();
            return View(userSignIn);
        }
        //TODO: add captcha
        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm]UserSignIn userSignIn,[FromQuery] string returnUrl)
        {
            await SignInManager.SignOutAsync();//Just for safety

            if (userSignIn.Password != userSignIn.RepeatPassword)
            {
                ModelState.AddModelError("PasswordError", "the Password and Repeat Password fields do not match.");
            }
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser(userSignIn.UserName,userSignIn.Email);
                user.Id = Guid.NewGuid().ToString(); //Make a new Id
                var result = await UserManager.CreateAsync(user, userSignIn.Password);
                if(result.Succeeded)
                {
                    //user = await UserManager.FindByNameAsync(userSignIn.UserName);
                    await SendConfirmationEmail(user);
                    await SignInManager.PasswordSignInAsync(userSignIn.UserName, userSignIn.Password, true, false);
                    return LocalRedirect(returnUrl ?? "/"); //return to the original Url.
                }
                else
                {
                    foreach(var e in result.Errors)
                    {
                        ModelState.AddModelError(e.Code, e.Description);
                    }
                }
            }
            return View(userSignIn);
        }
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            AppUser appUser = await UserManager.FindByIdAsync(userId);
            
            if(appUser != null)
            {
                var result = await UserManager.ConfirmEmailAsync(appUser, token);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(appUser, true);
                    return LocalRedirect("/");
                }
                else
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError(e.Code, e.Description);
                }
            }
            return View();

        }
        public async Task SendConfirmationEmail(AppUser user)
        {
            if (!user.EmailConfirmed)
            {
                string token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                string confirmationLink = Url.Action("ConfirmEmail",
                      "Account", new
                      {
                          userid = user.Id,
                          token = token
                      },
                      protocol: HttpContext.Request.Scheme);
                await EmailSender
                    .SendEmailAsync(user.Email, "Confirm your account"
                    , "click this link: " + confirmationLink);
            }
        }
    }
}
