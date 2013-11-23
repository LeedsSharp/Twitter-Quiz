using FluentAssertions;
using NUnit.Framework;
using TwitterQuiz.EventStore;

namespace TwitterQuiz.Test.EventStore
{
    [TestFixture]
    public class RepositoryExtensionsTestFixture
    {
        [TestCase("User", "Users")]
        [TestCase("Question", "Questions")]
        [TestCase("Answer", "Answers")]
        [TestCase("Goose", "Geese")]
        public void RepositoryExtensions_Pluralize_ReturnsExpectedResult(string input, string expextedResult)
        {
            expextedResult.Should().BeEquivalentTo(input.Pluralize());
        }
    }
}
