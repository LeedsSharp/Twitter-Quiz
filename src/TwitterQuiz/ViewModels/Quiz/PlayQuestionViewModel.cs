using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class PlayQuestionViewModel
    {
        public string PanelClass
        {
            get { return Sent ? "panel-success" : "panel-default"; }
        }
        public string Question { get; set; }
        public int Sequence { get; set; }
        public IList<PlayAnswerViewModel> Replies { get; set; }
        public bool Sent { get; set; }

        public IList<PossibleAnswerViewModel> PossibleAnswers { get; set; }
        private const string _letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public PlayQuestionViewModel(Domain.Question question)
        {
            Question = question.Tweet;
            Sequence = question.Sequence;
            Sent = question.DateSent.HasValue;
            PossibleAnswers = new List<PossibleAnswerViewModel>();
            for (int i = 0; i < question.PossibleAnswers.Count; i++)
            {
                var answer = question.PossibleAnswers[i];
                PossibleAnswers.Add(new PossibleAnswerViewModel(answer, _letters[i], question.AnswersGathered));
            }
            Replies = new List<PlayAnswerViewModel>();
            foreach (var reply in question.Replies)
            {
                Replies.Add(new PlayAnswerViewModel(reply));
            }
        }
    }
}