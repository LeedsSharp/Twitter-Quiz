using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using Raven.Database.Linq.PrivateExtensions;
using TwitterQuiz.Domain;

namespace TwitterQuiz.Runner.Raven
{
    internal interface IQuizAction
    {
        string[] GetTweetsForAction(Quiz quiz);
        void UpdateQuiz(Quiz quiz);
    }

    internal class StartQuiz : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            return new[]
                {
                    string.Format("The quiz \"{0}\" is about to begin", quiz.Name),
                    string.Format("Questions are multiple choice and will be tweeted every {0} minutes followed by each option", quiz.FrequencyOfQuestions),
                    string.Format("To answer DM this account before the next question is tweeted with the corresponding letter. For example: DM @{0} A", quiz.Host),
                    string.Format("This quiz is powered by @QuizimodoNET - http://www.quizimodo.net")
                };
        }

        public void UpdateQuiz(Quiz quiz)
        {
            quiz.Status = QuizStatus.InProgress;
        }
    }

    internal class StartRound : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var question = quiz.NextRound.NextQuestion;

            var response = new List<string>
                {
                    string.Format("Round {0}: {1}", quiz.NextRound.Sequence, quiz.NextRound.Name),
                    string.Format("Question {0}: {1}", quiz.NextRound.Questions.IndexOf(question), question.Tweet)
                };

            response.AddRange(question.PossibleAnswers.Select(x => string.Format("{0}: {1}", letters[question.PossibleAnswers.IndexOf(x)], x.Answer)));

            return response.ToArray();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            quiz.NextRound.NextQuestion.DateSent = DateTime.Now;
        }
    }

    internal class StartQuestion : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var question = quiz.ActiveRound.NextQuestion;

            var response = new List<string>
                {
                    string.Format("Question {0}: {1}", quiz.ActiveRound.Questions.IndexOf(question), question.Tweet)
                };

            response.AddRange(question.PossibleAnswers.Select(x => string.Format("{0}: {1}", letters[question.PossibleAnswers.IndexOf(x)], x.Answer)));

            return response.ToArray();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            quiz.ActiveRound.NextQuestion.DateSent = DateTime.Now;
        }
    }

    internal class GatherAnswers : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            return new string[0];
        }

        public void UpdateQuiz(Quiz quiz)
        {
            quiz.ActiveRound.ActiveQuestion.AnswersGathered = true;
        }
    }

    internal class CompleteQuiz : IQuizAction
    {
        private Dictionary<string, int> _results;
        public string[] GetTweetsForAction(Quiz quiz)
        {
            var replies = quiz.Rounds.SelectMany(x => x.Questions.SelectMany(q => q.Replies));

            var answers = replies.GroupBy(x => x.Player, x => x,
                (key, result) => new {Player = key, Score = result.Count(x => x.IsCorrect)})
                .ToDictionary(x => x.Player, x => x.Score);

            _results = answers.GroupBy(x => x.Key.Username, x => x.Value,
                (key, result) => new {Player = key, Score = result.Count(x => x == 1)})
                              .OrderBy(x => x.Score)
                              .ToDictionary(x => x.Player, x => x.Score);

            var response = new List<string>
                {
                    "The quiz is now over and the results are in..."
                };

            int index = 0;
            foreach (var result in _results)
            {
                int position = _results.Count() - index;
                if (position > 3)
                {
                    response.Add(string.Format("{0}th place goes to @{1}", result.Key, result.Value));
                }
                if (position == 3)
                {
                    response.Add("Time for the top three!");
                    response.Add(string.Format("3rd place goes to @{0} with a score of {1} - well done", result.Key, result.Value));
                }
                if (position == 2)
                {
                    response.Add(string.Format("2nd place goes to @{0} with a score of {1} - almost!", result.Key, result.Value));
                }
                if (position == 1)
                {
                    response.Add(string.Format("The winner is @{0} with a score of {1} - Congratulations!", result.Key, result.Value));
                }
                index++;
            }
            return response.ToArray();
        }

        public void UpdateQuiz(Quiz quiz)
        {
            quiz.Winner = _results.First().Key;
            quiz.Status = QuizStatus.Complete;
        }
    }

    internal class Sleep : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            return new string[0];
        }

        public void UpdateQuiz(Quiz quiz)
        {
        }
    }
}
