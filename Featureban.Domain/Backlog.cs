using Featureban.Domain.Common;
using static Featureban.Domain.Enums.WorkItemState;
using static  Featureban.Domain.Enums.WorkItemStatus;

namespace Featureban.Domain
{
    public class Backlog : ValueObject<Backlog>
    {
        public WorkItem CreateNewWorkItem() => new WorkItem(state: Available, status: Todo);

        protected override bool EqualsCore(Backlog other) => Equals(other);

        protected override int GetHashCodeCore() => GetHashCode();
    }
}
