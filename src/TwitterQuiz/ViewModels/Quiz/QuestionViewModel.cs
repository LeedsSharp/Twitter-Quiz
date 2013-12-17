using System.Collections.Generic;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuestionViewModel
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public List<AnswerViewModel> PossibleAnswers { get; set; }

        public QuestionViewModel()
        {
            PossibleAnswers = new List<AnswerViewModel>();
        }

        public QuestionViewModel(Domain.Question question)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            PossibleAnswers = new List<AnswerViewModel>();
            foreach (var answer in question.PossibleAnswers)
            {
                PossibleAnswers.Add(new AnswerViewModel(answer));
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