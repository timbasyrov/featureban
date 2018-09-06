using Xunit;
using FluentAssertions;
using AutoFixture;

namespace Featureban.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void PlayersWithIdenticalName_ShouldNotBeEqual()
        {
            var fixture = new Fixture();
            var firstPlayer = new Player("JD", fixture.Create<Board>(), fixture.Create<Coin>());
            var secondPlayer = new Player("JD", fixture.Create<Board>(), fixture.Create<Coin>());

            var result = firstPlayer.Equals(secondPlayer);

            result.Should().BeFalse();
        }
    }
}
