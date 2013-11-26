using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Round
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public IList<Question> Questions { get; set; }

        public Round()
        {
            Questions = new List<Question>();
        }

        internal static Round SampleRoundA()
        {
            return new Round
            {
                Name = "C# interview questions",
                Sequence = 1,
                Questions = new List<Question>
                {
                    new Question { Tweet = "What does immutable mean?" },
                    new Question { Tweet = "How do you prevent a class from being inherited?" },
                    new Question { Tweet = "What is the term used to describe converting a value type to a reference type?" }
                }
            };
        }

        internal static Round SampleRoundB()
        {
            return new Round
            {
                Name = "SQL server questions",
                Sequence = 1,
                Questions = new List<Question>
                {
                    new Question { Tweet = "What is a CTE?" },
                    new Question { Tweet = "How do you declare a local temp table?" }
                }
            };
        }
    }
}
