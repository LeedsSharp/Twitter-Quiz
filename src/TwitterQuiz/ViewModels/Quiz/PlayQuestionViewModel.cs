using System.Collections.Generic;
using TwitterQuiz.Domain.DTO;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayQuestionViewModel
    {
        public string Question { get; set; }
        public int Sequence { get; set; }
        public IList<string> Replies { get; set; }

        public PlayQuestionViewModel(QuestionInProgress question)
        {
            Question = question.Question;
            Sequence = question.Sequence;
            Replies = question.Replies;
        }
    }
}