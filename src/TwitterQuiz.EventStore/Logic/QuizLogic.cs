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

        public Quiz AppendToStream(Quiz quiz)
        {
            _eventStoreConnection.AppendToStream(quiz);
            return quiz;
        }
    }
}
