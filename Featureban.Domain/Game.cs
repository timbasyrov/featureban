using System;
using System.Collections.Generic;
using System.Linq;
using Featureban.Domain.Common;

namespace Featureban.Domain
{
    public class Game : AggregateRoot
    {
        private readonly uint _roundsCount;
        private readonly Board _board;
        private readonly List<Player> _players;

        public Game(IEnumerable<string> playerNames, uint roundsCount, uint wipLimit)
        {
            if (playerNames == null || !playerNames.Any())
                throw new ArgumentException("Can not create game without players", nameof(playerNames));

            if (roundsCount == 0)
                throw new ArgumentException("Rounds count must be greater than zero", nameof(roundsCount));

            _roundsCount = roundsCount;
            _board = new Board(wipLimit);
            _players = new List<Player>();
            var coin = new Coin();

            foreach (var name in playerNames)
            {
                _players.Add(new Player(name, _board, coin));
            }
        }

        public int Play()
        {
            StartGame();

            for (var i = 0; i < _roundsCount; i++)
            {
                PlayRound();
            }

            return _board.WorkItems.Count(_ => _.IsComplete);
        }

        private void PlayRound()
        {
            foreach (var player in _players)
            {
                var coinFlipResult = player.FlipCoin();
                player.MakeMove(coinFlipResult);
            }

        }

        private void StartGame()
        {
            foreach (var player in _players)
            {
                _board.TryAssignWorkItemTo(player);
            }
        }
    }
}
