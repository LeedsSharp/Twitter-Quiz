using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common.Utils;

namespace TwitterQuiz.EventStore
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> GetEventStream<T>(this IEventStoreConnection eventStoreConnection)
        {
            var stream = eventStoreConnection.ReadStreamEventsBackward(typeof(T).Name.Pluralize(), 0, int.MaxValue, true);
            return stream.Events.Select(x => x.Event.Data.ParseJson<T>());
        }

        public static IEnumerable<T> SelectMany<T>(this IEventStoreConnection eventStoreConnection, Func<T, bool> filter)
        {
            var events = eventStoreConnection.GetEventStream<T>();
            return events.Where(filter);
        }

        public static bool Exists<T>(this IEventStoreConnection eventStoreConnection, Func<T, bool> filter)
        {
            var events = eventStoreConnection.GetEventStream<T>();
            return events.Any(filter);
        }

        public static T AppendToStream<T>(this IEventStoreConnection eventStoreConnection, T newEvent, string stream = "", object metaData = null)
        {
            var eventType = typeof(T).Name;
            if (stream == String.Empty)
            {
                stream = eventType.Pluralize();
            }
            var eventData = new List<EventData>
            {
                new EventData(Guid.NewGuid(), eventType, true, newEvent.ToJsonBytes(), new { metaData }.ToJsonBytes())
            };
            eventStoreConnection.AppendToStream(stream, ExpectedVersion.Any, eventData);
            return newEvent;
        }

        public static string Pluralize(this string @this, int count = 0)
        {
            return count == 1
                       ? @this
                       : System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(
                           new System.Globalization.CultureInfo("en-GB")).Pluralize(@this);
        }
    }
}
