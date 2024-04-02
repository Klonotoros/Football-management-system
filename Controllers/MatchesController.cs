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
    public class MatchesController : Controller
    {
        private readonly ProjektContext _context;

        public MatchesController(ProjektContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var matches = _context.Match.Include(m => m.MatchTeamCombinations).ThenInclude(c => c.Team).ToList();
            return View(matches); //to dodałem i to co wyżej
            //return View(await _context.Match.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Match == null)
            {
                return NotFound();
            }

            var match = await _context.Match
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewBag.Teams = new SelectList(_context.Team, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,HomeTeamPoints,AwayTeamPoints,MatchTeamCombinations")] Match match)
        {
            if (ModelState.IsValid)
            {
                bool isFirstIteration = true;
                _context.Add(match);

                // Dodajemy rekordy MatchTeamCombination dla drużyn
                foreach (var matchTeamCombination in match.MatchTeamCombinations)
                {
                    if (isFirstIteration)
                    {
                        matchTeamCombination.IsHomeTeam = true;
                        isFirstIteration = false;
                    }
                    matchTeamCombination.MatchId = match.Id; // Przypiszemy Id meczu
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(match);
        }
        //public async Task<IActionResult> Create([Bind("Id,Date,HomeTeamPoints,AwayTeamPoints")] Match match)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(match);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(match);
        //}

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Match == null)
            {
                return NotFound();
            }

            var match = await _context.Match
                .Include(m => m.MatchTeamCombinations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            ViewBag.Teams = new SelectList(_context.Team, "Id", "Name");

            return View(match);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,HomeTeamPoints,AwayTeamPoints,MatchTeamCombinations")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);

                    // Pobierz istniejące rekordy MatchTeamCombination dla danego meczu
                    var existingCombinations = await _context.MatchTeamCombination
                        .Where(x => x.MatchId == match.Id)
                        .ToListAsync();

                    // Zaktualizuj rekordy MatchTeamCombination na podstawie danych z formularza
                    foreach (var matchTeamCombination in match.MatchTeamCombinations)
                    {
                        var existingCombination = existingCombinations.FirstOrDefault(x => x.TeamId == matchTeamCombination.TeamId);

                        if (existingCombination != null)
                        {
                            // Znaleziono istniejący rekord, zaktualizuj flagę IsHomeTeam
                            existingCombination.IsHomeTeam = matchTeamCombination.IsHomeTeam;
                            _context.Update(existingCombination);
                        }
                        else
                        {
                            // Nie znaleziono istniejącego rekordu, dodaj nowy
                            matchTeamCombination.MatchId = match.Id;
                            _context.MatchTeamCombination.Add(matchTeamCombination);
                        }

                        // Zamień drużyny miejscami i zaktualizuj flagę IsHomeTeam dla obu drużyn
                        var otherCombination = existingCombinations.FirstOrDefault(x => x.TeamId != matchTeamCombination.TeamId);

                        if (otherCombination != null)
                        {
                            otherCombination.IsHomeTeam = !matchTeamCombination.IsHomeTeam;
                            _context.Update(otherCombination);
                        }

                        // Aktualizuj flagę IsHomeTeam dla pierwszej drużyny
                        var firstCombination = existingCombinations.FirstOrDefault();

                        if (firstCombination != null)
                        {
                            firstCombination.IsHomeTeam = matchTeamCombination.IsHomeTeam;
                            _context.Update(firstCombination);
                        }
                    }

                    // Usuń zbędne rekordy MatchTeamCombination
                    var obsoleteCombinations = existingCombinations
                        .Where(x => !match.MatchTeamCombinations.Any(c => c.TeamId == x.TeamId))
                        .ToList();

                    _context.MatchTeamCombination.RemoveRange(obsoleteCombinations);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
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

            ViewBag.Teams = new SelectList(_context.Team, "Id", "Name");

            return View(match);
        }






        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Match == null)
            {
                return NotFound();
            }

            var match = await _context.Match
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Match == null)
            {
                return Problem("Entity set 'ProjektContext.Match'  is null.");
            }
            var match = await _context.Match.FindAsync(id);
            if (match != null)
            {
                _context.Match.Remove(match);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
          return _context.Match.Any(e => e.Id == id);
        }
    }
}
