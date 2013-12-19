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

        private readonly TwitterService _twitterService;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private string _accessToken;
        private string _accessTokenSecret;

        public TweetService(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = _consumerKey, ConsumerSecret = _consumerSecret };
            _twitterService = new TwitterService(twitterClientInfo);
        }

        public TweetService()
        {
            _consumerKey = "RMEjl9PUttoX3jl34Bb3iQ";
            _consumerSecret = "vpIEH6sBorbEo39JRsUTFivIcEX5e8EV3sLPHj2u54";
            _accessToken = "183268831-4nxoXhFvGuiv74lI17SUqU6v82GX4q67IIH46lAY";
            _accessTokenSecret = "8yXYxa0AiCn02aNgngKFdl2Yhe8NZgipmKgGa1cUuRE9m";
            var twitterClientInfo = new TwitterClientInfo { ConsumerKey = _consumerKey, ConsumerSecret = _consumerSecret };
            _twitterService = new TwitterService(twitterClientInfo);
        }

        public string GetAuthorizeUri(string callback)
        {

            // This is the registered callback URL
            OAuthRequestToken requestToken = _twitterService.GetRequestToken(string.Format("http://localhost:12347/{0}", callback));

            // Step 2 - Redirect to the OAuth Authorization URL
            Uri uri = _twitterService.GetAuthorizationUri(requestToken);
            return uri.ToString();
        }

        public OAuthAccessToken GetAccessToken(string oauth_token, string oauth_verifier)
        {
            var requestToken = new OAuthRequestToken { Token = oauth_token };

            // Step 3 - Exchange the Request Token for an Access Token
            return _twitterService.GetAccessToken(requestToken, oauth_verifier);
        }

        public TwitterUser GetUserCredentials(OAuthAccessToken accessToken)
        {
            // Step 4 - User authenticates using the Access Token
            _twitterService.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
            var options = new VerifyCredentialsOptions();
            return _twitterService.VerifyCredentials(options);
        }

        public void Tweet(string message)
        {
            _twitterService.AuthenticateWith(_accessToken, _accessTokenSecret);
            _twitterService.SendTweet(new SendTweetOptions { Status = message });
        }

        public void Tweet(string accessToken, string accessTokenSecret, string message)
        {
            _twitterService.AuthenticateWith(accessToken, accessTokenSecret);
            _twitterService.SendTweet(new SendTweetOptions { Status = message });
        }

        public IEnumerable<TwitterDirectMessage> GetDMs()
        {
            _twitterService.AuthenticateWith(_accessToken, _accessTokenSecret);
            var dms = _twitterService.ListDirectMessagesReceived(new ListDirectMessagesReceivedOptions());
            if (_twitterService.Response == null)
            {
                throw new ApplicationException("Response was null");
            }
            if (_twitterService.Response.Error != null)
            {
                throw new ApplicationException(_twitterService.Response.Error.Message + "(" + _twitterService.Response.Error.Code + ")");
            }
            return dms;
        }
    }
}
