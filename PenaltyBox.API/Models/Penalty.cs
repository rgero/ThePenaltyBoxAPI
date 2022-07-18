using System.ComponentModel.DataAnnotations;

namespace PenaltyBox.API.Models
{
    public enum SeasonType
    {
        Preseason,
        Regular,
        Playoffs
    }

    public class Penalty
    {
        public int Id { get; set; }

        public DateTime GameDate { get; set; }
        public string Player { get; set; }
        public string Team { get; set; }
        public string PenaltyName { get; set; }
        public string Opponent { get; set; }
        public string[] Referees { get; set; }
        public bool Home { get; set; }

        public SeasonType SeasonType { get; set; }

    }
}
