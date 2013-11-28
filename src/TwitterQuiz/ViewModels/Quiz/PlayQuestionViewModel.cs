using System.Collections.Generic;
using TwitterQuiz.Domain.DTO;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayQuestionViewModel
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public IList<PlayAnswerViewModel> Replies { get; set; }

        public PlayQuestionViewModel(QuestionInProgress question)
        {
            Question = question.Question;
            Sequence = question.Sequence;
            Replies = new List<PlayAnswerViewModel>();
            foreach (var reply in question.Replies)
            {
                Replies.Add(new PlayAnswerViewModel(reply));
            }
        }
    }
}