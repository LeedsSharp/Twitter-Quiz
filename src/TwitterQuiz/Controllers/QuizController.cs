using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventStore.ClientAPI;
using Raven.Client.Linq;
using TwitterQuiz.Domain.Account;
using TwitterQuiz.EventStore.Logic;
using TwitterQuiz.ViewModels.Quiz;
using Raven.Client;
using TwitterQuiz.Domain;

/*
 * For now I am dropping back to what I already know - Raven. 
 * Once we've got the LS Christmas pub quiz done I will be 
 * rewriting this as an excercise in using CQRS and Event Store
 * *******/

namespace TwitterQuiz.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IDocumentSession _documentSession;
        public QuizController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ActionResult Index()
        {
            var quizzes = _documentSession.Query<Quiz>().Where(x => x.Owner == User.Identity.Name);
            //var quizzes = _quizLogic.GetQuizzes(User.Identity.Name);
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
            var model = new EditQuizViewModel
                        {
                            Details = new QuizDetailsViewModel
                                      {
                                          StartDate = DateTime.Now.AddHours(1),
                                          Host = User.Identity.Name,
                                          Owner = User.Identity.Name
                                      },
                            IsNew = true
                        };
            model.Rounds.Add(RoundViewModel.NewRound());
            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            // Temp raven cop out to get the front end working whilst I figure out CQRS...
            //var quiz = _quizLogic.GetQuiz(id, User.Identity.Name);
            var model = new EditQuizViewModel(quiz);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditQuizViewModel model)
        {
            var quiz = model.ToQuizModel();

            var host = GetExistingHost(quiz.Host);
            if (host != null)
            {
                quiz.HostUser = host;
                quiz.HostIsAuthenticated = host.AccessTokens.Any(x => x.ProviderType == "twitter");
            }
            _documentSession.Store(quiz);
            _documentSession.SaveChanges();

            return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
        }

        private User GetExistingHost(string host)
        {
            return _documentSession.Query<User>().FirstOrDefault(x => x.Username == host);
        }

        [HttpPost]
        public ActionResult AddRound(EditQuizViewModel model)
        {
            model.Rounds.Add(new RoundViewModel());
            return PartialView("_EditQuiz", model);
        }

        [HttpGet]
        public ActionResult SmokeAndMirrors()
        {
            Random r = new Random();
            var NumOfQuizzes = r.Next(1, 5);
            for (int i = 0; i < NumOfQuizzes; i++)
            {
                var quiz = Quiz.SampleQuiz(i, r, User.Identity.Name);
                _documentSession.Store(quiz);
            }
            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Start(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            quiz.StartDate = DateTime.Now;
            quiz.Status = QuizStatus.InProgress;
            _documentSession.Store(quiz);
            _documentSession.SaveChanges();
            return RedirectToAction("Play", new {id});
        }

        public ActionResult Play(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            var model = new PlayQuizViewModel(quiz);
            return View(model);
        }

        public ActionResult Player(int id, string player)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            QuizPlayerViewModel model = new QuizPlayerViewModel(quiz, player);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            _documentSession.Delete(quiz);
            _documentSession.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Stop(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            quiz.StartDate = DateTime.Now.AddHours(1);
            quiz.Status = QuizStatus.Draft;
            _documentSession.Store(quiz);
            _documentSession.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Reset(int id)
        {
            var quiz = _documentSession.Load<Quiz>(id);
            foreach (var round in quiz.Rounds)
            {
                foreach (var question in round.Questions)
                {
                    question.DateSent = null;
                    question.Replies = new List<Answer>();
                }
            }
            quiz.Status = QuizStatus.Draft;
            quiz.StartDate = DateTime.Now.AddHours(1);
            _documentSession.Store(quiz);
            _documentSession.SaveChanges();
            return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
        }
    }
}