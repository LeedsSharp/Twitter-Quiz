using System;
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

        internal static Round SampleRound(int roundNumber, Random r)
        {
            var round =  new Round
            {
                Name = string.Format("Round {0}", roundNumber),
                Sequence = roundNumber,
                Questions = new List<Question>()
            };
            var num = r.Next(1, 10);
            for (int i = 0; i < num; i++)
            {
                var question = new Question
                    {
                        Tweet = string.Format("Sample Question {0}", i),
                        PossibleAnswers = new List<string>()
                    };
                var num2 = r.Next(1, 3);
                for (int j = 0; j < num2; j++)
                {
                    question.PossibleAnswers.Add(string.Format("Possible Answer {0}", j));
                }

                round.Questions.Add(question);
            }
            return round;
        }
    }
}
