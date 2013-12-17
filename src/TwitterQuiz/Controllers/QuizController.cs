﻿using System;
using System.Web.Mvc;
using EventStore.ClientAPI;
using Raven.Client.Linq;
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
        private readonly QuizLogic _quizLogic;
        private readonly IDocumentSession _documentSession;
        public QuizController(IEventStoreConnection eventStoreConnection, IDocumentSession documentSession)
        {
            _quizLogic = new QuizLogic(eventStoreConnection);
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

        [HttpPost]
        public ActionResult New(EditQuizViewModel model)
        {
            var quiz = model.ToQuizModel();
            _documentSession.Store(quiz);
            _documentSession.SaveChanges();
            // Temp raven cop out to get the front end working whilst I figure out CQRS...
            //_quizLogic.CreateNewQuiz(model.ToQuizModel(), User.Identity.Name);

            return RedirectToAction("Edit", "Quiz", new { id = quiz.Id });
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
            return View(model);
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

        public ActionResult Play(int id)
        {
            var quizInProgress = _quizLogic.GetStartedQuiz(id, User.Identity.Name);
            var model = new PlayQuizViewModel(quizInProgress);
            return View(model);
        }

        public ActionResult Player(int id, string player)
        {
            var quizInProgress = _quizLogic.GetStartedQuiz(id, User.Identity.Name);
            QuizPlayerViewModel model = new QuizPlayerViewModel(quizInProgress, player);
            return View(model);
        }
    }
}