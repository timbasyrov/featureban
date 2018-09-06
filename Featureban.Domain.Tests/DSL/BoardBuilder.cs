namespace Featureban.Domain.Tests.DSL
{
    public class BoardBuilder
    {
        private uint _wipLimit;

        public BoardBuilder WithLimit(uint wipLimit)
        {
            _wipLimit = wipLimit;
            return this;
        }

        public Board Please()
        {
            return new Board(_wipLimit);
        }

    }
}
