using Featureban.Domain.Common;
using Featureban.Domain.Enums;

namespace Featureban.Domain
{
    public class Player : Entity
    {
        public string Name { get; }
        private readonly Coin _coin;

        public Player(string name, Coin coin)
        {
            Name = name;
            _coin = coin;
        }

        public CoinFlipResult FlipCoin()
        {
            return _coin.Flip();
        }
    }
}
