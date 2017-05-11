using System.Threading.Tasks;
using Cars.Commands;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestCommandHandler2 : ICommandHandler<TestCommand2>
    {
        public Task ExecuteAsync(TestCommand2 command)
        {
            command.WasHandled = true;

            return Task.CompletedTask;
        }
    }
}