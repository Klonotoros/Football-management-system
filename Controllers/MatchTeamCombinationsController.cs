using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;

namespace Projekt.Controllers
{
    public class MatchTeamCombinationsController : Controller
    {
        private readonly ProjektContext _context;

        public MatchTeamCombinationsController(ProjektContext context)
        {
            _context = context;
        }

        // GET: MatchTeamCombinations
        public async Task<IActionResult> Index()
        {
            var projektContext = _context.MatchTeamCombination.Include(m => m.Match).Include(m => m.Team);
            return View(await projektContext.ToListAsync());
        }

        // GET: MatchTeamCombinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MatchTeamCombination == null)
            {
                return NotFound();
            }

            var matchTeamCombination = await _context.MatchTeamCombination
                .Include(m => m.Match)
                .Include(m => m.Team)
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (matchTeamCombination == null)
            {
                return NotFound();
            }

            return View(matchTeamCombination);
        }

        // GET: MatchTeamCombinations/Create
        public IActionResult Create()
        {
            ViewData["MatchId"] = new SelectList(_context.Match, "Id", "Id");
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Id");
            return View();
        }

        // POST: MatchTeamCombinations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MatchId,TeamId,IsHomeTeam")] MatchTeamCombination matchTeamCombination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matchTeamCombination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MatchId"] = new SelectList(_context.Match, "Id", "Id", matchTeamCombination.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Id", matchTeamCombination.TeamId);
            return View(matchTeamCombination);
        }

        // GET: MatchTeamCombinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MatchTeamCombination == null)
            {
                return NotFound();
            }

            var matchTeamCombination = await _context.MatchTeamCombination.FindAsync(id);
            if (matchTeamCombination == null)
            {
                return NotFound();
            }
            ViewData["MatchId"] = new SelectList(_context.Match, "Id", "Id", matchTeamCombination.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Id", matchTeamCombination.TeamId);
            return View(matchTeamCombination);
        }

        // POST: MatchTeamCombinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatchId,TeamId,IsHomeTeam")] MatchTeamCombination matchTeamCombination)
        {
            if (id != matchTeamCombination.MatchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matchTeamCombination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchTeamCombinationExists(matchTeamCombination.MatchId))
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
            ViewData["MatchId"] = new SelectList(_context.Match, "Id", "Id", matchTeamCombination.MatchId);
            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Id", matchTeamCombination.TeamId);
            return View(matchTeamCombination);
        }

        // GET: MatchTeamCombinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MatchTeamCombination == null)
            {
                return NotFound();
            }

            var matchTeamCombination = await _context.MatchTeamCombination
                .Include(m => m.Match)
                .Include(m => m.Team)
                .FirstOrDefaultAsync(m => m.MatchId == id);
            if (matchTeamCombination == null)
            {
                return NotFound();
            }

            return View(matchTeamCombination);
        }

        // POST: MatchTeamCombinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MatchTeamCombination == null)
            {
                return Problem("Entity set 'ProjektContext.MatchTeamCombination'  is null.");
            }
            var matchTeamCombination = await _context.MatchTeamCombination.FindAsync(id);
            if (matchTeamCombination != null)
            {
                _context.MatchTeamCombination.Remove(matchTeamCombination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchTeamCombinationExists(int id)
        {
          return _context.MatchTeamCombination.Any(e => e.MatchId == id);
        }
    }
}
