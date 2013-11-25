namespace TwitterQuiz.ViewModels.Quiz
{
    public class NewQuizViewModel
    {
        public QuizDetailsViewModel Details { get; set; }

        public Domain.Quiz ToQuizModel()
        {
            return new Domain.Quiz
                {
                    Name = Details.Name,
                    Description = Details.Description,
                    Host = Details.Host,
                    StartDate = Details.StartDate,
                    FrequencyOfAnswers = Details.FrequencyOfAnswers.HasValue ? Details.FrequencyOfAnswers.Value : 10,
                    FrequencyOfQuestions = Details.FrequencyOfQuestions.HasValue ? Details.FrequencyOfQuestions.Value : 3
                };
        }
    }
}