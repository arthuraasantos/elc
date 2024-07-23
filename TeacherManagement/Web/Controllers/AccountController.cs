using Core.Entities.Users;
using Core.Factories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Users;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _identityUserManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> identityUserManager, SignInManager<AppUser> signInManager)
        {
            _identityUserManager = identityUserManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = null;

                var user = await _identityUserManager.FindByEmailAsync(model.Email);
                
                if (user == null || !(await _identityUserManager.CheckPasswordAsync(user, model.Password)))
                {
                    ViewBag.Message = "Erro ao fazer login. Verifique as credenciais ou tente novamente.";
                } else
                {
                    await _signInManager.SignInAsync(user, false);

                    return Redirect("~/Home/Index");
                }
            }

            return View("Index", model);
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Message = null;

            if (ModelState.IsValid)
            {
                var user = UserFactory.Create(model.Name, model.Email);
                
                var identityResult = await _identityUserManager.CreateAsync(user, model.Password);

                if (!identityResult.Succeeded)
                {
                    ViewBag.Message = "Erro ao registrar usuário";

                    return View("Register", model);
                }

                await _signInManager.SignInAsync(user, false);

                return Redirect("~/Home/Index");
            }

            return View("Register", model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }
    }
}
