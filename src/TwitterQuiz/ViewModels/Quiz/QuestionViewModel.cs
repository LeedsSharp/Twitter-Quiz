using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuestionViewModel
    {
        [Required]
        public string Question { get; set; }
        public int Sequence { get; set; }
        public List<AnswerViewModel> PossibleAnswers { get; set; }
        public int Round { get; set; }
        public int Index { get; set; }

        public QuestionViewModel()
        {
            PossibleAnswers = new List<AnswerViewModel>();
        }

        public QuestionViewModel(Question question, int roundIndex, int index)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            Round = roundIndex;
            Index = index;
            PossibleAnswers = new List<AnswerViewModel>();
            for (int i = 0; i < question.PossibleAnswers.Count; i++)
            {
                PossibleAnswers.Add(new AnswerViewModel(question.PossibleAnswers[i], i));
            }
        }

        public static QuestionViewModel NewQuestion()
        {
            var question = new QuestionViewModel();
            question.PossibleAnswers.Add(new AnswerViewModel());
            return question;
        }

        public Question ToQuestionModel()
        {
            var question = new Question
                           {
                               Tweet = Question,
                               Sequence = Sequence,
                               PossibleAnswers = new List<PossibleAnswer>(PossibleAnswers.Count)
                           };
            foreach (var answer in PossibleAnswers)
            {
                question.PossibleAnswers.Add(new PossibleAnswer { Answer = answer.Answer, IsCorrect = answer.IsCorrect});
            }
            return question;
        }
    }
}