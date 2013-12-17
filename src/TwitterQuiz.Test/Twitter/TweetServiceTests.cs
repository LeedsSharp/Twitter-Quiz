using System.Linq;
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

        [Test]
        public void Can_get_direct_messages()
        {
            // Arrange
            var tweetService = new TweetService();

            // Act
            var dms = tweetService.GetDMs();

            // Assert
            Assert.IsNotNull(dms);
            Assert.IsTrue(dms.Count() <= 20);
            var latestDirectMessage = dms.FirstOrDefault();
            tweetService.Tweet(latestDirectMessage.SenderScreenName + " said " + latestDirectMessage.Text);
        }
    }
}