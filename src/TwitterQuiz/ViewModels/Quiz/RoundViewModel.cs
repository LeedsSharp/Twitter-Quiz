using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class RoundViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Sequence { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public int Index { get; set; }

        public RoundViewModel()
        {
            Questions = new List<QuestionViewModel>();
        }

        public RoundViewModel(Round round, int roundIndex)
        {
            Name = round.Name;
            Sequence = round.Sequence;
            Index = roundIndex;
            Questions = new List<QuestionViewModel>();
            for (int i = 0; i < round.Questions.Count; i++)
            {
                Questions.Add(new QuestionViewModel(round.Questions[i], roundIndex, i));
            }
        }

        public static RoundViewModel NewRound()
        {
            var round = new RoundViewModel();
            round.Questions.Add(QuestionViewModel.NewQuestion());
            return round;
        }

        public Round ToRoundModel()
        {
            var round = new Round
                        {
                            Name = Name,
                            Sequence = Sequence,
                            Questions = new List<Question>(Questions.Count)
                        };
            foreach (var question in Questions)
            {
                round.Questions.Add(question.ToQuestionModel());
            }
            return round;
        }
    }
}