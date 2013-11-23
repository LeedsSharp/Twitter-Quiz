using System;
using System.Configuration;
using System.Net;
using EventStore.ClientAPI;

namespace TwitterQuiz.EventStore
{
    public class EventStoreConnectionProvider
    {
        private static IEventStoreConnection _eventStoreConnection;

        public static IEventStoreConnection EventStore
        {
            get { return _eventStoreConnection ?? (_eventStoreConnection = CreateEventStoreConnection()); }
        }

        private static IEventStoreConnection CreateEventStoreConnection()
        {
            var ip = IPAddress.Parse(ConfigurationManager.AppSettings["Store.IP"]);
            int tcpPort = Convert.ToInt32(ConfigurationManager.AppSettings["Store.Port"]);
            var tcpEndPoint = new IPEndPoint(ip, tcpPort);

            return EventStoreConnection.Create(ConnectionSettings.Default, tcpEndPoint);
        }
    }
}