using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Faculty { get; set; }
        public int? Points { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public virtual List<MatchTeamCombination> MatchTeamCombinations { get; set; } = new();
        public virtual List<Player> Players { get; set; } = new();
    }
}
