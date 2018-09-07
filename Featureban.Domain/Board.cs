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
        private readonly Backlog _backlog;

        public Board(uint wipLimit)
        {
            WipLimit = wipLimit;
            _workItems = new List<WorkItem>();
            _backlog = new Backlog();
        }

        public bool TryAssignNewWorkItemTo(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player), "WorkItem must be assigned to player");

            if (IsWipLimitReachedFor(WorkItemStatus.Todo.Next()))
                return false;

            var workItem = _backlog.CreateNewWorkItem();
            workItem.ChangeStatusTo(WorkItemStatus.Todo.Next());
            workItem.AssignToPlayer(player);
            _workItems.Add(workItem);
            return true;
        }

        public bool TryMoveWorkItem(WorkItem workItem)
        {
            if (workItem.IsComplete)
                throw new InvalidOperationException("Can't move completed work item");

            if (IsWipLimitReachedFor(workItem.Status.Next()))
                return false;

            workItem.ChangeStatusTo(workItem.Status.Next());
            return true;
        }

        private bool IsWipLimitReachedFor(WorkItemStatus status)
        {
            if (WipLimit == 0 || status == WorkItemStatus.Todo || status == WorkItemStatus.Complete)
                return false;

            return _workItems.Count(_ => _.Status == status) == WipLimit;
        }

        public IReadOnlyList<WorkItem> GetOtherPlayersWorkItemsFor(Player player)
        {
            return _workItems.Where(_ => _.Player != this).ToList().AsReadOnly();
        }

        public IReadOnlyList<WorkItem> GetWorkItemsFor(Player player)
        {
            return _workItems.Where(_ => _.Player == player).ToList().AsReadOnly();
        }
    }
}
