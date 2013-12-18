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
        private const string ConsumerKey = "RMEjl9PUttoX3jl34Bb3iQ";
        private const string ConsumerSecret = "vpIEH6sBorbEo39JRsUTFivIcEX5e8EV3sLPHj2u54";
        private const string AccessToken = "183268831-4nxoXhFvGuiv74lI17SUqU6v82GX4q67IIH46lAY";
        private const string AccessTokenSecret = "8yXYxa0AiCn02aNgngKFdl2Yhe8NZgipmKgGa1cUuRE9m";

        public TweetService()
        {
            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret };
            twitterService = new TwitterService(twitterClientInfo);
        }

        public string GetAuthorizeUri()
        {

            // This is the registered callback URL
            OAuthRequestToken requestToken = twitterService.GetRequestToken("http://localhost:12347/Authorize/AuthorizeCallback");

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = twitterService.GetAuthorizationUri(requestToken);
            return uri.ToString();
        }

        public OAuthAccessToken GetAccessToken(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };

            // Step 3 - Exchange the Request Token for an Access Token
            return twitterService.GetAccessToken(requestToken, oauth_verifier);
        }

        public TwitterUser GetUserCredentials(OAuthAccessToken accessToken)
        {
            // Step 4 - User authenticates using the Access Token
            twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            var options = new VerifyCredentialsOptions();
            return twitterService.VerifyCredentials(options);
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
