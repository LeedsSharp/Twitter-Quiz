using System;
using System.Collections.Generic;
using System.Linq;
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
                    string.Format("The quiz {0} is about to begin", quiz.Name),
                    string.Format("Questions are multiple choice and will be tweeted every {0} minutes followed by each option", quiz.FrequencyOfQuestions),
                    "To answer DM this account before the next question is tweeted with the corresponding letter. e.g. A",
                    "Good Luck!"
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

    internal class CompleteQuiz : IQuizAction
    {
        public string[] GetTweetsForAction(Quiz quiz)
        {
            return new[]
                {
                    "The quiz is over"
                };
        }

        public void UpdateQuiz(Quiz quiz)
        {
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