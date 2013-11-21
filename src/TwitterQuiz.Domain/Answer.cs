using System;

namespace TwitterQuiz.Domain
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Player Player { get; set; }
        public string DirectMessage { get; set; }
    }
}