using EventStore.ClientAPI;
using TwitterQuiz.Domain;

namespace TwitterQuiz.EventStore.Logic
{
    public class QuizLogic
    {
        private readonly IEventStoreConnection _eventStoreConnection;

        public QuizLogic(IEventStoreConnection eventStoreConnection)
        {
            _eventStoreConnection = eventStoreConnection;
        }

        public Quiz CreateNewQuiz(Quiz quiz, string username)
        {
            var streamName = string.Format("{0}-Quizzes", username);
            _eventStoreConnection.AppendToStream(quiz, streamName);
            return quiz;
        }
    }
}
