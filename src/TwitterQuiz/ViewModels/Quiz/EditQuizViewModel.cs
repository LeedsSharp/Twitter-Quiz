using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class EditQuizViewModel
    {
        public int Id { get; set; }
        public string StreamName { get; set; }
        public QuizDetailsViewModel Details { get; set; }
        public List<RoundViewModel> Rounds { get; set; }

        public EditQuizViewModel()
        {
            Rounds = new List<RoundViewModel>();
        }

        public EditQuizViewModel(Domain.Quiz quiz)
        {
            Details = new QuizDetailsViewModel(quiz);
            Rounds = new List<RoundViewModel>();
            foreach (var round in quiz.Rounds)
            {
                Rounds.Add(new RoundViewModel(round));
            }
        }
    }
}