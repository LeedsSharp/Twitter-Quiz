using System;
using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Tweet { get; set; }
        public IList<Answer> Answers { get; set; }
    }
}