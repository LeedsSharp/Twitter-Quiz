using System.Web;
using System.Web.Mvc;
using SimpleAuthentication.Mvc;
using TwitterQuiz.ViewModels;

namespace TwitterQuiz.Controllers
{
    public class AuthenticationCallbackProvider : IAuthenticationCallbackProvider
    {
        public ActionResult Process(HttpContextBase context, AuthenticateCallbackData model)
        {
            return new ViewResult
            {
                ViewName = "AuthenticateCallback",
                ViewData = new ViewDataDictionary(new AuthenticateCallbackViewModel
                {
                    AuthenticatedClient = model.AuthenticatedClient,
                    Exception = model.Exception,
                    ReturnUrl = model.ReturnUrl
                })
            };
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