namespace TwitterQuiz.Domain
{
    public class TweetedQuestion
    {
        public Host Player { get; set; }
        public int Round { get; set; }
        public int Question { get; set; }
        public string QuizName { get; set; }
        public string Tweet { get; set; }
    }
}
