using AutoFixture;

namespace Featureban.Domain.Tests.DSL
{
    public class PlayerBuilder
    {
        private Board _board;
        private string _name;

        public PlayerBuilder WithBoard(Board board)
        {
            _board = board;
            return this;
        }

        public PlayerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Player Please()
        {
            var fixture = new Fixture();
            if (string.IsNullOrEmpty(_name))
                _name = fixture.Create<string>();

            return new Player(_name, _board ?? fixture.Create<Board>(), new Coin());
        }
    }
}
