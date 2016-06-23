using System.Collections.Generic;
using EnjoyCQRS.Collections;
using FluentAssertions;
using Xunit;

namespace EnjoyCQRS.UnitTests.Metadata
{
    public class MetadataCollectionTests
    {
        [Fact]
        public void Should_instantiate_a_dictionary_given_a_keyPairValue()
        {
            List<KeyValuePair<string, string>> metadatas = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("key1", "value1"),
                new KeyValuePair<string, string>("key2", "value2")
            };

            var metadataCollection = new EventSource.Metadata(metadatas);

            metadataCollection.Should().HaveCount(2);
        }
    }
}