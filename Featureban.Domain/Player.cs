using System.Collections.Generic;
using System.Linq;
using Featureban.Domain.Common;
using Featureban.Domain.Enums;

namespace Featureban.Domain
{
    public class Player : Entity
    {
        public string Name { get; }
            
        private readonly Coin _coin;
        private readonly Board _board;

        public Player(string name, Board board, Coin coin)
        {
            Name = name;
            _board = board;
            _coin = coin;
        }

        public CoinFlipResult FlipCoin() => _coin.Flip();

        public void BlockWorkItem(WorkItem workItem) => workItem.ChangeStateTo(WorkItemState.Blocked);

        public void UnblockWorkItem(WorkItem workItem) => workItem.ChangeStateTo(WorkItemState.Available);

        private bool TryTakeNewWorkItem() => _board.TryAssignWorkItemTo(this);

        private bool TryMoveWorkItemRight(WorkItem workItem) => _board.TryMoveWorkItemRight(workItem);

        private bool TryHelp() => _board
                .GetOtherPlayersWorkItemsFor(this)
                .Any(item => _board.TryMoveWorkItemRight(item));

        public IReadOnlyList<WorkItem> GetWorkItems(WorkItemStatus status) =>
            _board.GetWorkItemsFor(this).Where(_ => _.Status == status).ToList().AsReadOnly();
    }
}
