using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SimpleAuthentication.Mvc;
using TwitterQuiz.Domain.Account;
using TwitterQuiz.ViewModels.Home;
using Raven.Client;

namespace TwitterQuiz.Controllers
{
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        //private readonly IEventStoreConnection _eventStoreConnection;
        private readonly IDocumentSession _documentSession;

        public AuthenticationCallbackProvider(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
            //_eventStoreConnection = eventStoreConnection;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            //var publicAccessToken = model.AuthenticatedClient.AccessToken.PublicToken;
            //var providerType = model.AuthenticatedClient.ProviderName;

            var user = User.FromAuthenticatedClient(model.AuthenticatedClient);

            if (!_documentSession.Query<User>().Any(x => x.Username == user.Username))
            {
                _documentSession.Store(user);
                _documentSession.SaveChanges();
            }
            else
            {
                var dbUser = _documentSession.Query<User>().First(x => x.Username == user.Username);
                var accessToken = user.AccessTokens.First();
                if (dbUser.AccessTokens.All(x => x.ProviderType != accessToken.ProviderType))
                {
                    dbUser.AccessTokens.Add(accessToken);
                    _documentSession.SaveChanges();
                }
            }
            // Create the user if doesn't already exist
            //if (!_eventStoreConnection.Exists<User>(x => x.AccessTokens.Any(a => a.PublicAccessToken == publicAccessToken && a.ProviderType == providerType)))
            //{
            //    _eventStoreConnection.AppendToStream(user, new { Created = DateTime.Now });
            //}

            // Create auth token
            var ticket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, ""); 

            var strEncryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
            context.Response.Cookies.Add(cookie);

            return new RedirectResult(model.ReturnUrl);
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            return new ViewResult
            {
                ViewName = "Index",
                ViewData = new ViewDataDictionary(new HomeIndexViewModel
                {
                    ErrorMessage = errorMessage
                })
            };
        }
    }
}