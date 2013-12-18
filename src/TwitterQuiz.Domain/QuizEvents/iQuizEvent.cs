namespace TwitterQuiz.Domain.QuizEvents
{
    public class QuizEvent
    {
        public QuizEvent(IQuizEvent e)
        {
            Event = e;
        }

        public IQuizEvent Event;

        public Quiz ApplyChanges(Quiz quiz)
        {
            return Event.ApplyChanges(quiz);
        }
    }

    public interface IQuizEvent
    {
        Quiz ApplyChanges(Quiz quiz);
    }
}