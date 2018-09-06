using System;
using System.Collections.Generic;
using System.Linq;
using Featureban.Domain.Common;
using Featureban.Domain.Enums;

namespace Featureban.Domain
{
    public class Board : Entity
    {
        public IReadOnlyList<WorkItem> WorkItems => _workItems.AsReadOnly();
        public uint WipLimit { get; }

        private readonly List<WorkItem> _workItems;

        public Board(uint wipLimit)
        {
            WipLimit = wipLimit;
            _workItems = new List<WorkItem>();
        }

        public bool TryAssignWorkItemTo(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "WorkItem must be assigned to player");

            if (IsWipLimitReachedFor(WorkItemStatus.InDevelopment))
                return false;
            
            var workItem = new WorkItem(player);
            workItem.ChangeStatusTo(WorkItemStatus.InDevelopment);
            _workItems.Add(workItem);
            return true;
        }

        public bool TryMoveWorkItemRight(WorkItem workItem)
        {
            if (workItem.IsComplete)
                throw new InvalidOperationException("Can't move completed work item");

            var workItemNextStatus = workItem.Status + 1;

            if (IsWipLimitReachedFor(workItemNextStatus))
                return false;

            workItem.ChangeStatusTo(workItemNextStatus);
            return true;
        }

        private bool IsWipLimitReachedFor(WorkItemStatus status)
        {
            if (WipLimit == 0 || status == WorkItemStatus.Todo || status == WorkItemStatus.Complete)
                return false;

            return _workItems.Count(_ => _.Status == status) == WipLimit;
        }

        public void BlockWorkItem(WorkItem workItem)
        {
            workItem.ChangeStateTo(WorkItemState.Blocked);
        }

        public void UnblockWorkItem(WorkItem workItem)
        {
            workItem.ChangeStateTo(WorkItemState.Available);
        }

        public IReadOnlyList<WorkItem> GetOtherPlayersWorkItemsFor(Player player)
        {
            return WorkItems.Where(_ => _.Player != this && _.IsAvailable).ToList().AsReadOnly();
        }

        public IReadOnlyList<WorkItem> GetWorkItemsFor(Player player)
        {
            return WorkItems.Where(_ => _.Player == player).ToList().AsReadOnly();
        }
    }
}
