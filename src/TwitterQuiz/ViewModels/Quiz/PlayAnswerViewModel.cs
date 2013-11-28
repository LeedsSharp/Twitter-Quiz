using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayAnswerViewModel
    {
        public PlayAnswerViewModel(Answer reply)
        {
            Username = reply.Player.Username;
            ImageUrl = reply.Player.ImageUrl;
            Answer = reply.AnswerConent;
        }

        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Answer { get; set; }
    }
}