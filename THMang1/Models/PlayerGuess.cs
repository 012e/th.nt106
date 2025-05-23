using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1.Models
{
    public class PlayerGuess
    {
        public string PlayerName { get; set; }
        public int GuessedNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsCorrect { get; set; }
        public string Feedback { get; set; } // e.g., "Too high", "Too low", "Correct"


        public PlayerGuess(string playerName, int guessedNumber, bool isCorrect, string feedback)
        {
            PlayerName = playerName;
            GuessedNumber = guessedNumber;
            Timestamp = DateTime.UtcNow;
            IsCorrect = isCorrect;
            Feedback = feedback;
        }
        public PlayerGuess() { }
    }
}
