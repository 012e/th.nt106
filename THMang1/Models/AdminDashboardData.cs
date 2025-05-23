using THMang1.Server.Services;

namespace THMang1.Models;

public class AdminDashboardData
{
    // Current Round Info
    public int? CurrentTargetNumber { get; set; } // The secret number for the current round
    public int CurrentMinRange { get; set; }
    public int CurrentMaxRange { get; set; }
    public bool IsRoundActive { get; set; }

    // Game Progress Info
    public int CurrentRoundNumber { get; set; }
    public int TotalRoundsToPlay { get; set; }
    public bool IsGameFinished { get; set; }
    public string? OverallWinner { get; set; }


    // Player Info
    public List<string> ConnectedPlayerNames { get; set; } = new List<string>();
    public int PlayerCount { get; set; }

    // History
    public List<GameRound> FullGameHistory { get; set; } = new List<GameRound>();

    public AdminDashboardData(IGameService gameService)
    {
        CurrentTargetNumber = gameService.IsRoundActive ? gameService.SecretNumber : (int?)null;
        CurrentMinRange = gameService.MinRange;
        CurrentMaxRange = gameService.MaxRange;
        IsRoundActive = gameService.IsRoundActive;

        CurrentRoundNumber = gameService.CurrentRoundNumber;
        TotalRoundsToPlay = gameService.TotalRoundsToPlay;
        IsGameFinished = gameService.IsGameFinished;
        OverallWinner = gameService.IsGameFinished ? gameService.GetOverallWinner() : null;

        ConnectedPlayerNames = gameService.ConnectedPlayers.Select(p => p.PlayerName).ToList();
        PlayerCount = gameService.ConnectedPlayers.Count;

        FullGameHistory = new List<GameRound>(gameService.GameHistory); // Create a copy
    }

    public AdminDashboardData() { }
}
