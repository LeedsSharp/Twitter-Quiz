using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace TwitterQuiz.ViewModels.Quiz
{
    public class QuizDetailsViewModel
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
        [Required]
        public string Owner { get; set; }

        public QuizDetailsViewModel()
        {
            
        }

        public QuizDetailsViewModel(Domain.Quiz quiz)
        {
            Name = quiz.Name;
            Description = quiz.Description;
            Owner = quiz.Owner;
            Host = quiz.Host;
            StartDate = quiz.StartDate;
            FrequencyOfAnswers = quiz.FrequencyOfAnswers;
            FrequencyOfQuestions = quiz.FrequencyOfQuestions;
        }
    }
}