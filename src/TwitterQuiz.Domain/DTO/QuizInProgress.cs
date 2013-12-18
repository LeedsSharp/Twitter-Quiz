using System.Collections.Generic;
using TwitterQuiz.Domain.Account;

namespace TwitterQuiz.Domain.DTO
{
    public class QuizInProgress
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Host { get; set; }
        public IList<RoundInProgress> Rounds { get; set; }
        public bool Complete { get; set; }

        public QuizInProgress()
        {
            Rounds = new List<RoundInProgress>();
        }

        public void Initialize(Quiz quiz)
        {
            Id = quiz.Id;
            InternalName = quiz.InternalName;
            Name = quiz.Name;
            Description = quiz.Description;
            Host = quiz.HostUser;
        }
    }

    public class RoundInProgress
    {
        public string Name { get; set; }
        public int Sequence { get; set; }
        public IList<QuestionInProgress> Questions { get; set; }

        public RoundInProgress()
        {
            Questions = new List<QuestionInProgress>();
        }

        public RoundInProgress(Round round)
        {
            Name = round.Name;
            Sequence = round.Sequence;
            Questions = new List<QuestionInProgress>();
        }
    }

    public class QuestionInProgress
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public List<Answer> Replies { get; set; }

        public QuestionInProgress(Question question)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            Replies = new List<Answer>();
        }
    }
}
