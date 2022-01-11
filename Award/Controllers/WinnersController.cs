using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Award.Core.Entities;
using Award.Infrastructure.Data;
using Award.Web.Models;
using Award.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Award.Web.Controllers
{
    [Authorize]

    public class WinnersController : Controller
    {
        private readonly AwardDbContext _context;

        public WinnersController(AwardDbContext context)
        {
            _context = context;
        }

        // GET: Winners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Winners.Include(a => a.Award).Include(a => a.Category).Include(a => a.Employee).Include(a => a.Sector).ToListAsync());
        }

        // GET: Winners/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winner = await _context.Winners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (winner == null)
            {
                return NotFound();
            }

            return View(winner);
        }

        // GET: Winners/Create
        public async Task<IActionResult> Create()
        {
            //var WinnerViewModel = new CategoryViewModel
            //{
            //      Awards = await _context.Awards.ToListAsync().ConfigureAwait(false)
            //};
            var WinnerViewModel = new WinnerViewModel
            {
                Awards = await _context.Awards.ToListAsync().ConfigureAwait(false),
                Sectors = await _context.Sectors.ToListAsync().ConfigureAwait(false),
                Employees = await _context.Employee.ToListAsync().ConfigureAwait(false),
                Categories = await _context.Categories.ToListAsync().ConfigureAwait(false)
            };
            return View(WinnerViewModel);
        }

        // POST: Winners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WinnerViewModel winnerViewModel)
        {
            //Winner winner = new Winner();
            if (ModelState.IsValid)
            {
                winnerViewModel.winner.CreateDate = System.DateTime.Now;
                _context.Add(winnerViewModel.winner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(winnerViewModel.winner);
        }

        // GET: Winners/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winner = await _context.Winners.FindAsync(id);
            if (winner == null)
            {
                return NotFound();
            }
            return View(winner);
        }

        // POST: Winners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("IsAnnounced,ShowWinnerImage,AnounceDate,IdAward,IdCategory,IdEmployee,IdSector,Id,CreateDate,UpdatedDate,DeletedDate")] Winner winner)
        public async Task<IActionResult> Edit(long id, [Bind("ShowWinnerImage,AnounceDate,Id")] Winner winner)
        {
            if (id != winner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var winner2 = await _context.Winners.FindAsync(id);
                    winner2.ShowWinnerImage = winner.ShowWinnerImage;
                    winner2.IsAnnounced = true;
                    if (winner2.AnounceDate == null)
                    {
                        winner2.AnounceDate = System.DateTime.Now;
                    }
                    _context.Update(winner2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WinnerExists(winner.Id))
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
            return View(winner);
        }

        // GET: Winners/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var winner = await _context.Winners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (winner == null)
            {
                return NotFound();
            }

            return View(winner);
        }

        // POST: Winners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var winner = await _context.Winners.FindAsync(id);
            _context.Winners.Remove(winner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WinnerExists(long id)
        {
            return _context.Winners.Any(e => e.Id == id);
        }

    }
}
