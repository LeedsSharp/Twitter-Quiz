using System;
using System.Collections.Generic;

namespace TwitterQuiz.Domain
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public int FrequencyOfQuestions { get; set; } // The number of minutes the questions are tweeted
        public int FrequencyOfAnswers { get; set; } // The number of minutes the correct answers are tweeted at the end of the quiz
        public IList<Question> Questions { get; set; }
        public Player Winner { get; set; }
    }
}