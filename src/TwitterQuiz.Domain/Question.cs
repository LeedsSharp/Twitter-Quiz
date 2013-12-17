using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Question
    {
        public string Tweet { get; set; }
        public int Sequence { get; set; }
        public IList<PossibleAnswer> PossibleAnswers { get; set; }

        public Question()
        {
            PossibleAnswers = new List<PossibleAnswer>();
        }
    }
}