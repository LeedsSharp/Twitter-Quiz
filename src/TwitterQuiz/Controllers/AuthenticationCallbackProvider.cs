using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EventStore.ClientAPI;
using SimpleAuthentication.Mvc;
using TwitterQuiz.Domain.Account;
using TwitterQuiz.EventStore;
using TwitterQuiz.ViewModels.Home;

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