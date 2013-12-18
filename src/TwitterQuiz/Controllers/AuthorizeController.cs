using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Raven.Abstractions.Extensions;
using Raven.Client;
using TwitterQuiz.AppServices;
using TwitterQuiz.Domain.Account;

namespace TwitterQuiz.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly IDocumentSession _documentSession;

        public AuthorizeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ActionResult AuthorizeViaTwitter()
        {
            var tweetService = new TweetService();
            var uri = tweetService.GetAuthorizeUri();
            return new RedirectResult(uri, false /*permanent*/);
        }

        public ActionResult AuthorizeCallback(string oauth_token, string oauth_verifier)
        {
            var tweetService = new TweetService();
            var token = tweetService.GetAccessToken(oauth_token, oauth_verifier);
            var userCreds = tweetService.GetUserCredentials(token);

            var user = Domain.Account.User.FromAuthenticatedTwitterUser(userCreds, token);

            if (!_documentSession.Query<User>().Any(x => x.Username == user.Username))
            {
                _documentSession.Store(user);
            }
            else
            {
                var existingUser = _documentSession.Query<User>().First(x => x.Username == user.Username);
                foreach (var accessToken in user.AccessTokens)
                {
                    if (existingUser.AccessTokens.Any(x => x.ProviderType == accessToken.ProviderType))
                    {
                        var toke = existingUser.AccessTokens.First(x => x.ProviderType == accessToken.ProviderType);
                        existingUser.AccessTokens.Remove(toke);
                    }
                }
                existingUser.AccessTokens.AddRange(user.AccessTokens);
                _documentSession.Store(existingUser);
            }


            _documentSession.SaveChanges();

            // Create auth token
            var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, "");

            var strEncryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
            HttpContext.Response.Cookies.Add(cookie);
            
            return RedirectToAction("Index", "Home");
        }
    }
}
