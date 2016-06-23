using System;
using EnjoyCQRS.Events;

namespace EnjoyCQRS.IntegrationTests.Stubs.DomainLayer
{
    public class PlayerChangedName : DomainEvent
    {
        public int Player { get; }
        public string Name { get; }

        public PlayerChangedName(Guid aggregateId, int player, string name) : base(aggregateId)
        {
            Player = player;
            Name = name;
        }
    }
}