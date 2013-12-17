using System.Linq;
using System.Web.Mvc;
using EventStore.ClientAPI;
using Raven.Client;
using Raven.Client.Linq;
using TwitterQuiz.Domain;
using TwitterQuiz.ViewModels.Home;
using TwitterQuiz.ViewModels.Quiz;

namespace TwitterQuiz.Controllers
{
    public class HomeController : Controller
    {
        //private readonly QuizLogic _quizLogic;
        private readonly IDocumentSession _documentSession;
        public HomeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
            //_quizLogic = new QuizLogic(eventStoreConnection);
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var quizzes = _documentSession.Query<Quiz>().Where(x => x.Owner == User.Identity.Name);
                foreach (var quiz in quizzes)
                {
                    var quizmodel = new QuizListViewModel
                    {
                        Description = quiz.Description,
                        Name = quiz.Name,
                        Id = quiz.Id,
                        Start = quiz.StartDate
                    };
                    model.Quizzes.Add(quizmodel);
                }
            }

            return View(model);
        }

    }
}
