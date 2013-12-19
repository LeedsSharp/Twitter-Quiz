﻿using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using Raven.Client;
using TwitterQuiz.AppServices;
using TwitterQuiz.Domain;

namespace TwitterQuiz.Runner.Raven
{
    class Program
    {
        private static TweetService _tweetService;
        static void Main(string[] args)
        {
            var consumerKey = ConfigurationManager.AppSettings["ConsumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["ConsumerSecret"];
            _tweetService = new TweetService(consumerKey, consumerSecret);
            
            var documentStore = RavenSessionProvider.DocumentStore;
            
            while (true)
            {
                using (var documentSession = documentStore.OpenSession())
                {
                    StartDueQuizzes(documentSession);

                    foreach (var activeQuiz in documentSession.Query<Quiz>().Where(x => x.Status == QuizStatus.InProgress))
                    {
                        var action = GetQuizAction(activeQuiz);
                        ApplyAction(action, activeQuiz);
                        //Console.WriteLine("{0} - Action: {1}", DateTime.Now, action.GetType());
                        documentSession.Store(activeQuiz);
                    }
                    documentSession.SaveChanges();
                }
                Thread.Sleep(1000);
            }
        }

        private static void ApplyAction(IQuizAction action, Quiz activeQuiz)
        {
            string[] tweets = action.GetTweetsForAction(activeQuiz);
            var accessToken = activeQuiz.HostUser.AccessTokens.First(x => x.ProviderType == "twitter");

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
                //_tweetService.Tweet(accessToken.PublicAccessToken, accessToken.TokenSecret, tweet);
                Thread.Sleep(5000);
            }

            action.UpdateQuiz(activeQuiz);
        }

        private static IQuizAction GetQuizAction(Quiz activeQuiz)
        {
            // if no rounds have been started then start the first
            if (activeQuiz.ActiveRound == null)
            {
                return new StartRound();
            }
            // If no questions have been sent yet for this round. Send the first one.
            if (activeQuiz.ActiveRound.ActiveQuestion == null)
            {
                return new StartRound();
            }
            // Shouldn't happen but if there us an active question with no sent value then send it
            if (!activeQuiz.ActiveRound.ActiveQuestion.DateSent.HasValue)
            {
                return new StartRound();
            }
            // If time has elapsed between questions then send the next question
            if (activeQuiz.ActiveRound.ActiveQuestion.DateSent.Value.AddMinutes(activeQuiz.FrequencyOfQuestions) < DateTime.Now)
            {
                // If next question is null then start the next round
                if (activeQuiz.ActiveRound.NextQuestion == null)
                {
                    // If the next round is null then the quiz is over
                    if (activeQuiz.NextRound == null)
                    {
                        return new CompleteQuiz();
                    }
                    return new StartRound();
                }
                return new StartQuestion();
            }
            return new Sleep();
        }

        private static void StartDueQuizzes(IDocumentSession documentSession)
        {
            foreach (var dueQuiz in documentSession.Query<Quiz>().Where(x => x.Status == QuizStatus.Draft && x.HostIsAuthenticated && x.StartDate < DateTime.Now))
            {
                ApplyAction(new StartQuiz(), dueQuiz);
                documentSession.Store(dueQuiz);
                documentSession.SaveChanges();
            }
        }
    }
}
