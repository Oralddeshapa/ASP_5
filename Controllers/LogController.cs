using System;
using System.Collections.Generic;
using System.Linq;
using TeaChair.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TeaChair.Data;
using TeaChair.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace TeaChair.Controllers
{
    public class LogController : Controller
    {
        private readonly ApplicationContext _context;

        public LogController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View( await _context.Logs.ToListAsync());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            LogForDonate log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                _context.Logs.Remove(log);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
