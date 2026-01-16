using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StartEvent.Web.Data;
using StartEvent.Web.Models;

namespace StartEvent.Web.Controllers
{
    public class ContactMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ContactMessages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactMessages.ToListAsync());
        }

        // GET: ContactMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactMessage == null)
            {
                return NotFound();
            }

            return View(contactMessage);
        }

        // GET: ContactMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Message,DateEnvoye")] ContactMessage contactMessage)
        {
            if (ModelState.IsValid)
            {
                contactMessage.DateEnvoye = DateTime.UtcNow;
                _context.Add(contactMessage);
                await _context.SaveChangesAsync();
                TempData["ContactSuccess"] = "Votre message a bien été envoyé et stocké dans la boîte de réception.";
                return RedirectToAction(nameof(Index));
            }
            return View(contactMessage);
        }

        // GET: ContactMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage == null)
            {
                return NotFound();
            }
            return View(contactMessage);
        }

        // POST: ContactMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Message,DateEnvoye")] ContactMessage contactMessage)
        {
            if (id != contactMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactMessageExists(contactMessage.Id))
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
            return View(contactMessage);
        }

        // GET: ContactMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactMessage = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactMessage == null)
            {
                return NotFound();
            }

            return View(contactMessage);
        }

        // POST: ContactMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactMessage = await _context.ContactMessages.FindAsync(id);
            if (contactMessage != null)
            {
                _context.ContactMessages.Remove(contactMessage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactMessageExists(int id)
        {
            return _context.ContactMessages.Any(e => e.Id == id);
        }
    }
}
