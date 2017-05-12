using System;
using Cars.Commands;

namespace Cars.Testing.Shared.StubApplication.Commands.BarAggregate
{
    public class SpeakCommand : Command
    {
        public string Text { get; }

        public SpeakCommand(Guid aggregateId, string text) : base(aggregateId)
        {
            Text = text;
        }
    }
}