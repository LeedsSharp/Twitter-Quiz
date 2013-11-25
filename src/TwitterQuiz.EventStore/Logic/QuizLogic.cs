﻿using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;
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
            var id = GetNumberOfQuizzes(streamName);
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
            var slice = _eventStoreConnection.ReadStreamEventsBackward(streamName, 0, int.MaxValue, false);
            return slice.Events.Select(x => x.Event.Data.ParseJson<Quiz>());
        }
    }
}
