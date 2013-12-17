using System;
using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Quiz
    {
        public int Id { get; set; }
        public string InternalName { get { return string.Format("{0}-Quiz-{1}", Owner.Replace(" ", ""), Id); }}
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public string Host { get; set; }
        public DateTime StartDate { get; set; }
        public int FrequencyOfQuestions { get; set; } // The number of minutes the questions are tweeted
        public int FrequencyOfAnswers { get; set; } // The number of minutes the correct answers are tweeted at the end of the quiz
        public IList<Round> Rounds { get; set; }
        public Player Winner { get; set; }

        public Quiz()
        {
            Rounds = new List<Round>();
        }

        public static Quiz SampleQuiz(int id, Random r, string username)
        {
            var numOfRounds = r.Next(1, 5);
            var quiz = new Quiz
            {
                Name = string.Format("Quiz {0}", id),
                Description = string.Format("Sample quiz number {0}", id),
                Owner = username,
                Host = "LeedsSharp",
                StartDate = DateTime.Now.AddHours(1),
                FrequencyOfQuestions = r.Next(1, 10),
                FrequencyOfAnswers = r.Next(1, 10),
                Rounds = new List<Round>(numOfRounds)
            };
            for (int i = 0; i < numOfRounds; i++)
            {
                quiz.Rounds.Add(Round.SampleRound(i, r));
            }
            return quiz;
        }
    }
}