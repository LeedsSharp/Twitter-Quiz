namespace TwitterQuiz.Domain.QuizEvents
{
    public class QuestionSent
    {
        public string Host { get; set; }
        public int Round { get; set; }
        public int Question { get; set; }
        public string Tweet { get; set; }
    }
}
