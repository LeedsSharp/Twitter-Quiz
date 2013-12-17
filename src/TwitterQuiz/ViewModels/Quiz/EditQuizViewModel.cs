using System.Collections.Generic;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class EditQuizViewModel
    {
        public int Id { get; set; }
        public string StreamName { get; set; }
        public QuizDetailsViewModel Details { get; set; }
        public List<RoundViewModel> Rounds { get; set; }
        public bool IsNew { get; set; }

        public EditQuizViewModel()
        {
            Rounds = new List<RoundViewModel>();
        }

        public EditQuizViewModel(Domain.Quiz quiz)
        {
            Id = quiz.Id;
            Details = new QuizDetailsViewModel(quiz);
            Rounds = new List<RoundViewModel>();
            if (quiz.Rounds != null)
            {
                foreach (var round in quiz.Rounds)
                {
                    Rounds.Add(new RoundViewModel(round));
                }
            }
        }

        public Domain.Quiz ToQuizModel()
        {
            var quiz = new Domain.Quiz
            {
                Name = Details.Name,
                Description = Details.Description,
                Owner = Details.Owner,
                Host = Details.Host,
                StartDate = Details.StartDate,
                FrequencyOfAnswers = Details.FrequencyOfAnswers.HasValue ? Details.FrequencyOfAnswers.Value : 10,
                FrequencyOfQuestions = Details.FrequencyOfQuestions.HasValue ? Details.FrequencyOfQuestions.Value : 3
            };
            quiz.Rounds = new List<Round>(Rounds.Count);
            foreach (var round in Rounds)
            {
                quiz.Rounds.Add(round.ToRoundModel());
            }
            return quiz;
        }
    }
}