using Xunit;
using FluentAssertions;

namespace Featureban.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void PlayersWithIdenticalName_ShouldNotBeEqual()
        {
            var firstPlayer = new Player("JD", coin: null);
            var secondPlayer = new Player("JD", coin: null);

            var result = firstPlayer.Equals(secondPlayer);

            result.Should().BeFalse();
        }
    }
}
