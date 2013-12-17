using System;
using System.Collections.Generic;
using TweetSharp;

namespace TwitterQuiz.AppServices
{
    public class TweetService
    {
        #region References

        /* https://dev.twitter.com/apps/5533272/show to manage the PubQuiziminator app */

        #endregion

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

        public IEnumerable<TwitterDirectMessage> GetDMs()
        {
            twitterService.AuthenticateWith(AccessToken, AccessTokenSecret);
            var dms = twitterService.ListDirectMessagesReceived(new ListDirectMessagesReceivedOptions());
            if (twitterService.Response == null)
            {
                throw new ApplicationException("Response was null");
            }
            if (twitterService.Response.Error != null)
            {
                throw new ApplicationException(twitterService.Response.Error.Message + "(" + twitterService.Response.Error.Code + ")");
            }
            return dms;
        }
    }
}
