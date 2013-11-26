using System.Collections.Generic;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class EditQuizViewModel
    {
        public int Id { get; set; }
        public string StreamName { get; set; }
        public QuizDetailsViewModel Details { get; set; }
        public List<RoundViewModel> Rounds { get; set; }

        public EditQuizViewModel()
        {
            Rounds = new List<RoundViewModel>();
        }

        public EditQuizViewModel(Domain.Quiz quiz)
        {
            Details = new QuizDetailsViewModel(quiz);
            var round = new RoundViewModel
                {
                    Name = "Round 1",
                    Questions = new List<QuestionViewModel>
                        {
                            new QuestionViewModel
                                {
                                    Question = "Question 1",
                                    PossibleAnswers = new List<string>
                                        {
                                            "",
                                            "",
                                            ""
                                        }
                                },
                            new QuestionViewModel
                                {
                                    Question = "Question 1",
                                    PossibleAnswers = new List<string>
                                        {
                                            "",
                                            "",
                                            ""
                                        }
                                }
                        }
                };
            Rounds = new List<RoundViewModel> { round, round };
        }
    }
}