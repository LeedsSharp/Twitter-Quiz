using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class RoundViewModel
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public List<QuestionViewModel> Questions { get; set; }

        public RoundViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }

        public RoundViewModel(Domain.Round round)
        {
            Name = round.Name;
            Sequence = round.Sequence;
            Questions = new List<QuestionViewModel>();
            foreach (var question in round.Questions)
            {
                Questions.Add(new QuestionViewModel(question));
            }
        }

        public static RoundViewModel NewRound()
        {
            var round = new RoundViewModel();
            round.Questions.Add(QuestionViewModel.NewQuestion());
            return round;
        }
    }
}