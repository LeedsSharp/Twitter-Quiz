using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Raven.Client;
using TwitterQuiz.Domain;

namespace TwitterQuiz.Runner.Raven
{
    class Program
    {
        private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static void Main(string[] args)
        {
            var documentStore = RavenSessionProvider.DocumentStore;
            
            while (true)
            {
                using (var documentSession = documentStore.OpenSession())
                {
                    Console.WriteLine("tick {0}", DateTime.Now);
                    StartDueQuizzes(documentSession);

                    foreach (var activeQuiz in documentSession.Query<Quiz>().Where(x => x.Status == QuizStatus.InProgress))
                    {
                        if (activeQuiz.ActiveRound == null)
                        {
                            // Start the first round
                            StartRound(activeQuiz.NextRound);
                        }
                        else
                        {
                            if (activeQuiz.ActiveRound.ActiveQuestion == null)
                            {
                                // No questions have been sent yet for this round. Send the first one.
                                StartQuestion(activeQuiz.ActiveRound.NextQuestion);
                            }
                            else
                            {
                                if (activeQuiz.ActiveRound.ActiveQuestion.DateSent.HasValue)
                                {
                                    if (activeQuiz.ActiveRound.ActiveQuestion.DateSent.Value.AddMinutes(activeQuiz.FrequencyOfQuestions) < DateTime.Now)
                                    {
                                        if (activeQuiz.ActiveRound.NextQuestion == null)
                                        {
                                            if (activeQuiz.NextRound == null)
                                            {
                                                activeQuiz.Status = QuizStatus.Complete;
                                            }
                                            else
                                            {
                                                StartRound(activeQuiz.NextRound);
                                            }
                                        }
                                        else
                                        {
                                            StartQuestion(activeQuiz.ActiveRound.NextQuestion);
                                        }
                                    }
                                }
                                else
                                {
                                    StartQuestion(activeQuiz.ActiveRound.ActiveQuestion);
                                }
                            }
                        }

                    }
                    documentSession.SaveChanges();
                }
                Thread.Sleep(1000);
            }
        }

        private static void StartRound(Round round)
        {
            Console.WriteLine("Round {0}: {1}", round.Sequence, round.Name);
            StartQuestion(round.NextQuestion);
        }

        private static void StartQuestion(Question question)
        {
            question.DateSent = DateTime.Now;
            Console.WriteLine("Question {0}: {1}", question.Sequence, question.Tweet);
            for (int i = 0; i < question.PossibleAnswers.Count; i++)
            {
                Console.WriteLine("{0}: {1}", _letters[i], question.PossibleAnswers[i].Answer);
            }
        }

        private static void StartDueQuizzes(IDocumentSession documentSession)
        {
            foreach (var dueQuiz in documentSession.Query<Quiz>().Where(x => x.Status == QuizStatus.Draft && x.HostIsAuthenticated && x.StartDate < DateTime.Now))
            {
                Console.WriteLine("Starting Quiz: {0}", dueQuiz.Name);
                dueQuiz.Status = QuizStatus.InProgress;
                documentSession.Store(dueQuiz);
                documentSession.SaveChanges();
            }
        }
    }
}
