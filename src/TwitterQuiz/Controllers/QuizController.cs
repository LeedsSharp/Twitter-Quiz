﻿using System;
using System.Web.Mvc;
using EventStore.ClientAPI;
using TwitterQuiz.EventStore.Logic;
using TwitterQuiz.ViewModels.Quiz;

namespace TwitterQuiz.Controllers
{
    public class QuizController : Controller
    {
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly QuizLogic _quizLogic;
        public QuizController(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
            _quizLogic = new QuizLogic(_eventStoreConnection);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            var model = new NewQuizViewModel
                {
                    StartDate = DateTime.Now.AddHours(1)
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult New(NewQuizViewModel model)
        {
            _quizLogic.AppendToStream(model.ToQuizModel());
            return View(model);
        }
    }
}