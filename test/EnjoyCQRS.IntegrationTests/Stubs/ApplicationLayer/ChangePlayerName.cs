using System;
using EnjoyCQRS.Commands;

namespace EnjoyCQRS.IntegrationTests.Stubs.ApplicationLayer
{
    public class ChangePlayerName : Command
    {
        public int Player { get; }
        public string Name { get; }

        public ChangePlayerName(Guid aggregateId, int player, string name) : base(aggregateId)
        {
            Player = player;
            Name = name;
        }
    }
}