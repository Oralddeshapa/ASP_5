using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeaChair.Models;
using TeaChair.Services;

namespace TeaChair.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(CurrentTimeService cts)//Count counter, CounterServ cs
        {
            ViewData["time"] = cts.GetTime(0);
            if (_roleManager.Roles.Any())
            {

            }
            else
            {
                await _roleManager.CreateAsync(new IdentityRole("user"));
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("moder"));
            }
            /*
            ViewData["count_1"] = counter.Value;
            ViewData["count_2"] = cs.Counter.Value;
            <a class="nav-link text-dark">@((int)ViewData["count_1"])</a>
            <a class="nav-link text-dark">@((int)ViewData["count_2"])</a>*/
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(CurrentTimeService cts, int? statusCode = null )
        {
            if (statusCode.HasValue)
            {
                if (statusCode.Value == 404 || statusCode.Value == 500)
                {
                    var viewName = statusCode.ToString();
                    ErrorViewModel evm = new ErrorViewModel();
                    evm.Time = cts.GetTime(0);
                    evm.SratusCode = Activity.Current.Id;
                    evm.RequestId = statusCode.Value.ToString();
                    return View(evm);
                }
            }
            return View();
        }
    }
}
