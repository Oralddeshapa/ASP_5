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
    public class TeachersController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _signInManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ILogger<HomeController> _logger;

        public TeachersController(ApplicationContext context,UserManager<User> signInManager, IHubContext<ChatHub> hubContext, ILogger<HomeController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _hubContext = hubContext;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into Controller");
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.ToListAsync());
        }

        // GET: Teachers/Details/5
        [Authorize(Roles = "moder,admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        [Authorize(Roles = "moder,admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "moder,admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,ReleaseDate,Subject,Tier")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var Class = _context.Classes.FirstOrDefault(m => m.Tier == teacher.Tier);
                if (Class != null)
                {
                    _context.Add(teacher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "moder,admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "moder,admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ReleaseDate,Subject,Tier")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                var Class = _context.Classes.FirstOrDefault(m => m.Tier == teacher.Tier);
                if (Class != null)
                {
                    try
                    {
                        _context.Update(teacher);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TeacherExists(teacher.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "moder,admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "user,moder")]
        public async Task<IActionResult> Donate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);

            TeacherViewModel tvm = new TeacherViewModel(teacher);
            if (teacher == null)
            {
                return NotFound();
            }

            User user = await _signInManager.GetUserAsync(User);

            Mark_for_numb mfn = await _context.Grades.FirstOrDefaultAsync(o => o.Number == user.Points);
            
            if(mfn == null)
                tvm.Points_of_user = -1;
            else
                tvm.Points_of_user = mfn.Points;

            return View(tvm);
        }

        [HttpPost]
        [Authorize(Roles = "user,moder")]
        public async Task<IActionResult> Donate(TeacherViewModel tvm)
        {
            User user = await _signInManager.GetUserAsync(User);

            Mark_for_numb mfn = await _context.Grades.FirstOrDefaultAsync(o => o.Number == user.Points);

            Class cls = await _context.Classes
                .FirstOrDefaultAsync(m => m.Tier == tvm.Tier);

            if (mfn.Points > Math.Abs(tvm.New_points))
            {           
                var users = _hubContext.Clients;

                Teacher teacher = new Teacher(tvm);
                mfn.Points -= Math.Abs(tvm.New_points);

                ///----------------------------------------------------------------------------
                var true_class = _context.Classes.Where(o => ((o.Min < teacher.points) && (o.Max > teacher.points))).First();

                //await _context.Classes.FirstOrDefaultAsync(o => ((o.Min < user.Points) && (o.Max > user.Points)));
                /*if (cls.Min > teacher.points)
                {
                    var array = _context.Classes.ToArray<Class>();
                    int index = Array.FindIndex(array, row => row.Tier == tvm.Tier);
                    while (cls.Min > teacher.points) // points can't be lower then D.min or teacher is garbage
                    {
                        index += 1;
                        cls = array[index];
                    }
                }
                else if (cls.Max < teacher.points)
                {
                    var array = _context.Classes.ToArray<Class>();
                    int index = Array.FindIndex(array, row => row.Tier == tvm.Tier);
                    while (cls.Max < teacher.points) // points can't be higher then D.max or teacher is too good
                    {
                        index -= 1;
                        cls = array[index];
                    }
                }*/
                teacher.Tier = true_class.Tier;
                ///----------------------------------------------------------------------------
               
                _context.Update(user);
                _context.Update(teacher);
                await _hubContext.Clients.All.SendAsync("Change", teacher.points.ToString(), teacher.Name, teacher.Subject);
                await _context.SaveChangesAsync();
                
            }
               
            return RedirectToAction();

        }

        public async Task<IActionResult> Help()
        {
            return View();
        }
    }
}
