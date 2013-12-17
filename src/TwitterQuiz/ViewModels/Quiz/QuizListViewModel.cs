using System;
using TwitterQuiz.Domain;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuizListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }

        public string PanelClass
        {
            get
            {
                switch (Status)
                {
                    case QuizStatus.Draft:
                        return "panel-info";
                    case QuizStatus.Ready:
                        return "panel-primary";
                    case QuizStatus.InProgress:
                        return "panel-success";
                    default:
                        return "panel-default";
                }
            }
        }

        public QuizStatus Status { get; set; }
    }
}