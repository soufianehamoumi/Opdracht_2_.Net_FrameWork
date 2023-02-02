#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DocentsController : ApplicationController
    {
        private readonly IdentityContext _context;

        public DocentsController(IdentityContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
            _context = context;
        }

        // GET: Docents
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var identityContext = _context.Docent.Include(d => d.Gender).Include(d => d.Modules);
            return View(await identityContext.ToListAsync());
        }

        // GET: Docents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                .Include(d => d.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // GET: Docents/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Gender, "Id", "Id");
            ViewData["ModuleIds"] = new MultiSelectList(_context.Module.OrderBy(c => c.Name), "Id", "Name");
            Docent docent = new Docent();
            return View(docent);
        }

        // POST: Docents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Voornaam,Achternaam,Geboortedatum,GenderID,ModuleIds")] Docent docent)
        {
            if (ModelState.IsValid)
            {
                docent.user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (docent.Modules == null)
                {
                    docent.Modules = new List<Module>();
                    foreach (int id in docent.ModuleIds)
                        docent.Modules.Add(_context.Module.FirstOrDefault(c => c.Id == id));
                }
                    
                _context.Add(docent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "Id", "Id", docent.GenderID);
            ViewData["ModuleIds"] = new MultiSelectList(_context.Module.OrderBy(c => c.Name), "Id", "Name");

            return View(docent);
        }

        // GET: Docents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent.FindAsync(id);
            if (docent == null)
            {
                return NotFound();
            }
            docent.ModuleIds = new List<int>();
            if (docent.Modules != null)
                foreach (Module cat in docent.Modules)
                    docent.ModuleIds.Add(cat.Id);
            ViewData["GenderID"] = new SelectList(_context.Gender, "Id", "Id", docent.GenderID);
            ViewData["ModuleIds"] = new MultiSelectList(_context.Module.OrderBy(c => c.Name), "Id", "Name");

            return View(docent);
        }

        // POST: Docents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Voornaam,Achternaam,Geboortedatum,GenderID,ModuleIds")] Docent docent)
        {
            if (id != docent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (docent.Modules == null)
                        docent.Modules = new List<Module>();
                    foreach (int i in docent.ModuleIds)
                        docent.Modules.Add(_context.Module.FirstOrDefault(c => c.Id == i));
                    _context.Update(docent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocentExists(docent.Id))
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
            docent.ModuleIds = new List<int>();
            if (docent.Modules != null)
                foreach (Module mod in docent.Modules)
                    docent.ModuleIds.Add(mod.Id);
            ViewData["GenderID"] = new SelectList(_context.Gender, "Id", "Id", docent.GenderID);
            ViewData["ModuleIds"] = new MultiSelectList(_context.Module.OrderBy(c => c.Name), "Id", "Name");

            return View(docent);
        }

        // GET: Docents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                .Include(d => d.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // POST: Docents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docent = await _context.Docent.FindAsync(id);
            _context.Docent.Remove(docent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocentExists(int id)
        {
            return _context.Docent.Any(e => e.Id == id);
        }
    }
}
