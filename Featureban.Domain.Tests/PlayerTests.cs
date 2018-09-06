using System.Linq;
using Featureban.Domain.Enums;
using Xunit;
using FluentAssertions;
using Featureban.Domain.Tests.DSL;
using static Featureban.Domain.Enums.WorkItemState;

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
            var board = Create.Board.WithLimit(5).Please();
            var player = Create.Player.WithBoard(board).Please();
            player.TryTakeNewWorkItem();

            player.MakeMove(CoinFlipResult.Head);

            player.BlockedWorkItems.Count.Should().Be(1);
            player.AvailableWorkItems.Count.Should().Be(1);
        }

        [Fact]
        public void PlayerShouldBlockAvailableWorkItemAndDontGetNewWorkItem_WhenCoinFlipResultIsHead_AndWipLimitIsReached()
        {
            var board = Create.Board.WithLimit(1).Please();
            var player = Create.Player.WithBoard(board).Please();
            player.TryTakeNewWorkItem();

            player.MakeMove(CoinFlipResult.Head);

            player.BlockedWorkItems.Count.Should().Be(1);
            player.AvailableWorkItems.Count.Should().Be(0);
        }

        [Fact]
        public void PlayerShouldMoveAvailableWorkItem_WhenCoinFlipResultIsTail_AndWipLimitIsNotReached()
        {
            var board = Create.Board.WithLimit(5).Please();
            var player = Create.Player.WithBoard(board).Please();
            player.TryTakeNewWorkItem();
            var workItem = player.WorkItems.Single();
            var workItemPreviousStatus = workItem.Status;

            player.MakeMove(CoinFlipResult.Tail);

            workItem.Status.Should().Be(workItemPreviousStatus.Next());
        }

        [Fact]
        public void PlayerShouldUnblockBlockedWorkItem_WhenCoinFlipResultIsTail_AndPlayerHasNotAvailableWorkItems()
        {
            var player = Create.Player.Please();
            player.TryTakeNewWorkItem();
            var workItem = player.WorkItems.Single();
            player.BlockWorkItem(workItem);
            var workItemPreviousState = workItem.State;

            player.MakeMove(CoinFlipResult.Tail);

            workItemPreviousState.Should().Be(Blocked);
            workItem.State.Should().Be(Available);
        }

        [Fact]
        public void PlayerShouldGetNewWorkItem_WhenCoinFlipResultIsTail_AndPlayerHasNotAvailableOrBlockedWorkItems()
        {
            var player = Create.Player.Please();
            
            player.MakeMove(CoinFlipResult.Tail);

            player.AvailableWorkItems.Count.Should().Be(1);
        }
    }
}
