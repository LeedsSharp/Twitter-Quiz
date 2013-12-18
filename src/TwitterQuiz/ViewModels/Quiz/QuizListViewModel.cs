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
        public string Host { get; set; }
        public bool HostIsAuthenticated { get; set; }

        public string PanelClass
        {
            get
            {
                switch (Status)
                {
                    case QuizStatus.Draft:
                        return "panel-warning";
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