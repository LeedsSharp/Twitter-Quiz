namespace TwitterQuiz.Domain.QuizEvents
{
    public class QuizEnded
    {
        public string Host { get; set; }
        public string Tweet { get; set; }
    }
}
