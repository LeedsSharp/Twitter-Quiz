namespace TwitterQuiz.Domain.QuizEvents
{
    public class AnswerReceived
    {
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Answer { get; set; }
        public int Round { get; set; }
        public int Question { get; set; }
    }
}
