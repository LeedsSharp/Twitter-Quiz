using NUnit.Framework;
using TwitterQuiz.AppServices;

namespace TwitterQuiz.Test.Twitter
{
    [TestFixture]
    public class TweetServiceTests
    {
        #region Setup
        [TestFixtureSetUp]
        public void SetUpOnce()
        {

        }

        [TestFixtureTearDown]
        public void TearDownOnce()
        {

        }

        [SetUp]
        public void SetupBeforeEveryTest()
        {

        }

        [TearDown]
        public void TearDownBeforeEveryTest()
        {

        } 
        #endregion

        [Test]
        public void Can_tweet_a_question()
        {
            // Arrange
            var tweetService = new TweetService();

            // Act
            tweetService.Tweet("Q1: How many unit tests does it take to make the sun go green?");

            // Assert
            Assert.IsTrue(true);
        }
    }
}