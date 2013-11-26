using System;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuizListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }

    }
}