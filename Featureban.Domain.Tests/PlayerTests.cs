using Featureban.Domain.Enums;
using Xunit;
using FluentAssertions;
using Featureban.Domain.Tests.DSL;

namespace Featureban.Domain.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void PlayersWithIdenticalName_ShouldNotBeEqual()
        {
            var firstPlayer = Create.Player.WithName("JD").Please();
            var secondPlayer = Create.Player.WithName("JD").Please();

            var result = firstPlayer.Equals(secondPlayer);

            result.Should().BeFalse();
        }

        [Fact]
        public void PlayerShouldBlockAvailableWorkItemAndGetNewWorkItem_WhenCoinFlipResultIsHead_AndWipLimitIsNotReached()
        {
            var board = new Board(wipLimit: 5);
            var player = Create.Player.WithBoard(board).Please();
            player.TryTakeNewWorkItem();

            player.MakeMove(CoinFlipResult.Head);

            player.GetBlockedWorkItems().Count.Should().Be(1);
            player.GetAvailableWorkItems().Count.Should().Be(1);
        }

        [Fact]
        public void PlayerShouldBlockAvailableWorkItemAndDontGetNewWorkItem_WhenCoinFlipResultIsHead_AndWipLimitIsReached()
        {
            var board = new Board(wipLimit: 1);
            var player = Create.Player.WithBoard(board).Please();
            player.TryTakeNewWorkItem();

            player.MakeMove(CoinFlipResult.Head);

            player.GetBlockedWorkItems().Count.Should().Be(1);
            player.GetAvailableWorkItems().Count.Should().Be(0);
        }
    }
}
