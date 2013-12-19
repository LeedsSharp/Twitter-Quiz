namespace TwitterQuiz.Domain
{
    public class Answer
    {
        public Player Player { get; set; }
        public string AnswerConent { get; set; }
        public bool IsCorrect { get; set; }
    }
}