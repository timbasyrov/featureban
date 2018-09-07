using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Featureban.Domain;

namespace Featureban.Client
{
    class GameClient
    {
        static void Main(string[] args)
        {
            int[] roundsCount = {15, 20};
            int[] wipLimits = {0, 1, 2, 3, 4, 5};
            int[] playersCount = {3, 5, 10};
            int gamesCount = 1000;

            List<GameResult> gameResults = new List<GameResult>();

            Console.Write("Simulation in progress");

            foreach (var wipLimit in wipLimits)
            {                
                foreach (var players in playersCount)
                {
                    foreach (var rounds in roundsCount)
                    {
                        var workItemsDone = 0;

                        for (var i = 0; i < gamesCount; i++)
                        {
                            var game = new Game(CreatePlayers(players), (uint)rounds, (uint)wipLimit);
                            workItemsDone += game.Play();
                        }

                        gameResults.Add(new GameResult()
                        {
                            PlayersCount = players,
                            RoundsCount = rounds,
                            WipLimit = wipLimit,
                            WorkItemsDone = workItemsDone
                        });

                        Console.Write(".");
                    }
                }
            }

            Console.WriteLine("Done");
            OutputGameResults(gameResults, gamesCount, wipLimits);
            Console.ReadKey();
        }

        private static IEnumerable<string> CreatePlayers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return $"Player {i}";
            }
        }

        private static void OutputGameResults(List<GameResult> gameResults, int gamesCount, int[] wipLimits)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Results for games count = {gamesCount}");
            stringBuilder.Append($"{"WipLimits",22} |");

            foreach (var wipLimit in wipLimits)
            {
                stringBuilder.Append($"{wipLimit,10}|");
            }

            stringBuilder.AppendLine();

            var orderedGameResults = gameResults
                .OrderBy(_ => _.RoundsCount)
                .ThenBy(_ => _.PlayersCount)
                .GroupBy(_ => new { _.RoundsCount, _.PlayersCount });

            foreach (var group in orderedGameResults)
            {
                stringBuilder.Append($"{group.Key.RoundsCount + " rounds",10}, {group.Key.PlayersCount + " players",10} |");
                foreach (var gameResult in group.OrderBy(_ => _.WipLimit))
                {
                    stringBuilder.Append($"{gameResult.WorkItemsDone,10}|");
                }
                stringBuilder.AppendLine();
            }

            Console.WriteLine(stringBuilder.ToString());
            File.WriteAllText("results.txt", stringBuilder.ToString());
        }
    }
}
