using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class AnswerViewModel
    {
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }

        public AnswerViewModel()
        {

        }

        public AnswerViewModel(PossibleAnswer answer)
        {
            Answer = answer.Answer;
            IsCorrect = answer.IsCorrect;
        }
    }
}