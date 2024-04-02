using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;

namespace Projekt.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ProjektContext _context;

        public StatisticsController(ProjektContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teamStats = _context.Team
                .Where(x=>x.Id !=10)
                .Select(team => new
                {
                    Team = team,
                    MatchesPlayed = team.MatchTeamCombinations.Count(),
                    Wins = team.MatchTeamCombinations.Count(c => c.IsHomeTeam && c.Match.HomeTeamPoints > c.Match.AwayTeamPoints || !c.IsHomeTeam && c.Match.AwayTeamPoints > c.Match.HomeTeamPoints),
                    Draws = team.MatchTeamCombinations.Count(c => c.Match.HomeTeamPoints == c.Match.AwayTeamPoints),
                    Losses = team.MatchTeamCombinations.Count(c => c.IsHomeTeam && c.Match.HomeTeamPoints < c.Match.AwayTeamPoints || !c.IsHomeTeam && c.Match.AwayTeamPoints < c.Match.HomeTeamPoints),
                    GoalsFor = team.MatchTeamCombinations.Sum(c => c.IsHomeTeam ? c.Match.HomeTeamPoints : c.Match.AwayTeamPoints),
                    GoalsAgainst = team.MatchTeamCombinations.Sum(c => c.IsHomeTeam ? c.Match.AwayTeamPoints : c.Match.HomeTeamPoints)
                })
                .ToList();

            foreach (var teamStat in teamStats)
            {
                teamStat.Team.Points = teamStat.Wins * 3 + teamStat.Draws;
            }

            var sortedTeamStats = teamStats.OrderByDescending(stat => stat.Team.Points).ToList();

            var model = sortedTeamStats.Cast<dynamic>().ToList();

            return View(model);
        }

    }
}