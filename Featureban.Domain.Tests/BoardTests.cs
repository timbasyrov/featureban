using Xunit;
using FluentAssertions;

namespace Featureban.Domain.Tests
{
    public class BoardTests
    {
        [Fact]
        public void ShouldAllowTakeNewWorkItem_WhenWipLimitIsNotReached()
        {
            var board = new Board(wipLimit: 1);

            var result = board.TryAssignWorkItemTo(new Player("JD", board, new Coin()));

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldNotAllowTakeNewWorkItem_WhenWipLimitIsReached()
        {
            var board = new Board(wipLimit: 1);
            var player = new Player("JD", board, new Coin());
            board.TryAssignWorkItemTo(player);

            var result = board.TryAssignWorkItemTo(player);

            result.Should().BeFalse();
        }
    }
}
