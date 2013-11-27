using System.Collections.Generic;
using TwitterQuiz.Domain.DTO;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayRoundViewModel
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public IList<PlayQuestionViewModel> Questions { get; set; }

        public PlayRoundViewModel(RoundInProgress round)
        {
            Name = round.Name;
            Sequence = round.Sequence;
            Questions = new List<PlayQuestionViewModel>();

            foreach (var question in round.Questions)
            {
                Questions.Add(new PlayQuestionViewModel(question));
            }
        }
    }
}