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

        public bool IsAvailable => State == WorkItemState.Available;

        public bool IsComplete => Status == WorkItemStatus.Complete;

        public void ChangeStatusTo(WorkItemStatus status) => Status = status;

        public void ChangeStateTo(WorkItemState state) => State = state;
    }
}
