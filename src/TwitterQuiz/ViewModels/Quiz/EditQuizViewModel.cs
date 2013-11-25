namespace TwitterQuiz.ViewModels.Quiz
{
    public class EditQuizViewModel
    {
        public int Id { get; set; }
        public QuizDetailsViewModel Details { get; set; }

        public EditQuizViewModel()
        {
            
        }

        public EditQuizViewModel(Domain.Quiz quiz)
        {
            Details = new QuizDetailsViewModel(quiz);
        }
    }
}