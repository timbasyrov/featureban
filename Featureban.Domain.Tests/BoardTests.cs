using System.Linq;
using Featureban.Domain.Enums;
using Featureban.Domain.Tests.DSL;
using Xunit;
using FluentAssertions;

namespace Featureban.Domain.Tests
{
    public class BoardTests
    {
        [Fact]
        public void ShouldAllowTakeNewWorkItem_WhenWipLimitIsNotReached()
        {
            var board = Create.Board.WithLimit(1).Please();
            var player = Create.Player.WithBoard(board).Please();

            var result = board.TryAssignNewWorkItemTo(player);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotAllowTakeNewWorkItem_WhenWipLimitIsReached()
        {
            var board = Create.Board.WithLimit(1).Please();
            var player = Create.Player.WithBoard(board).Please();
            board.TryAssignNewWorkItemTo(player);

            var result = board.TryAssignNewWorkItemTo(player);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotAllowMoveWorkItem_WhenLimitIsReachedForNextStatus()
        {
            var board = Create.Board.WithLimit(1).Please();
            var firstPlayer = Create.Player.WithBoard(board).Please();
            var secondPlayer = Create.Player.WithBoard(board).Please();
            board.TryAssignNewWorkItemTo(firstPlayer);
            var firstMoveResult = board.TryMoveWorkItem(firstPlayer.WorkItems.Single());
            board.TryAssignNewWorkItemTo(secondPlayer);

            var secondMoveResult = board.TryMoveWorkItem(secondPlayer.WorkItems.Single());

            firstMoveResult.Should().BeTrue();
            secondMoveResult.Should().BeFalse();
        }

        [Fact]
        public void ShouldChangeWorkItemStatusToNextStatus_WhenMoveWorkItem()
        {
            var board = Create.Board.Please();
            var player = Create.Player.WithBoard(board).Please();
            board.TryAssignNewWorkItemTo(player);
            var workItem = player.WorkItems.Single();
            var workItemPreviousStatus = workItem.Status;

            board.TryMoveWorkItem(workItem);

            workItem.Status.Should().Be(workItemPreviousStatus.Next());
        }
    }
}
