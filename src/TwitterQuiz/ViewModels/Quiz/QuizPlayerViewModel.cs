using TwitterQuiz.Domain.DTO;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuizPlayerViewModel
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public bool IsComplete { get; set; }
        public string Usernamne { get; set; }

        public QuizPlayerViewModel(QuizInProgress quiz, string username)
        {
            Id = quiz.Id;
            InternalName = quiz.InternalName;
            Name = quiz.Name;
            Description = quiz.Description;
            Host = quiz.Host;
            IsComplete = quiz.Complete;
            Usernamne = username;
        }
    }
}