using TweetSharp;

namespace TwitterQuiz.AppServices
{
    public class TweetService
    {
        private readonly TwitterService twitterService;
        private const string ConsumerKey = "cCUfLlJdwNanXcdTFPaw";
        private const string ConsumerSecret = "LeLyBeYm655HdtdAJnBr74NDZ7DXWKgxmVQomhN1Y";
        private const string AccessToken = "576199065-QkMWPvETEvstaTkC2ksQ8Y3tvrJip0XDjTVgEZu7";
        private const string AccessTokenSecret = "ydDC3XFV98i8GHihwbMkhD5VNGrbtRE0B0iQM6Mtj0nAj";

        public TweetService()
        {
            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret };
            twitterService = new TwitterService(twitterClientInfo);
        }

        public void Tweet(string message)
        {
            twitterService.AuthenticateWith(AccessToken, AccessTokenSecret);
            twitterService.SendTweet(new SendTweetOptions { Status = message });
        }
    }
}
