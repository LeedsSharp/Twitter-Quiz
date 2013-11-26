using System;
using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Quiz
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public DateTime StartDate { get; set; }
        public int FrequencyOfQuestions { get; set; } // The number of minutes the questions are tweeted
        public int FrequencyOfAnswers { get; set; } // The number of minutes the correct answers are tweeted at the end of the quiz
        public IList<Round> Rounds { get; set; }
        public Player Winner { get; set; }

        public static Quiz SampleQuiz()
        {
            return new Quiz
            {
                Name = "Leeds Sharp Pub Quiz",
                Description = "A fun quiz to do down the pub at the December Leeds Sharp meetup.",
                Host = "LeedsSharp",
                StartDate = DateTime.Now.AddHours(1),
                FrequencyOfQuestions = 3,
                FrequencyOfAnswers = 1,
                Rounds = new List<Round>{
                    Round.SampleRoundA(),
                    Round.SampleRoundB()
                }
            };
        }
    }
}