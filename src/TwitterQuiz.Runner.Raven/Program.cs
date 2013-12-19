using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Raven.Client;
using TweetSharp;
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
                        GetAnswers(activeQuiz);

                        var action = GetQuizAction(activeQuiz);
                        ApplyAction(action, activeQuiz);
                        //Console.WriteLine("{0} - Action: {1}", DateTime.Now, action.GetType());
                        if (activeQuiz.Status == QuizStatus.Complete)
                        {
                            GetAnswers(activeQuiz);
                        }
                        documentSession.Store(activeQuiz);
                    }
                    documentSession.SaveChanges();
                }
                Thread.Sleep(1000);
            }
        }

        private static void GetAnswers(Quiz quiz)
        {
            var accessToken = quiz.HostUser.AccessTokens.First(x => x.ProviderType == "twitter");
            var dms = _tweetService.GetDMs(accessToken.PublicAccessToken, accessToken.TokenSecret).ToList();

            if (dms.Any(x => x.CreatedDate > quiz.StartDate))
            {
                foreach (var dm in dms.Where(x => x.CreatedDate > quiz.StartDate).OrderBy(x => x.CreatedDate))
                {
                    AddResponse(quiz, dm);
                }
            }
        }

        private static void AddResponse(Quiz quiz, TwitterDirectMessage response)
        {
            var answer = new Answer
                {
                    Player = new Player { Username = response.SenderScreenName, ImageUrl = response.Sender.ProfileImageUrl },
                    AnswerConent = response.Text
                };
            var responseTime = response.CreatedDate;

            quiz.Rounds.SelectMany(x => x.Questions).Where(x => x.DateSent < responseTime).OrderByDescending(x => x.DateSent).First().Replies.Add(answer);

            Console.WriteLine("{0}: {1}", answer.Player.Username, answer.AnswerConent);
        }

        private static void ApplyAction(IQuizAction action, Quiz activeQuiz)
        {
            string[] tweets = action.GetTweetsForAction(activeQuiz);
            var accessToken = activeQuiz.HostUser.AccessTokens.First(x => x.ProviderType == "twitter");

            action.UpdateQuiz(activeQuiz);
            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
                //_tweetService.Tweet(accessToken.PublicAccessToken, accessToken.TokenSecret, tweet);
                Thread.Sleep(5000);
            }
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
