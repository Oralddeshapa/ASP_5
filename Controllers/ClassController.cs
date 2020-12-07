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

namespace TeaChair.Controllers
{
    public class ClassController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<RolesController> _logger;
        public ClassController(ApplicationContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into ClassController");
        }

        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Index() { return View(await _context.Classes.ToListAsync()); }
        
        [Authorize(Roles = "admin, moder")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Create(Class cls)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cls);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cls);
        }

        [HttpPost]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var cls = await _context.Classes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cls == null)
            {
                return NotFound();
            }

            return View(cls);
        }

        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cls = await _context.Classes.FindAsync(id);
            if (cls == null)
            {
                return NotFound();
            }
            return View(cls);
        }
        [HttpPost]
        [Authorize(Roles = "admin, moder")]
        public async Task<IActionResult> Edit(Class cls)
        {
            if (ModelState.IsValid)
            {
                Class clas = await _context.Classes.FindAsync(cls.Id);
                if (ModelState.IsValid)
                {
                    try
                    {
                        clas = cls;
                        _context.Update(clas);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Classes.Any(e => e.Id == cls.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(cls);
        }
    }
}
