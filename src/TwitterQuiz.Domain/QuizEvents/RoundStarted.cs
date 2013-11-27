namespace TwitterQuiz.Domain.QuizEvents
{
    public class RoundStarted
    {
        public string Host { get; set; }
        public int Round { get; set; }
        public string Tweet { get; set; }
    }
}
