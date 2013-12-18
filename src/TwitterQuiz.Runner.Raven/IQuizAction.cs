using System;
using TwitterQuiz.Domain;

namespace TwitterQuiz.Runner.Raven
{
    internal interface IQuizAction
    {
        void CarryOutAction(Quiz quiz);
    }

    internal class StartQuiz : IQuizAction
    {
        public void CarryOutAction(Quiz quiz)
        {
            quiz.Status = QuizStatus.InProgress;
        }
    }

    internal class StartRound : IQuizAction
    {
        public void CarryOutAction(Quiz quiz)
        {
            quiz.NextRound.NextQuestion.DateSent = DateTime.Now;
        }
    }

    internal class StartQuestion : IQuizAction
    {
        public void CarryOutAction(Quiz quiz)
        {
            quiz.ActiveRound.NextQuestion.DateSent = DateTime.Now;
        }
    }

    internal class CompleteQuiz : IQuizAction
    {
        public void CarryOutAction(Quiz quiz)
        {
            quiz.Status = QuizStatus.Complete;
        }
    }

    internal class Sleep : IQuizAction
    {
        public void CarryOutAction(Quiz quiz)
        {
        }
    }
}