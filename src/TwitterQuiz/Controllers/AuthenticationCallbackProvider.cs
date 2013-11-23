using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

            // Create the user if doesn't already exist
            if (!_eventStoreConnection.Exists<User>(x => x.AccessTokens.Any(a => a.PublicAccessToken == publicAccessToken && a.ProviderType == providerType)))
            {
                var newUser = User.FromAuthenticatedClient(model.AuthenticatedClient);
                _eventStoreConnection.AppendToStream(newUser, new { Created = DateTime.Now });
            }

            // TODO: Create auth token

            return new RedirectResult(model.ReturnUrl);
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