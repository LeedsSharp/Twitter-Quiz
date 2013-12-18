namespace TwitterQuiz.ViewModels.Quiz
{
    public class PossibleAnswerViewModel
    {
        public PossibleAnswerViewModel(char letter, string answer)
        {
            Letter = letter;
            Answer = answer;
        }

        public char Letter { get; set; }
        public string Answer { get; set; }
    }
}