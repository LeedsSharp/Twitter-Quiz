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
            var streamName = string.Format("User-{0}-Quizzes", username);
            var id = GetNumberOfQuizzes(username);
            quiz.InternalName = string.Format("Quiz-{0}{1}", username, id);
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
            var streamName = string.Format("User-{0}-Quizzes", username);
            var slice = _eventStoreConnection.ReadStreamEventsBackward(streamName, -1, int.MaxValue, true);

            var quizzes = slice.Events.Select(x => x.Event.Data.ParseJson<Quiz>()).ToList();

            var elements = new HashSet<int>();
            quizzes.RemoveAll(x => !elements.Add(x.Id));
            return quizzes;
        }

        public Quiz GetQuiz(int id, string username)
        {
            var streamName = string.Format("User-{0}-Quizzes", username);
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

            foreach (var roundStartedEvent in slice.Events.Where(x => x.Event.EventType == "RoundStarted").Select(@event => @event.Event.Data.ParseJson<RoundStarted>()))
            {
                quizSoFar.Rounds.Add(new RoundInProgress(quiz.Rounds.First(x => x.Sequence == roundStartedEvent.Round)));
            }

            foreach (var questionSentEvent in slice.Events.Where(x => x.Event.EventType == "QuestionSent").Select(@event => @event.Event.Data.ParseJson<QuestionSent>()))
            {
                var question = quiz.Rounds.First(x => x.Sequence == questionSentEvent.Round)
                                           .Questions.First(x => x.Sequence == questionSentEvent.Question);
                        quizSoFar.Rounds.First(x => x.Sequence == questionSentEvent.Round)
                                        .Questions.Add(new QuestionInProgress(question));
            }

            foreach (var answerReceivedEvent in slice.Events.Where(x => x.Event.EventType == "AnswerReceived").Select(@event => @event.Event.Data.ParseJson<AnswerReceived>()))
            {
                var answer = new Answer
                {
                    Player = new Player
                    {
                        Username = answerReceivedEvent.Username,
                        ImageUrl = answerReceivedEvent.ImageUrl
                    },
                    AnswerConent = answerReceivedEvent.Answer
                };
                var round = quizSoFar.Rounds.FirstOrDefault(x => x.Sequence == answerReceivedEvent.Round);
                if (round != null)
                {
                    var qtion = round.Questions.FirstOrDefault(x => x.Sequence == answerReceivedEvent.Question);
                    if (qtion != null)
                    {
                        qtion.Replies.Add(answer);
                        var elements = new HashSet<string>();
                        qtion.Replies.RemoveAll(x => !elements.Add(x.Player.Username));
                    }
                }
            }

            quizSoFar.Complete = slice.Events.Any(x => x.Event.EventType == "QuizEnded");

            return quizSoFar;
        }

        public void CreateFakeAnswer(AnswerReceived answer, string selectedQuiz)
        {
            var slice = _eventStoreConnection.ReadStreamEventsBackward(selectedQuiz, StreamPosition.End, int.MaxValue, true);

            if (!slice.Events.Any() || slice.Events.Any(x => x.Event.EventType == "QuizEnded")) return;

            answer.Round = slice.Events.First(x => x.Event.EventType == "RoundStarted")
                                .Event.Data.ParseJson<RoundStarted>()
                                .Round;
            answer.Question = slice.Events.First(x => x.Event.EventType == "QuestionSent")
                                .Event.Data.ParseJson<QuestionSent>()
                                .Question;

            _eventStoreConnection.AppendToStream(answer, selectedQuiz);
        }
    }
}
