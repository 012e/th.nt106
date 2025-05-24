using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THMang1.Models;

namespace THMang1.Server.Services
{
    public interface IGameService
    {
        // Game State Properties
        int SecretNumber { get; }
        int MinRange { get; }
        int MaxRange { get; }
        bool IsRoundActive { get; }
        string? CurrentRoundWinnerName { get; }
        int CurrentRoundNumber { get; }
        int TotalRoundsToPlay { get; }
        List<Player> ConnectedPlayers { get; }
        List<GameRound> GameHistory { get; }
        GameRound? CurrentGameRound { get; }


        // Player Management
        Player? RegisterPlayer(string playerName, string connectionId);
        Player? RemovePlayer(string connectionId);
        Player? GetPlayerByConnectionId(string connectionId);
        Player? GetPlayerByName(string playerName);

        // Game Flow
        (bool roundStarted, string? message) StartNewRound();
        (string feedback, bool isCorrect, bool roundOver, string? winnerName) ProcessGuess(string playerName, int guessedNumber);
        string? GetOverallWinner(); // Basic implementation for now
        void EndGame(); // Placeholder for game wrap-up logic
        bool IsGameFinished { get; }
        string FormatHistoryForUpload();
    }

    public class GameService : IGameService
    {
        private readonly object _lock = new object(); // For thread safety
        private Random _random = new Random();

        public int SecretNumber { get; private set; }
        public int MinRange { get; private set; }
        public int MaxRange { get; private set; }
        public bool IsRoundActive { get; private set; }
        public string? CurrentRoundWinnerName { get; private set; }
        public int CurrentRoundNumber { get; private set; }
        public int TotalRoundsToPlay { get; private set; } // Will be set in constructor
        public List<Player> ConnectedPlayers { get; private set; } = new List<Player>();
        public List<GameRound> GameHistory { get; private set; } = new List<GameRound>();
        public GameRound? CurrentGameRound { get; private set; }
        public bool IsGameFinished { get; private set; } = false;

        private const int MIN_TOTAL_ROUNDS = 5;
        private const int MAX_TOTAL_ROUNDS = 10; // Server decides, e.g. random between 5 and 10

        public GameService()
        {
            // Server decides the number of rounds
            TotalRoundsToPlay = _random.Next(MIN_TOTAL_ROUNDS, MAX_TOTAL_ROUNDS + 1);
            // Optionally, start the first round immediately if desired, or wait for players.
            // For now, let's wait for a manual trigger or player joining.
        }

        public Player? RegisterPlayer(string playerName, string connectionId)
        {
            lock (_lock)
            {
                if (ConnectedPlayers.Any(p => p.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
                {
                    // Potentially handle name collision (e.g., return null or throw exception)
                    // For now, let's assume unique names or overwrite (simpler for this stage)
                    // Or, better, return null if name exists and let client handle it.
                    return null; // Name already taken
                }
                var player = new Player(playerName, connectionId);
                ConnectedPlayers.Add(player);
                return player;
            }
        }

        public Player? RemovePlayer(string connectionId)
        {
            lock (_lock)
            {
                var player = ConnectedPlayers.FirstOrDefault(p => p.ConnectionId == connectionId);
                if (player != null)
                {
                    ConnectedPlayers.Remove(player);
                }
                // If no players left and game was active, consider pausing or ending the round/game.
                // For now, just remove.
                return player;
            }
        }

        public Player? GetPlayerByConnectionId(string connectionId)
        {
            lock (_lock)
            {
                return ConnectedPlayers.FirstOrDefault(p => p.ConnectionId == connectionId);
            }
        }
        public Player? GetPlayerByName(string playerName)
        {
            lock (_lock)
            {
                return ConnectedPlayers.FirstOrDefault(p => p.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase));
            }
        }


        public (bool roundStarted, string? message) StartNewRound()
        {
            lock (_lock)
            {
                if (IsGameFinished) return (false, "The game has already finished.");
                if (IsRoundActive) return (false, "A round is already in progress.");
                if (CurrentRoundNumber >= TotalRoundsToPlay)
                {
                    EndGame();
                    return (false, "Game has reached the total number of rounds.");
                }

                CurrentRoundNumber++;
                MinRange = _random.Next(1, 50);
                MaxRange = _random.Next(MinRange + 10, 150); // Ensure b > a and some gap
                SecretNumber = _random.Next(MinRange + 1, MaxRange); // a < x < b

                IsRoundActive = true;
                CurrentRoundWinnerName = null;
                CurrentGameRound = new GameRound(CurrentRoundNumber, MinRange, MaxRange, SecretNumber);

                return (true, $"New round {CurrentRoundNumber} started! Guess between {MinRange} and {MaxRange}.");
            }
        }

        public (string feedback, bool isCorrect, bool roundOver, string? winnerName) ProcessGuess(string playerName, int guessedNumber)
        {
            lock (_lock)
            {
                if (!IsRoundActive || CurrentGameRound == null)
                {
                    return ("No round is currently active.", false, false, null);
                }
                if (IsGameFinished)
                {
                    return ("The game has already finished.", false, false, null);
                }


                var player = GetPlayerByName(playerName);
                if (player == null)
                {
                    return ("Player not found.", false, false, null);
                }

                // Enforce 3-second delay (server-side check)
                if ((DateTime.UtcNow - player.LastGuessTime).TotalSeconds < 3)
                {
                    return ("Please wait before guessing again.", false, false, null);
                }
                player.LastGuessTime = DateTime.UtcNow;


                if (guessedNumber <= MinRange || guessedNumber >= MaxRange)
                {
                    CurrentGameRound.Guesses.Add(new PlayerGuess(playerName, guessedNumber, false, "Guess out of range."));
                    player.TotalIncorrectGuesses++;
                    // player.Score -= 1; // Example scoring
                    return ($"Your guess {guessedNumber} is out of the range ({MinRange} - {MaxRange}).", false, false, null);
                }

                bool isCorrect = guessedNumber == SecretNumber;
                string feedback;
                PlayerGuess guessRecord;

                if (isCorrect)
                {
                    CurrentRoundWinnerName = playerName;
                    IsRoundActive = false; // Round ends
                    player.CorrectRoundsWon++;
                    player.Score += 10; 
                    feedback = $"Correct! {playerName} guessed the number {SecretNumber}.";
                    guessRecord = new PlayerGuess(playerName, guessedNumber, true, "Correct");
                    CurrentGameRound.WinnerName = playerName;
                }
                else
                {
                    player.TotalIncorrectGuesses++;
                    player.Score -= 1; 
                    feedback = guessedNumber < SecretNumber ? "Too low." : "Too high.";
                    guessRecord = new PlayerGuess(playerName, guessedNumber, false, feedback);
                }

                CurrentGameRound.Guesses.Add(guessRecord);

                if (isCorrect) // Round is over because a correct guess was made
                {
                    CurrentGameRound.EndTime = DateTime.UtcNow;
                    GameHistory.Add(CurrentGameRound);
                    if (CurrentRoundNumber >= TotalRoundsToPlay)
                    {
                        EndGame(); // This will set IsGameFinished
                    }
                    return (feedback, true, true, playerName);
                }

                return (feedback, false, false, null);
            }
        }

        public void EndGame()
        {
            lock (_lock)
            {
                IsRoundActive = false;
                IsGameFinished = true;
                // Further logic for determining overall winner, preparing history for upload etc.
                // will be added here later.
                // For now, just mark as finished.
                if (CurrentGameRound != null && !GameHistory.Contains(CurrentGameRound) && CurrentGameRound.EndTime == null)
                {
                    // If the game ends due to total rounds without a winner in the last active round
                    CurrentGameRound.EndTime = DateTime.UtcNow;
                    GameHistory.Add(CurrentGameRound);
                }
            }
        }
        public string? GetOverallWinner()
        {
            if (!IsGameFinished || !ConnectedPlayers.Any()) return null;

            lock (_lock)
            {
                var sortedPlayers = ConnectedPlayers
                    .OrderByDescending(p => p.CorrectRoundsWon)
                    .ThenBy(p => p.TotalIncorrectGuesses)
                    .ToList();

                return sortedPlayers.FirstOrDefault()?.PlayerName;
            }
        }

        public string FormatHistoryForUpload()
        {
            lock (_lock)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"=== GAME ĐOÁN SỐ - Game History ({DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC) ===");
                sb.AppendLine($"Total Rounds Played: {GameHistory.Count} out of {TotalRoundsToPlay} planned.");

                foreach (var round in GameHistory)
                {
                    sb.AppendLine($"\n--- Round {round.RoundNumber} ---");
                    sb.AppendLine($"Range: {round.MinRange} < X < {round.MaxRange}");
                    sb.AppendLine($"Secret Number: {round.SecretNumber}");
                    sb.AppendLine($"Started: {round.StartTime:HH:mm:ss}");
                    if (round.EndTime.HasValue)
                    {
                        sb.AppendLine($"Ended: {round.EndTime.Value:HH:mm:ss}");
                    }
                    sb.AppendLine($"Winner: {round.WinnerName ?? "N/A"}");
                    sb.AppendLine("Guesses:");
                    if (round.Guesses.Any())
                    {
                        foreach (var guess in round.Guesses)
                        {
                            sb.AppendLine($"  - {guess.PlayerName}: {guess.GuessedNumber} ({guess.Feedback}) at {guess.Timestamp:HH:mm:ss}");
                        }
                    }
                    else
                    {
                        sb.AppendLine("  No guesses made this round.");
                    }
                }

                sb.AppendLine("\n--- Player Stats ---");
                if (ConnectedPlayers.Any())
                {
                    foreach (var player in ConnectedPlayers.OrderByDescending(p => p.CorrectRoundsWon).ThenBy(p => p.TotalIncorrectGuesses))
                    {
                        sb.AppendLine($"- {player.PlayerName}: {player.CorrectRoundsWon} wins, {player.TotalIncorrectGuesses} incorrect guesses, Score: {player.Score}");
                    }
                }
                else
                {
                    sb.AppendLine("No player data available (all players may have disconnected before game end).");
                }


                string? overallWinner = GetOverallWinner();
                sb.AppendLine($"\nOverall Winner: {overallWinner ?? "N/A"}");
                return sb.ToString();
            }
        }
    }
}
