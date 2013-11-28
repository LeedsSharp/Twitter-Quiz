using System.Collections.Generic;
using TwitterQuiz.Domain.DTO;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayQuizViewModel
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public bool IsComplete { get; set; }
        public List<PlayRoundViewModel> Rounds { get; set; }

        public PlayQuizViewModel()
        {
            Rounds = new List<PlayRoundViewModel>();
        }

        public PlayQuizViewModel(QuizInProgress quiz)
        {
            Id = quiz.Id;
            InternalName = quiz.InternalName; 
            Name = quiz.Name;
            Description = quiz.Description;
            Host = quiz.Host;
            IsComplete = quiz.Complete;
            Rounds = new List<PlayRoundViewModel>();
            foreach (var round in quiz.Rounds)
            {
                Rounds.Add(new PlayRoundViewModel(round));
            }
        }
    }
}