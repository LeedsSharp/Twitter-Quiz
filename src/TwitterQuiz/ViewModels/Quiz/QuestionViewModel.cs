using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuestionViewModel
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public List<string> PossibleAnswers { get; set; }

        public QuestionViewModel()
        {
            PossibleAnswers = new List<string>();
        }

        public QuestionViewModel(Domain.Question question)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            PossibleAnswers = new List<string>();
            foreach (var answer in question.PossibleAnswers)
            {
                PossibleAnswers.Add(answer);
            }
        }
    }
}