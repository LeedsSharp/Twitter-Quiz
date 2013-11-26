using System.Collections.Generic;
using TwitterQuiz.ViewModels.Quiz;

namespace TwitterQuiz.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public string ErrorMessage { get; set; }
        
        public List<QuizListViewModel> Quizzes { get; set; }

        public HomeIndexViewModel()
        {
            Quizzes = new List<QuizListViewModel>();
        }
    }
}