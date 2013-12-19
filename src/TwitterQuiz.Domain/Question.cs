using System;
using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Question
    {
        public string Tweet { get; set; }
        public int Sequence { get; set; }
        public IList<PossibleAnswer> PossibleAnswers { get; set; }
        public DateTime? DateSent { get; set; }
        public IList<Answer> Replies { get; set; }
        public bool AnswersGathered { get; set; }

        public Question()
        {
            Replies = new List<Answer>();
            PossibleAnswers = new List<PossibleAnswer>();
        }
    }
}