using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuizIndexViewModel
    {
        public List<QuizListViewModel> Quizzes { get; set; }

        public QuizIndexViewModel()
        {
            Quizzes = new List<QuizListViewModel>();
        }
    }
}