using Microsoft.AspNetCore.Mvc.Rendering;
using Projekt.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int HomeTeamPoints { get; set; }
        public int AwayTeamPoints { get; set; }
        public virtual List<MatchTeamCombination> MatchTeamCombinations { get; set; } = new();
    }
}
