using System;
using System.Web.Mvc;
using EventStore.ClientAPI;
using TwitterQuiz.EventStore.Logic;
using TwitterQuiz.ViewModels.Quiz;

namespace TwitterQuiz.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly QuizLogic _quizLogic;
        public QuizController(IEventStoreConnection eventStoreConnection)
        {
            _quizLogic = new QuizLogic(eventStoreConnection);
        }

        public ActionResult Index()
        {
            var quizzes = _quizLogic.GetQuizzes(User.Identity.Name);
            var model = new QuizIndexViewModel();
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
            return View(model);
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new NewQuizViewModel
                {
                    StartDate = DateTime.Now.AddHours(1),
                    Host = User.Identity.Name
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(NewQuizViewModel model)
        {
            _quizLogic.CreateNewQuiz(model.ToQuizModel(), User.Identity.Name);
            return RedirectToAction("Index", "Home");
        }
    }
}