using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayRoundViewModel
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public bool Started { get; set; }
        public IList<PlayQuestionViewModel> Questions { get; set; }

        public PlayRoundViewModel(Domain.Round round)
        {
            Name = round.Name;
            Sequence = round.Sequence;
            Questions = new List<PlayQuestionViewModel>();
            Started = round.RoundStarted;

            foreach (var question in round.Questions)
            {
                Questions.Add(new PlayQuestionViewModel(question));
            }
        }
    }
}