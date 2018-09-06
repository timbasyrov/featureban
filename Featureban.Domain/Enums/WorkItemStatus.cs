namespace Featureban.Domain.Enums
{
    public enum WorkItemStatus
    {
        Todo = 0,
        InDevelopment = 1,
        InTesting = 2,
        Complete = 3
    }

    public static class WorkItemStatusExtensions
    {
        public static WorkItemStatus Next(this WorkItemStatus status) => status + 1;
    }
}
