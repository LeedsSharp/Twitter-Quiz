using System.ComponentModel.DataAnnotations;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class AnswerViewModel
    {
        [Required]
        public string Answer { get; set; }
        [Required]
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