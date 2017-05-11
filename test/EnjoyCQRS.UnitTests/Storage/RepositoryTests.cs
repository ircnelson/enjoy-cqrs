using System;
using Cars.EventSource.Storage;
using Cars.Logger;
using FluentAssertions;
using Moq;
using Xunit;

namespace Cars.UnitTests.Storage
{
    public class RepositoryTests
    {
        private const string CategoryName = "Unit";
        private const string CategoryValue = "Repository";

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Cannot_pass_null_instance_of_LoggerFactory()
        {
            var session = Mock.Of<ISession>();

            Action act = () => new Repository(null, session);

            act.ShouldThrowExactly<ArgumentNullException>();
        }

        [Trait(CategoryName, CategoryValue)]
        [Fact]
        public void Cannot_pass_null_instance_of_Session()
        {
            Action act = () => new Repository(new NoopLoggerFactory(), null);

            act.ShouldThrowExactly<ArgumentNullException>();
        }


    }
}