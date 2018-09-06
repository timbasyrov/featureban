using System;
using Featureban.Domain.Common;
using Featureban.Domain.Enums;

namespace Featureban.Domain
{
    public class WorkItem : Entity
    {
        public WorkItemState State { get; private set; }

        public WorkItemStatus Status { get; private set; }

        public Player Player { get; }

        public WorkItem(Player player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            State = WorkItemState.Available;
            Status = WorkItemStatus.Todo;
        }

        public bool IsBlocked => State == WorkItemState.Blocked;

        public void Block() => State = WorkItemState.Blocked;

        public void Unblock() => State = WorkItemState.Available;
    }
}
