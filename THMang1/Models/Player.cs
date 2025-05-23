using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THMang1.Models
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string ConnectionId { get; set; }
        public int CorrectRoundsWon { get; set; }
        public int TotalIncorrectGuesses { get; set; }
        public int Score { get; set; }
        public DateTime LastGuessTime { get; set; } = DateTime.MinValue; // Initialize to allow first guess immediately

        public Player(string playerName, string connectionId)
        {
            PlayerName = playerName;
            ConnectionId = connectionId;
            CorrectRoundsWon = 0;
            TotalIncorrectGuesses = 0;
            Score = 0;
        }
        
        public Player() { }
    }
}
