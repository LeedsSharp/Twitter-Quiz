using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PossibleAnswerViewModel
    {
        public PossibleAnswerViewModel(char letter, string answer)
        {
            Letter = letter;
            Answer = answer;
        }

        public PossibleAnswerViewModel(PossibleAnswer answer, char letter, bool done)
        {
            Letter = letter;
            Answer = answer.Answer;
            IsCorrect = answer.IsCorrect;
            AnswerClass = done && answer.IsCorrect ? "answer-correct" : "";
        }

        public char Letter { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public string AnswerClass { get; set; }
    }
}