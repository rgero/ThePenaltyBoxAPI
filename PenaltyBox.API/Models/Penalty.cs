using System.ComponentModel.DataAnnotations;

namespace PenaltyBox.API.Models
{
    public class Penalty
    {
        public int Id { get; set; }

        [Required]
        public DateTime GameDate { get; set; }
        
        [Required]
        public string Player { get; set; }
        
        [Required]
        public string Team { get; set; }
        
        [Required]
        public string PenaltyName { get; set; }
        
        [Required]
        public string Opponent { get; set; }
        
        [Required]
        public string[] Referees { get; set; }
        
        [Required]
        public bool Home { get; set; }

    }
}
