using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
using TwitterQuiz.Domain;
using TwitterQuiz.Domain.DTO;
using TwitterQuiz.Domain.QuizEvents;

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
            var id = GetNumberOfQuizzes(username);
            quiz.InternalName = string.Format("{0}-Quiz-{1}", username, id);
            quiz.Id = id;
            _eventStoreConnection.AppendToStream(quiz, streamName);
            return quiz;
        }

        public int GetNumberOfQuizzes(string username)
        {
            return GetQuizzes(username).Select(x => x.InternalName).Distinct().Count();
        }

        public IEnumerable<Quiz> GetQuizzes(string username)
        {
            var streamName = string.Format("{0}-Quizzes", username);
            var slice = _eventStoreConnection.ReadStreamEventsBackward(streamName, -1, int.MaxValue, true);

            var quizzes = slice.Events.Select(x => x.Event.Data.ParseJson<Quiz>()).ToList();

            var elements = new HashSet<int>();
            quizzes.RemoveAll(x => !elements.Add(x.Id));
            return quizzes;
        }

        public Quiz GetQuiz(int id, string username)
        {
            var streamName = string.Format("{0}-Quizzes", username);
            var slice = _eventStoreConnection.ReadStreamEventsBackward(streamName, -1, int.MaxValue, true);

            var quizzes = slice.Events.Select(x => x.Event.Data.ParseJson<Quiz>()).ToList();
            return quizzes.First(x => x.Id == id);
        }

        public QuizInProgress GetStartedQuiz(int quizId, string username)
        {
            var quiz = GetQuiz(quizId, username);

            var quizSoFar = new QuizInProgress();
            quizSoFar.Initialize(quiz);


            var slice = _eventStoreConnection.ReadStreamEventsForward(quiz.InternalName, StreamPosition.Start, int.MaxValue, true);

            foreach (var @event in slice.Events)
            {
                switch (@event.Event.EventType)
                {
                    case "RoundStarted":
                        var roundStartedEvent = @event.Event.Data.ParseJson<RoundStarted>();
                        quizSoFar.Rounds.Add(new RoundInProgress(quiz.Rounds.First(x => x.Sequence == roundStartedEvent.Round)));
                        break;
                    case "QuestionSent":
                        var questionSentEvent = @event.Event.Data.ParseJson<QuestionSent>();
                        var question = quiz.Rounds.First(x => x.Sequence == questionSentEvent.Round)
                                           .Questions.First(x => x.Sequence == questionSentEvent.Question);
                        quizSoFar.Rounds.First(x => x.Sequence == questionSentEvent.Round)
                                        .Questions.Add(new QuestionInProgress(question));
                        break;
                    case "QuizEnded":
                        quizSoFar.Complete = true;
                        break;
                }
            }

            return quizSoFar;
        }
    }
}
