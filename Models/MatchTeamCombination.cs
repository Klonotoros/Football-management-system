using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class MatchTeamCombination
    {
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public bool IsHomeTeam { get; set; }
        public virtual Team Team { get; set; }
        public virtual Match Match { get; set; }
    }
}
