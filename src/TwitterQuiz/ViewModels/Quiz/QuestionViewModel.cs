using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuestionViewModel
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public List<AnswerViewModel> PossibleAnswers { get; set; }

        public QuestionViewModel()
        {
            PossibleAnswers = new List<AnswerViewModel>();
        }

        public QuestionViewModel(Domain.Question question)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            PossibleAnswers = new List<AnswerViewModel>();
            foreach (var answer in question.PossibleAnswers)
            {
                PossibleAnswers.Add(new AnswerViewModel(answer));
            }
        }

        public static QuestionViewModel NewQuestion()
        {
            var question = new QuestionViewModel();
            question.PossibleAnswers.Add(new AnswerViewModel());
            return question;
        }
    }
}