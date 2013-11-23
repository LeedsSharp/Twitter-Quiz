using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EventStore.ClientAPI;
using SimpleAuthentication.Mvc;
using TwitterQuiz.Domain.Account;
using TwitterQuiz.EventStore;
using TwitterQuiz.ViewModels;

namespace TwitterQuiz.Controllers
{
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public AuthenticationCallbackProvider(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            var publicAccessToken = model.AuthenticatedClient.AccessToken.PublicToken;
            var providerType = model.AuthenticatedClient.ProviderName;

            var user = User.FromAuthenticatedClient(model.AuthenticatedClient);

            // Create the user if doesn't already exist
            if (!_eventStoreConnection.Exists<User>(x => x.AccessTokens.Any(a => a.PublicAccessToken == publicAccessToken && a.ProviderType == providerType)))
            {
                _eventStoreConnection.AppendToStream(user, new { Created = DateTime.Now });
            }

            // TODO: Create auth token
            var ticket = new FormsAuthenticationTicket(1, // version 
                                                       user.Username, // user name
                                                       DateTime.Now, // create time
                                                       DateTime.Now.AddSeconds(30), // expire time
                                                       false, // persistent
                                                       ""); // user data, such as roles

            var strEncryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, strEncryptedTicket);
            context.Response.Cookies.Add(cookie);

            context.Response.Redirect(model.ReturnUrl);
            return null;
        }

        public ActionResult OnRedirectToAuthenticationProviderError(HttpContextBase context, string errorMessage)
        {
            return new ViewResult
            {
                ViewName = "AuthenticateCallback",
                ViewData = new ViewDataDictionary(new IndexViewModel
                {
                    ErrorMessage = errorMessage
                })
            };
        }
    }
}