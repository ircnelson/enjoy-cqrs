using System.Threading.Tasks;
using Cars.Commands;

namespace Cars.Grace.UnitTests.Stubs
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task ExecuteAsync(TestCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}