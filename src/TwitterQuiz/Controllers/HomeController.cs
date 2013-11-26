using System.Web.Mvc;
using EventStore.ClientAPI;
using TwitterQuiz.EventStore.Logic;
using TwitterQuiz.ViewModels.Home;
using TwitterQuiz.ViewModels.Quiz;

namespace TwitterQuiz.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuizLogic _quizLogic;
        public HomeController(IEventStoreConnection eventStoreConnection)
        {
            _quizLogic = new QuizLogic(eventStoreConnection);
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewBag.Username = User.Identity.Name;

            var model = new HomeIndexViewModel();

            if (User.Identity.IsAuthenticated)
            {
                var quizzes = _quizLogic.GetQuizzes(User.Identity.Name);
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
