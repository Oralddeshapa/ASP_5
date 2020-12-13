using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TeaChair.Models;
using Microsoft.Extensions.Logging;
using TeaChair.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TeaChair.Controllers
{

        public class UsersController : Controller
        {
            UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(UserManager<User> userManager, ILogger<UsersController> logger)
            {
                _userManager = userManager;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into UserController");
        }

        public async Task<IActionResult> Index()
        {
            //
            if (!_userManager.Users.Any())
            {
                Random rand = new Random();
                User admin = new User
                {
                    Points = 85350155,
                    Email = "Orald" + "@bsuir.by",
                    UserName = "Orald"
                };
                var result = await _userManager.CreateAsync(admin, "123456");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "user");
                    await _userManager.AddToRoleAsync(admin, "moder");
                    await _userManager.AddToRoleAsync(admin, "admin");
                }

                for (int i = 0; i < 20; i++)
                {
                    int tom = 85350100 + i;
                    string tim = tom.ToString();
                    User user_1 =
                        new User
                        {
                            Points = 85350100 + i,
                            Email = tim + "@bsuir.by",
                            UserName = tim
                        };
                    int pass = (85350100 + i) / (i + 1) * i;
                    result = await _userManager.CreateAsync(user_1, pass.ToString());
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user_1, "user");
                    }
                }
            }
            //
            return View(_userManager.Users.ToList());
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create() => View();

            [HttpPost]
            [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
            {             
                if (ModelState.IsValid)
                {  
                    User user = new User { Email = model.Email, UserName = model.Email, Points = model.Points };
                    var result = await _userManager.CreateAsync(user, model.Password);

                //
                //

                if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                return View(model);
            }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Points = user.Points };
                return View(model);
            }

            [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.Points = model.Points;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
                return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                }
                return RedirectToAction("Index");
            }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
                return View(model);
            }


            [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
                        var _passwordValidator =
                            HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                        var _passwordHasher =
                            HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                        IdentityResult result =
                            await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Пользователь не найден");
                    }
                }
                return View(model);
            }
        }
}
