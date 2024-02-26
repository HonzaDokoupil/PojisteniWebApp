using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PojisteniWebApp.Data;
using PojisteniWebApp.Models.AccountViewModels;
using PojisteniWebApp.Models;
using PojisteniWebApp.Classes;

namespace PojisteniWebApp.Controllers
{

    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context) : Controller
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Akce prihlaseni uživatele GET
        /// </summary>
        /// <param name="returnUrl">url</param>
        /// <returns>View</returns>
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Akce prihlaseni uzivatele POST
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="returnUrl">url</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if(ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    TempData["success"] = "Přihlášení proběhlo úspěšně";
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("Login error", "Neplatné přihlašovací údaje.");
                TempData["error"] = "Přihlášení se nezdařilo";
                return View(model);
            }
            TempData["error"] = "Přihlášení se nezdařilo";
            return View(model);
        }
        /// <summary>
        /// Akce registrace uzivatele GET
        /// </summary>
        /// <param name="returnUrl">url</param>
        /// <returns>View</returns>
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        /// <summary>
        /// Akce registrace uzivatele POST
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="returnUrl">url</param>
        /// <returns>View</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            

            if (ModelState.IsValid)
            {
                if (UserExists(model.Email)) 
                {
                    ModelState.AddModelError("Email", "Tento Email už je regitrovaný");
                    TempData["error"] = "Registrace se nezdařila";
                    return View(model);
                }

                IdentityUser user = new IdentityUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //pokud se registruje novy uzivatel co uz je v databazi jako pojistenec(najdeme ho metodou) priradime mu rovnou roli pojisteneho
                    var person = GetPersonByUserEmail(user);
                    if(person != null)
                    {
                            await _userManager.AddToRoleAsync(user, UserRoles.Insured);
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            TempData["success"] = "Registrace proběhla úspěšně";
                            return RedirectToLocal(returnUrl);
                    }
                    //pokud se registruje novy uživatel který není veden jako pojištěnec presmerujeme ho na vytvoreni pojistence
                    await _userManager.AddToRoleAsync(user, UserRoles.Insured);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["success"] = "Registrace proběhla úspěšně";
                    return RedirectToAction("CreateOnRegister", "InsuredPersons");
                }
            }
            TempData["error"] = "Registrace se nezdařila";
            return View(model);
        }
        /// <summary>
        /// Akce odhlaseni uzivatele
        /// </summary>
        /// <returns>View</returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["success"] = "Odhlášení proběhlo úspěšně";
            return RedirectToLocal(null);
        }
        /// <summary>
        /// Akce pro zmenu hesla GET
        /// </summary>
        /// <returns> View</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<IActionResult> ChangePassword(string? returnUrl = null)
        {
            IdentityUser user = await _userManager.GetUserAsync(User) ??
                throw new ApplicationException($"Nepodařilo se načíst uživatele s ID {_userManager.GetUserId(User)}.");

            ChangePasswordViewModel model = new();
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }
        /// <summary>
        /// Akce pro zmenu hesla POST
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>View</returns>
        /// <exception cref="ApplicationException"></exception>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Chyba změny hesla";
                return View(model);
            }

            IdentityUser user = await _userManager.GetUserAsync(User) ??
                throw new ApplicationException($"Nepodařilo se načíst uživatele s ID: {_userManager.GetUserId(User)}.");

            IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                ModelState.AddModelError("Password", "Nepodařilo se změnit heslo");
                TempData["error"] = "Chyba změny hesla";
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["success"] = "Změna hesla proběhla úspěšně";
            return RedirectToLocal(returnUrl);
        }
        /// <summary>
        /// Vraci uzivatele na domovskou stranku
        /// </summary>
        /// <param name="returnUrl">url adresa</param>
        /// <returns>akce přesměrování</returns>
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) :RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Získá pojištence na základě jeho emailu
        /// </summary>
        /// <param name="user">IdentityUser</param>
        /// <returns>pojištěnce</returns>
        private InsuredPerson GetPersonByUserEmail(IdentityUser user)
        {
            return _context.InsuredPersons.FirstOrDefault(person => person.Email == user.Email);
        }
        /// <summary>
        /// Zjistí jestli existuje uživatel
        /// </summary>
        /// <param name="email">řetězec email</param>
        /// <returns>logická hodnota</returns>
        private bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
