using System.Collections.Generic;
using System.Linq;
using Featureban.Domain.Common;
using Featureban.Domain.Enums;
using static Featureban.Domain.Enums.WorkItemState;

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

        public void BlockWorkItem(WorkItem workItem) => workItem.ChangeStateTo(Blocked);

        public void UnblockWorkItem(WorkItem workItem) => workItem.ChangeStateTo(Available);

        public bool TryTakeNewWorkItem() => _board.TryAssignWorkItemTo(this);

        private bool TryMoveOrUnblockWorkItem(IReadOnlyList<WorkItem> workItems)
        {
            if (workItems
                .Where(_ => _.IsAvailable && !_.IsComplete)
                .Any(item => _board.TryMoveWorkItemRight(item)))
                return true;

            var blockedWorkItem = workItems.FirstOrDefault(_ => _.IsBlocked);
            if (blockedWorkItem == null)
                return false;

            UnblockWorkItem(blockedWorkItem);
            return true;

        }

        public IReadOnlyList<WorkItem> WorkItems => _board.GetWorkItemsFor(this);
        public IReadOnlyList<WorkItem> BlockedWorkItems => WorkItems.Where(_ => _.IsBlocked).ToList().AsReadOnly();
        public IReadOnlyList<WorkItem> AvailableWorkItems => WorkItems.Where(_ => _.IsAvailable).ToList().AsReadOnly();

        public void MakeMove(CoinFlipResult coinFlipResult)
        {
            if (coinFlipResult == CoinFlipResult.Tail)
            {                
                if (TryMoveOrUnblockWorkItem(WorkItems))
                    return;

                if (TryTakeNewWorkItem())
                    return;
                
                // Try to help other player
                TryMoveOrUnblockWorkItem(_board.GetOtherPlayersWorkItemsFor(this));
                return;
            }

            var availableWorkItem = WorkItems.FirstOrDefault(_ => _.IsAvailable && !_.IsComplete);
            if (availableWorkItem == null)
                return;

            BlockWorkItem(availableWorkItem);
            TryTakeNewWorkItem();
        }
    }
}
