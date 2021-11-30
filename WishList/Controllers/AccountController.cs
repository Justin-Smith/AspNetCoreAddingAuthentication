using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInUser)
        {
            _userManager = userManager;
            _signInManager = signInUser;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {

            return View("Register");
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("Register", model);

            }

            var result = _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,

            }, model.Password).Result;

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]//Passes a value to http-only-cookie and the value is inserted into the form. If that value is not returned upon submission it fails.
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var signInResult = _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false).Result;
            if (!signInResult.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);

            }
            return RedirectToAction("Index", "Item");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}