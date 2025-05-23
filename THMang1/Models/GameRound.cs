using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1.Models
{
    public class GameRound
    {
        public int RoundNumber { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public int SecretNumber { get; set; }
        public string? WinnerName { get; set; }
        public List<PlayerGuess> Guesses { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public GameRound(int roundNumber, int minRange, int maxRange, int secretNumber)
        {
            RoundNumber = roundNumber;
            MinRange = minRange;
            MaxRange = maxRange;
            SecretNumber = secretNumber;
            Guesses = new List<PlayerGuess>();
            StartTime = DateTime.UtcNow;
        }
    }
}
