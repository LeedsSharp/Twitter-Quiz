using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Question
    {
        public string Tweet { get; set; }
        public IList<Answer> PossibleAnswers { get; set; }
    }
}