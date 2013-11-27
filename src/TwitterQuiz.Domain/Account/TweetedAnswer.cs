namespace TwitterQuiz.Domain.Account
{
    public class TweetedAnswer
    {
        public Host Host { get; set; }
        public int Round { get; set; }
        public int Question { get; set; }
        public string QuizName { get; set; }
        public string Tweet { get; set; }
    }
}
