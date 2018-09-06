namespace Featureban.Domain.Tests.DSL
{
    internal static class Create
    {
        public static PlayerBuilder Player => new PlayerBuilder();

        public static BoardBuilder Board => new BoardBuilder();
    }
}
