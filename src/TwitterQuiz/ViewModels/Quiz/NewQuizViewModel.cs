using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class NewQuizViewModel
    {
        [DisplayName("Quiz Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }
        [DisplayName("Quiz Host")]
        [Required]
        public string Host { get; set; }
        [DisplayName("Start")]
        [Required]
        public DateTime StartDate { get; set; }
        [DisplayName("Question Frequency")]
        [Required]
        [Digits]
        public int? FrequencyOfQuestions { get; set; }
        [DisplayName("Answer Frequency")]
        [Required]
        [Digits]
        public int? FrequencyOfAnswers { get; set; }

        public Domain.Quiz ToQuizModel()
        {
            return new Domain.Quiz
                {
                    Name = Name,
                    Description = Description,
                    Host = Host,
                    StartDate = StartDate,
                    FrequencyOfAnswers = FrequencyOfAnswers.HasValue ? FrequencyOfAnswers.Value : 10,
                    FrequencyOfQuestions = FrequencyOfQuestions.HasValue ? FrequencyOfQuestions.Value : 3
                };
        }
    }
}